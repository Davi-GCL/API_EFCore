using EFCore.Models;
using EFCore.Repositories;
using EFCore.Services;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;

namespace EFCore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {

        private readonly IContaRepository _contaRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        public AuthController(IContaRepository contaRepository, IUsuarioRepository usuarioRepository)
        {
            _contaRepository = contaRepository;
            _usuarioRepository = usuarioRepository;
        }

        [HttpPost("LoginUsuario")]
        public async Task<IActionResult> LoginUsuario([FromBody]UsuarioAuthForm loginForm)
        {
            int usuarioId = await _usuarioRepository.Check(loginForm);
            if (usuarioId > 0) {
                var usuario = await _usuarioRepository.GetById(usuarioId);
                var token = TokenServices.generateToken(usuario, "client");

                return Ok(new {id=usuarioId , valid = true, token = token});
            }

            return BadRequest("invalid Cpf or password");
        }

        //[HttpPost("Auth")]
        //public async Task<string> Auth([FromBody] UsuarioAuthForm loginForm)
        //{
        //    int resp = await _usuarioRepository.Check(loginForm);
        //    bool auxBool;

        //    auxBool = resp > 0;
        //    var obj = new
        //    {
        //        valid = auxBool,
        //        id = resp
        //    };
        //    return obj.ToJson();
        //}

    }

}
