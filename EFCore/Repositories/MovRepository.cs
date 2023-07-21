using EFCore.Models;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

namespace EFCore.Repositories
{
    public class MovRepository : IMovRepository
    {
        public readonly SistemaBancoContext _context;

        public MovRepository(SistemaBancoContext context)
        {
            _context = context;
        }

        public async Task<Mov> Create(Mov mov)
        {
            _context.Movs.Add(mov);
            await _context.SaveChangesAsync();
            return mov;
        }

        public async Task Delete(int id)
        {
            var toDelete = await _context.Movs.FindAsync(id);
            _context.Movs.Remove(toDelete);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Mov>> GetAll()
        {
            return await _context.Movs.ToListAsync();
        }

        public async Task<Mov> GetById(int id)
        {
            return await _context.Movs.FindAsync(id);
        }

        public async Task<List<Mov>> GetListByConta(int id)
        {
            return await _context.Movs.Where(u=>u.IdConta == id).ToListAsync();
        }

        public async Task Update(Mov mov)
        {
            _context.Entry(mov).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
