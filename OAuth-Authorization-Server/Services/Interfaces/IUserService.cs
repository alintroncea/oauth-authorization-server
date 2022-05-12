using OAuth_Authorization_Server.Services.DataTransferObjects.UserService;
using System;
using System.Threading.Tasks;

namespace WebApi.Services.Interfaces;

/// <summary>
/// The user service interface.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Authenticates the specified user.
    /// </summary>
    /// <param name="dto">The <see cref="AuthenticateReqDto"/> data transfer object.</param>
    /// <returns>The user info with token.</returns>
    Task<AuthenticateResDto> AuthenticateAsync(AuthenticateReqDto dto);

}