using Microsoft.AspNetCore.Mvc;
using EFCore.Repositories;
using EFCore.Models;
using EFCore.Services;
using System.Drawing;
using NuGet.Protocol;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Authorization;

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
        [Authorize]
        public async Task<IEnumerable<Conta>> GetContas()
        {
            return await _contaRepository.GetAll();
        }

        [HttpGet]
        [Route("TESTE")]
        [Authorize]
        public string TESTE()
        {
            return String.Format("{0}", User.Identity.Name);
        }

        [Authorize]
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
        [Authorize]
        public async Task<String> UpdateContas([FromBody] Conta conta)
        {
            await _contaRepository.Update(conta);
            return $"Conta de id: {conta.CodConta} atualizada com sucesso!";
        }

        [HttpPut("/Transactions/Deposit")]
        [Authorize(Roles = "client")]
        public async Task<IActionResult> DepositContas(int id , decimal value)
        {
            var conta = await _contaRepository.GetById( id );

            if (conta == null) return BadRequest("Account not found!");

            await _contaRepository.Deposit(conta, value);

            return Ok((new {codConta=id , value=value , result=conta.Saldo}).ToJson());
        }

        [HttpPut("/Transactions/Draw")]
        [Authorize(Roles = "client")]
        public async Task<IActionResult> DrawContas([FromBody]DrawForm form)
        {

            int id = form.codConta;
            decimal value = form.valor;
            string password = form.senha;

            var conta = await _contaRepository.GetById(id);
            if (conta == null) return BadRequest("Account not found!");
            else
            {
                try
                {
                    await _contaRepository.Draw(conta, value, password);
                    return Ok((new { codConta = id, value = value, result = conta.Saldo }).ToJson());
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
        [Authorize(Roles = "client")]
        public async Task<IActionResult> TransferContas([FromBody]TransferForm form)
        {
            
            var contaRemetente = await _contaRepository.GetById(form.idRemetente);
            var contaDestino = await _contaRepository.GetById(form.idDestinatario);

            if (contaRemetente == null) return BadRequest("Sender account not found!");
            else if (contaDestino == null) return BadRequest("Receiver account not found!");
            else
            {
                try
                {
                    await _contaRepository.Transfer(contaRemetente, contaDestino, form.valor, form.senha);
                    return Ok((new { codConta = contaRemetente.CodConta, value = form.valor, result = contaRemetente.Saldo }).ToJson());
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
        public int idRemetente { get; set; }
        public int idDestinatario { get; set; }
        public decimal valor { get; set; }
        public string senha { get; set; } = null!;

    }

    public class DrawForm
    {
        public int codConta { get; set; }
        public decimal valor { get; set; }
        public string senha { get; set; } = null!;

    }

}
