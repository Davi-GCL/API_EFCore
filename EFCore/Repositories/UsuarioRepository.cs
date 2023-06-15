using EFCore.Models;

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
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Usuario>> Get()
        {
            throw new NotImplementedException();
        }

        public Task<Usuario> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task Update(Usuario usuario)
        {
            throw new NotImplementedException();
        }
    }
}
