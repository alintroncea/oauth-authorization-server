using Microsoft.AspNetCore.Mvc;

namespace OAuth_Authorization_Server.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class OAuthController : ControllerBase
    {

        [HttpGet]
        [Route("authorize")]
        public async Task<IActionResult> AuthorizeAsync(
          string client_id,
          string scope,
          string response_type,
          string redirect_uri,
          string state
          )
        {

            var x = 2;

            return Ok();
        }
    }
}
