using EFCore.Models;
using EFCore.Services;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using NuGet.Protocol.Plugins;

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
            var array = await _context.Usuarios.ToArrayAsync();
            var exceptionDict = new Dictionary<string, bool>();

            foreach (var item in array)
            {
                if (item.Cpf == usuario.Cpf || item.Email == usuario.Email || item.Telefone == usuario.Telefone)
                {
                    exceptionDict.Add("Cpf", item.Cpf == usuario.Cpf);
                    exceptionDict.Add("Email", item.Email == usuario.Email);
                    exceptionDict.Add("Telefone", item.Telefone == usuario.Telefone);
                }  
            }

            throw new Exception("CPF,Email e Telefone ja existe");

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

        public async Task<int> Check(UsuarioAuthForm login)
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
                        return item.Id;
                    }
                }
                
            }

            return 0;
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
