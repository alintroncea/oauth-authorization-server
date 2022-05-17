using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OAuth_Authorization_Server.Data;
using OAuth_Authorization_Server.Helpers;
using OAuth_Authorization_Server.Services.DataTransferObjects.UserService;

namespace OAuth_Authorization_Server.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class OAuthController : ControllerBase
    {
        private readonly AppDbContext _db;


        public OAuthController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        [Route("authorize")]
        public async Task<ActionResult<AuthorizationResDto>> AuthorizeAsync(
          string client_id,
          string scope,
          string response_type,
          string redirect_uri,
          string state
          )
        {
            var client = await _db.OAuthClients
                .Where(x => x.ClientId.Equals(client_id))
                .Include(x => x.OAuthScopes)
                .FirstOrDefaultAsync();

            if (client is not null)
            {
                var scope_target = client.OAuthScopes.Last().Name;

                if (scope.EndsWith(scope_target))
                {
                    var newAuthResponse = new AuthorizationResDto()
                    {
                        RedirectUri = redirect_uri,
                        Scope = scope,
                        State = state
                    };

                    client.CurrentState = state;

                    await _db.SaveChangesAsync();
                    return Ok(newAuthResponse);
                }

                return BadRequest(new ResponseMessage { Message = "Invalid scope." });
            }

            return BadRequest(new ResponseMessage { Message = "Client app not found." });
        }

        [Authorize]
        [HttpGet]
        [Route("getAuthorization")]
        public IActionResult GetAuthorization()
        {
            return RedirectToAction("Index");
        }
    }
}

