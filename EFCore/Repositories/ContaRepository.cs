using EFCore.Models;
using EFCore.Services;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using NuGet.Protocol.Plugins;
using System.Drawing;

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
            
            return await _context.Contas.FindAsync(id);
        }

        public async Task Update(Conta conta)
        {
            _context.Entry(conta).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task Deposit(Conta conta, decimal value)
        {
            if(value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), $"{nameof(value)} should be above zero.");
            }

            conta.Saldo += value;

            await _context.SaveChangesAsync();
        }

        public async Task Draw(Conta conta, decimal value, string pwd)
        {
            pwd = pwd.GerarHash();
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), $"{nameof(value)} should be above zero.");
            }
            if(pwd == conta.Senha) 
            {
                if (conta.Saldo < value)
                {
                    throw new Exception($"Balance is not enough for draw {value:C}");
                }
                else
                {
                    conta.Saldo -= value;
                    await _context.SaveChangesAsync();
                }
            }
            else
            {
                throw new ArgumentException("Incorrect password", "password");
            }
        }

        public void History(int idConta, decimal valor, string tipo)
        {
            var aux = new Mov() { 
                IdConta = idConta,
                Valor = valor,
                DataHora = DateTime.Now,
                Tipo = tipo
            };

            _context.Movs.Add(aux);
            _context.SaveChanges();
        }

        public async Task Transfer(Conta senderAccount, Conta receiverAccount, decimal value, string password)
        {
            await Draw(senderAccount, value, password);
            await Deposit(receiverAccount, value);
            History(senderAccount.CodConta, (-1)*value, "Transferencia");
            History(receiverAccount.CodConta, value, "Transferencia");
        }
    }
}
