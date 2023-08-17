using EFCore.Models;
using EFCore.Services;
using MessagePack.Formatters;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using NuGet.Protocol.Plugins;
using System.ComponentModel;
using System.Diagnostics;

namespace EFCore.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        public readonly SistemaBancoContext _context;
        
        public UsuarioRepository(SistemaBancoContext context)
        {
            _context = context;
        }

        public int GetUserByCPF(string cpf)
        {
            var user =  _context.Usuarios.Where(u => u.Cpf == cpf).FirstOrDefault();

            return user != null ? user.Id : 0;
        }
        public int GetUserByEmail(string email)
        {
            var user = _context.Usuarios.Where(u => u.Email == email).FirstOrDefault();

            return user != null ? user.Id : 0;
        }
        public int GetUserByTel(string telefone)
        {
            var user = _context.Usuarios.Where(u => u.Telefone == telefone).FirstOrDefault();

            return user != null ? user.Id : 0;
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

        public async Task UpdatePassword(Usuario usuario, string password)
        {
            usuario.SetSenha = password;
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
