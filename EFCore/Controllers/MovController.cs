using Microsoft.AspNetCore.Mvc;
using EFCore.Repositories;
using EFCore.Models;
using Microsoft.AspNetCore.Authorization;

namespace EFCore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MovController : ControllerBase
    {

        private readonly IMovRepository _movRepository;
        public MovController(IMovRepository movRepository)
        {
            _movRepository = movRepository;
        }

        [Authorize]
        [HttpGet]
        public async Task<IEnumerable<Mov>> GetMovs()
        {
            return await _movRepository.GetAll();
        }

        [Authorize]
        [HttpGet("GetById/{id:int}")]
        public async Task<ActionResult<Mov>> GetMovs(int id)
        {
            return await _movRepository.GetById(id);
        }

        [Authorize]
        [HttpGet("GetListByIdConta/{id:int}")]
        public async Task<IEnumerable<Mov>> GetMovsByIdConta(int id)
        {
            return await _movRepository.GetListByConta(id);
        }

        [Authorize]
        [HttpPost("Create")]
        public async Task<ActionResult<Mov>> CreateMovs([FromBody] Mov mov)
        {
            var novoMov = await _movRepository.Create(mov);
            return CreatedAtAction(nameof(GetMovs), new { id = novoMov.IdMov }, novoMov);
        }

        [Authorize]
        [HttpPut("Update")]
        public async Task<String> UpdateMovs([FromBody] Mov mov)
        {
            await _movRepository.Update(mov);
            return $"Mov de id: {mov.IdMov} atualizada com sucesso!";
        }

    }
}

