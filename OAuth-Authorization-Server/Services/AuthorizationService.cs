using Microsoft.AspNetCore.Mvc;
using OAuth_Authorization_Server.Services.Interfaces;

namespace OAuth_Authorization_Server.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        public Task<IActionResult> AuthorizeAsync(string client_id, string scope, string response_type, string redirect_uri, string state, string code_challenge, string code_challenge_method)
        {
            throw new NotImplementedException();
        }
    }
}
