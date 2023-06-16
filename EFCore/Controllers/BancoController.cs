using Microsoft.AspNetCore.Mvc;
using EFCore.Repositories;
using EFCore.Models;

namespace EFCore.Controllers
{
    [ApiController]
    [Route("[controller]/Usuarios")]
    public class BancoController : ControllerBase
    {

        private readonly IUsuarioRepository _usuarioRepository;
        public BancoController(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<Usuario>> GetUsuarios()
        {
            return await _usuarioRepository.Get();
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuarios(int id)
        {
            return await _usuarioRepository.Get(id);
        }

        [HttpPost]
        public async Task<ActionResult<Usuario>> CreateUsuarios([FromBody] Usuario usuario)
        {
            var novoUsuario = await _usuarioRepository.Create(usuario);
            return CreatedAtAction(nameof(GetUsuarios), new { id = novoUsuario.Id }, novoUsuario);
        }




    }
}