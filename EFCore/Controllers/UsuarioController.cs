using Microsoft.AspNetCore.Mvc;
using EFCore.Repositories;
using EFCore.Models;
using NuGet.Protocol;

namespace EFCore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuarioController : ControllerBase
    {

        private readonly IUsuarioRepository _usuarioRepository;
        public UsuarioController(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<Usuario>> GetUsuarios()
        {
            return await _usuarioRepository.GetAll();
        }


        [HttpGet("GetById/{id:int}")]
        public async Task<ActionResult<Usuario>> GetUsuarios(int id)
        {
            return await _usuarioRepository.GetById(id);
        }

        [HttpPost("Create")]
        public async Task<ActionResult<Usuario>> CreateUsuarios([FromBody] Usuario usuario)
        {
            var novoUsuario = await _usuarioRepository.Create(usuario);
            return CreatedAtAction(nameof(GetUsuarios), new { id = novoUsuario.Id }, novoUsuario);
        }

        [HttpPut("Update")]
        public async Task<String> UpdateUsuarios([FromBody] Usuario usuario)
        {
            await _usuarioRepository.Update(usuario);
            return $"usuario de id: {usuario.Id} atualizada com sucesso! -> {usuario.ToJson()}";
        }

        [HttpPost("Check")]
        public async Task<String> Auth([FromBody] UsuarioAuthForm loginForm)
        {
            var resp = await _usuarioRepository.Check(loginForm);
            return resp;
        }

    }
}