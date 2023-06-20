using EFCore.Models;

namespace EFCore.Repositories
{
    public interface IContaRepository
    {
        Task<IEnumerable<Conta>> GetAll();
        Task<Conta> GetById(int id);

        Task<Conta> Create(Conta conta);

        Task Update(Conta conta);

        Task Delete(int id);
    }
}
