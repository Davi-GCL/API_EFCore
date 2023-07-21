using Microsoft.AspNetCore.Mvc;
using EFCore.Repositories;
using EFCore.Models;
using EFCore.Services;
using System.Drawing;

namespace EFCore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContaController : ControllerBase
    {

        private readonly IContaRepository _contaRepository;
        public ContaController(IContaRepository contaRepository)
        {
            _contaRepository = contaRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<Conta>> GetContas()
        {
            return await _contaRepository.GetAll();
        }


        [HttpGet("GetById/{id:int}")]
        public async Task<ActionResult<Conta>> GetContas(int id)
        {
            return await _contaRepository.GetById(id);
        }

        [HttpPost("Create")]
        public async Task<ActionResult<Conta>> CreateContas([FromBody] Conta conta)
        {
            var novoConta = await _contaRepository.Create(conta);
            return CreatedAtAction(nameof(GetContas), new { id = novoConta.CodConta }, novoConta);
        }

        [HttpPut("Update")]
        public async Task<String> UpdateContas([FromBody] Conta conta)
        {
            await _contaRepository.Update(conta);
            return $"Conta de id: {conta.CodConta} atualizada com sucesso!";
        }

        [HttpPut("/Transactions/Deposit")]
        public async Task<IActionResult> DepositContas(int id , decimal value)
        {
            var conta = await _contaRepository.GetById( id );

            if (conta == null) return BadRequest("Account not found!");

            await _contaRepository.Deposit(conta, value);

            return Ok($"Sucefully deposited {value:C} in id: {id}");
        }

        [HttpPut("/Transactions/Draw")]
        public async Task<IActionResult> DrawContas(int id, decimal value, string password)
        {
            
            var conta = await _contaRepository.GetById(id);
            if (conta == null) return BadRequest("Account not found!");
            else
            {
                try
                {
                    await _contaRepository.Draw(conta, value, password.GerarHash());
                    return Ok($"Balance after withdrawal:{conta.Saldo}");
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    return BadRequest("OutOfRange: " + ex.Message);
                }
                catch (ArgumentException ex)
                {
                    return BadRequest("Argument Exception: " + ex.Message);
                }
                catch (Exception ex)
                {
                    return BadRequest("General Exception: " + ex.Message);
                }
            }
            
        }

        [HttpPut("/Transactions/Transfer")]
        public async Task<IActionResult> TransferContas([FromBody]TransferForm form)
        {
            
            var contaRemetente = await _contaRepository.GetById(form.senderId);
            var contaDestino = await _contaRepository.GetById(form.receiverId);

            if (contaRemetente == null) return BadRequest("Sender account not found!");
            else if (contaDestino == null) return BadRequest("Receiver account not found!");
            else
            {
                try
                {
                    await _contaRepository.Transfer(contaRemetente, contaDestino, form.value, form.password);
                    
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    return BadRequest("Out of Range: " + ex.Message);
                }
                catch (ArgumentException ex)
                {
                    return BadRequest("Argument Exception: " + ex.Message);
                }
                catch (Exception ex)
                {
                    return BadRequest("General Exception: " + ex.Message);
                }
                
                return Ok($"Balance after withdrawal: {contaRemetente.Saldo}");
            }

            //var result = await DrawContas(senderId, value, password);

            //if (result.GetType() == typeof(BadRequestObjectResult)) return result;

            //var resultDeposit = await DepositContas(receiverId, value);

            //if (resultDeposit.GetType() == typeof(BadRequestObjectResult)) return resultDeposit;

            //return Ok("Deposited sucefully!");
        }
    }
    public class TransferForm
    {
        public int senderId { get; set; }
        public int receiverId { get; set; }
        public decimal value { get; set; }
        public string password { get; set; } = null!;

    }
}
