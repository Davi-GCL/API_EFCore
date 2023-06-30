using EFCore.Models;
using Microsoft.EntityFrameworkCore;

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
        //public Task<string> Check(Usuario usuario)
        //{
        //    if (usuario)
        //    {
        //        return "usuario";
        //    }
        //}

    }
}
