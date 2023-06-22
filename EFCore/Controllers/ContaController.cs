using Microsoft.AspNetCore.Mvc;
using EFCore.Repositories;
using EFCore.Models;

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

    }
}
