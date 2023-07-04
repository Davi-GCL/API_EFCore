using EFCore.Models;
using EFCore.Services;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;

namespace EFCore.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        public readonly SistemaBancoContext _context;
        
        public UsuarioRepository(SistemaBancoContext context)
        {
            _context = context;
        }

        public async Task<Usuario> Create(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return usuario;
        }

        public async Task Delete(int id)
        {
            var toDelete = await _context.Usuarios.FindAsync(id);
            _context.Usuarios.Remove(toDelete);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Usuario>> GetAll()
        {
            //return await _context.Usuarios.ToListAsync();
            return await _context.Usuarios.Include(u => u.Conta).AsNoTracking().ToListAsync();
        }

        public async Task<Usuario> GetById(int id)
        {
            return await _context.Usuarios.FindAsync(id);
        }

        public async Task Update(Usuario usuario)
        {
            _context.Entry(usuario).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<string> Check(UsuarioAuthForm login)
        {

            login.Senha = login.Senha.GerarHash();

            var array = await _context.Usuarios.ToArrayAsync();
            var array2 = await _context.Usuarios.Include(u => u.Conta).AsNoTracking().ToArrayAsync();

            foreach (var item in array)
            {
                if (item.Cpf == login.Cpf)
                {
                    if(item.Senha == login.Senha)
                    {
                        return $"Login de CPF e Senha validos!";
                    }
                }
                
            }

            return $"Usuario com o Cpf: {login.Cpf} Não encontrado!";
        }

        //public async Task<string> Check(string search)
        //{
        //    var array = await _context.Usuarios.ToArrayAsync();
        //    var array2 = await _context.Usuarios.Include(u => u.Conta).AsNoTracking().ToArrayAsync();

        //    foreach (var item in array2)
        //    {
        //        if(item.Nome == search)
        //        {
        //            foreach(var i in item.Conta)
        //            {
        //                return i.CodConta;
        //            }
        //        }
        //    }

        //    return $"Usuario com o nome: {search} Não encontrado!";
        //}

    }
}
