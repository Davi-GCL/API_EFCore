using EFCore.Models;
using Microsoft.EntityFrameworkCore;

namespace EFCore.Repositories
{
    public class ContaRepository : IContaRepository
    {
        public readonly SistemaBancoContext _context;
        
        public ContaRepository(SistemaBancoContext context)
        {
            _context = context;
        }

        public async Task<Conta> Create(Conta conta)
        {
            _context.Contas.Add(conta);
            await _context.SaveChangesAsync();
            return conta;
        }

        public async Task Delete(int id)
        {
            var toDelete = await _context.Contas.FindAsync(id);
            _context.Contas.Remove(toDelete);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Conta>> GetAll()
        {
            return await _context.Contas.Include(u=>u.IdUsuarioNavigation).AsNoTracking().ToListAsync();
            //return await _context.Contas.ToListAsync();
        }

        public async Task<Conta> GetById(int id)
        {
            
            return await _context.Contas.FindAsync(id.ToString());
        }

        public async Task Update(Conta conta)
        {
            _context.Entry(conta).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
