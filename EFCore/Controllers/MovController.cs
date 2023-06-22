using Microsoft.AspNetCore.Mvc;
using EFCore.Repositories;
using EFCore.Models;

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

        [HttpGet]
        public async Task<IEnumerable<Mov>> GetMovs()
        {
            return await _movRepository.GetAll();
        }


        [HttpGet("GetById/{id:int}")]
        public async Task<ActionResult<Mov>> GetMovs(int id)
        {
            return await _movRepository.GetById(id);
        }

        [HttpPost("Create")]
        public async Task<ActionResult<Mov>> CreateMovs([FromBody] Mov mov)
        {
            var novoMov = await _movRepository.Create(mov);
            return CreatedAtAction(nameof(GetMovs), new { id = novoMov.IdMov }, novoMov);
        }

        [HttpPut("Update")]
        public async Task<String> UpdateMovs([FromBody] Mov mov)
        {
            await _movRepository.Update(mov);
            return $"Mov de id: {mov.IdMov} atualizada com sucesso!";
        }

    }
}

