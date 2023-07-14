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
        public async Task<IActionResult> CreateUsuarios([FromBody] Usuario usuario)
        {
            Usuario novoUsuario;
            var dadosExistentes = new {
                cpf = _usuarioRepository.GetUserByCPF(usuario.Cpf) != 0 ,
                email = _usuarioRepository.GetUserByEmail(usuario.Email) != 0 ,
                telefone = _usuarioRepository.GetUserByTel(usuario.Telefone) != 0
            };

            if ( dadosExistentes.cpf ||
                 dadosExistentes.email ||
                 dadosExistentes.telefone )
            {
                return BadRequest(dadosExistentes.ToJson());
            }

            
             novoUsuario = await _usuarioRepository.Create(usuario);
             CreatedAtAction(nameof(GetUsuarios), new { id = novoUsuario.Id }, novoUsuario);
           
            
            return Ok(novoUsuario.ToJson());
        }

        //[HttpPost("Create")]
        //public async Task<ActionResult<Usuario>> CreateUsuarios([FromBody] Usuario usuario)
        //{
        //    Usuario novoUsuario;

        //    novoUsuario = await _usuarioRepository.Create(usuario);
        //    return CreatedAtAction(nameof(GetUsuarios), new { id = novoUsuario.Id }, novoUsuario);
        //}

        [HttpPut("Update")]
        public async Task<String> UpdateUsuarios([FromBody] Usuario usuario)
        {
            await _usuarioRepository.Update(usuario);
            return $"usuario de id: {usuario.Id} atualizada com sucesso! -> {usuario.ToJson()}";
        }

        [HttpPost("Auth")]
        public async Task<string> Auth([FromBody] UsuarioAuthForm loginForm)
        {
            int resp = await _usuarioRepository.Check(loginForm);
            bool auxBool; 
            
            auxBool = resp > 0;
            var obj = new
            {
                valid = auxBool,
                id = resp
            };
            return obj.ToJson();
        }

    }
}