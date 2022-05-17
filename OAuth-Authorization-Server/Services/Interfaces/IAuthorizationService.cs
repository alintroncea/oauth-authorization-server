using Microsoft.AspNetCore.Mvc;

namespace OAuth_Authorization_Server.Services.Interfaces
{
    public interface IAuthorizationService
    {
        Task<IActionResult> AuthorizeAsync(
            string client_id,
            string scope,
            string response_type,
            string redirect_uri,
            string state,
            string code_challenge,
            string code_challenge_method
            );

        //Task<IActionResult> LoginAsync(
        //    AuthUser authLogin,
        //    string redirect_uri,
        //    string state,
        //    string scope
        //    );
    }
}
