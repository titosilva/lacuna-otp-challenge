using Lacuna.Api.Commands;
using Lacuna.Domain.Entities;
using Lacuna.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Lacuna.Api.Controllers
{
    [ApiController]
    [Route("v1/lacuna")]
    public class LacunaController : ControllerBase
    {

        private readonly ILogger<LacunaController> _logger;

        public LacunaController(ILogger<LacunaController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [Route("login")]
        public string Login(
            [FromBody] LoginCommand command,
            [FromServices] IApiService service
        )
        {
            var result = service.Login(new User(command.Name, command.Password));
            return result==null? "FAIL" : result.ToString();
        }

        [HttpPost]
        [Route("user")]
        public string CreateUser(
            [FromBody] CreateUserCommand command,
            [FromServices] IApiService service
        )
        {
            command.Validate();
            if(!command.IsValid())
                return "FAIL";

            return service.CreateUser(new User(command.Name, command.Password))? "OK": "FAIL";
        }

        [HttpGet]
        [Route("secret")]
        public string GetSecret(
            [FromQuery] string token,
            [FromServices] IApiService service
        )
        {
            var result = service.GetSecret(new Token(token));

            return result==null? "FAIL" : result;
        }
    }
}
