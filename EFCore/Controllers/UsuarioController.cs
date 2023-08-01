using Microsoft.AspNetCore.Mvc;
using EFCore.Repositories;
using EFCore.Models;
using NuGet.Protocol;
using Microsoft.IdentityModel.Tokens;

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
                Cpf = _usuarioRepository.GetUserByCPF(usuario.Cpf) != 0,
                Email = _usuarioRepository.GetUserByEmail(usuario.Email) != 0,
                Telefone = _usuarioRepository.GetUserByTel(usuario.Telefone) != 0
            };

            if (dadosExistentes.Cpf ||
                 dadosExistentes.Email ||
                 dadosExistentes.Telefone)
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

        [HttpPost("Update")]
        public async Task<String> UpdateUsuarios([FromBody] Usuario usuario)
        {

            await _usuarioRepository.Update(usuario);
            return $"usuario de id: {usuario.Id} atualizada com sucesso! -> {usuario.ToJson()}";
        }

        [HttpPost("Update/Password")]
        public async Task<IActionResult> UpdateUsuarios([FromBody]UpdateForm Form)
        {
            int userId = _usuarioRepository.GetUserByCPF(Form.Cpf);
            var User = await _usuarioRepository.GetById(userId);
            if (User != null)
            {
                if(User.Email == Form.Email)
                {
                    await _usuarioRepository.UpdatePassword(User,Form.Senha);
                    return Ok();
                }
                else
                {
                    return BadRequest("This email does not check!");
                }
            }
            else
            {
                return BadRequest("User not found!");
            }

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
        
        //public void CopyProperties(object obj)
        //{
        //    Type objType = obj.GetType();

        //    System.Reflection.PropertyInfo[] properties = objType.GetProperties();
        //    for(int i=0; i<properties.Length; i++)
        //    {
        //        Console.WriteLine(properties[i].Name+": "+ properties[i].GetValue(obj));
        //    }
        //}
        public class UpdateForm
        {
            public string Cpf { get; set; } = null!;
            public string Email { get; set; } = null!;
            public string Senha { get; set; } = null!;
        }
    }
}