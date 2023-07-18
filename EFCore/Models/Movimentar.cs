namespace EFCore.Models
{
    public class Movimentar
    {
        public void Depositar(Conta conta, decimal valor)
        {
            conta.Saldo = 0;
        }
    }
}
