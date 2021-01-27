using Lacuna.Api.Commands;
using Lacuna.Domain.Entities;
using Lacuna.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Lacuna.Api.Controllers
{
    [ApiController]
    [Route("v1/token")]
    public class TokenController : ControllerBase
    {
        private readonly ILogger<LacunaController> _logger;

        public TokenController(ILogger<LacunaController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("usernamePosition")]
        public int UsernamePosition(
            [FromServices] ITokenAnalyser service
        )
        {
            try{
                return service.DiscoverUsernamePosition();
            }catch{
                return -1;
            }
        }

        [HttpGet]
        [Route("tokenMask")]
        public string TokenMask(
            [FromServices] ITokenAnalyser service
        )
        {
            try{
                var usernamePosition = service.DiscoverUsernamePosition();
                return service.DiscoverTokenMask(usernamePosition).ToString();
            }catch{
                return "Error while trying to get a token mask";
            }
        }

        [HttpGet]
        [Route("paddingChar")]
        public string PaddingChar(
            [FromServices] ITokenAnalyser service
        )
        {
            try{
                var usernamePosition = service.DiscoverUsernamePosition();
                var tokenMask = service.DiscoverTokenMask(usernamePosition);
                return service.DiscoverPaddingChar(usernamePosition, tokenMask).ToString();
            }catch{
                return "Error while trying to discover the padding char";
            }
        }

        [HttpGet]
        [Route("forge")]
        public string Forge(
            [FromQuery] string username,
            [FromServices] ITokenAnalyser service
        )
        {
            try{
                var usernamePosition = service.DiscoverUsernamePosition();
                var tokenMask = service.DiscoverTokenMask(usernamePosition);
                var paddingChar = service.DiscoverPaddingChar(usernamePosition, tokenMask);
                return service.ForgeToken(username, usernamePosition, tokenMask, paddingChar);
            }catch{
                return "Error while trying to forge a token";
            }
        }
    }
}
