using Microsoft.AspNetCore.Mvc;

namespace EFCore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BancoController : ControllerBase
    {

        private readonly ILogger<BancoController> _logger;

        public BancoController(ILogger<BancoController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{id}")]
        public string Get(int id)
        {
            return $"Seu id é {id}";
        }
    }
}