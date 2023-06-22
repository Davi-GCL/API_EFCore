using EFCore.Models;

namespace EFCore.Repositories
{
    public interface IMovRepository
    {
        Task<IEnumerable<Mov>> GetAll();
        Task<Mov> GetById(int id);

        Task<Mov> Create(Mov mov);

        Task Update(Mov mov);

        Task Delete(int id);
    }
}

