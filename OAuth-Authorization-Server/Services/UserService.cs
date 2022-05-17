using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OAuth_Authorization_Server.Data;
using OAuth_Authorization_Server.Helpers;
using OAuth_Authorization_Server.Helpers.Exceptions;
using OAuth_Authorization_Server.Resources.Localization;
using OAuth_Authorization_Server.Services.DataTransferObjects.UserService;
using WebApi.Services.Interfaces;

namespace OAuth_Authorization_Server.Services;

/// <summary>
/// The user service.
/// </summary>
/// <seealso cref="IUserService" />
public class UserService : IUserService
{
    private readonly ILogger<UserService> _logger;
    private readonly AppDbContext _db;
    private readonly AppSettings _appSettings;
    private readonly IStringLocalizer<Translation> _l;
    private readonly IStringLocalizer<LocalizedResource> _lr;
    private readonly IEmailService _emailService;
    private readonly IPasswordHelper _passwordHelper;
    private readonly EmbeddedFileProvider _embedded;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserService"/> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="db">The database context.</param>
    /// <param name="appSettings">The application settings.</param>
    /// <param name="l">The string localizer.</param>
    /// <param name="lr">The resource localizer.</param>
    /// <param name="emailService">The email service.</param>
    /// <param name="passwordHelper">The password helper.</param>
    public UserService(ILogger<UserService> logger, AppDbContext db, IOptions<AppSettings> appSettings,
        IStringLocalizer<Translation> l, IStringLocalizer<LocalizedResource> lr, IEmailService emailService,
        IPasswordHelper passwordHelper)
    {
        _logger = logger;
        _db = db;
        _appSettings = appSettings.Value;
        _l = l;
        _lr = lr;
        _emailService = emailService;
        _passwordHelper = passwordHelper;
        _embedded = new EmbeddedFileProvider(Assembly.GetExecutingAssembly());
    }

    /// <inheritdoc />
    public async Task<AuthenticateResDto> AuthenticateAsync(AuthenticateReqDto dto)
    {
        var user = await _db.Users.SingleOrDefaultAsync(x => x.Username == dto.Username && x.IsActive);

        // Check if the username exists.
        if (user == null) throw new EntityNotFoundException(_l["Username is incorrect."]);

        // Check for too many failed login attempts.
        if (user.LoginFailedAt != null)
        {
            var secondsPassed = DateTime.UtcNow.Subtract(user.LoginFailedAt.GetValueOrDefault()).Seconds;

            var isMaxCountExceeded = user.LoginFailedCount >= _appSettings.MaxLoginFailedCount;
            var isWaitingTimePassed = secondsPassed > _appSettings.LoginFailedWaitingTime;

            if (isMaxCountExceeded && !isWaitingTimePassed)
            {
                var secondsToWait = _appSettings.LoginFailedWaitingTime - secondsPassed;
                throw new TooManyFailedLoginAttemptsException(string.Format(
                    _l["You must wait for {0} seconds before you try to log in again."], secondsToWait));
            }
        }

        // Check if password is correct.
        if (!_passwordHelper.VerifyHash(dto.Password, user.PasswordHash, user.PasswordSalt))
        {
            user.LoginFailedCount += 1;
            user.LoginFailedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            throw new IncorrectPasswordException(_l["Password is incorrect."]);
        }

        // Authentication successful.
        user.LoginFailedCount = 0;
        user.LoginFailedAt = null;
        user.LastLoginAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();

        return new AuthenticateResDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            Token = CreateToken(user.Id.ToString())
        };
    }

    #region Private helper method
    private string CreateToken(string userId)
    {
        //var tokenHandler = new JwtSecurityTokenHandler();
        //var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
        //var tokenDescriptor = new SecurityTokenDescriptor
        //{
        //    Subject = new ClaimsIdentity(new[] {new Claim(ClaimTypes.Name, userId)}),
        //    Expires = DateTime.UtcNow.AddDays(7),
        //    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
        //        SecurityAlgorithms.HmacSha256Signature)
        //};
        //var token = tokenHandler.CreateToken(tokenDescriptor);
        //return tokenHandler.WriteToken(token);

        return default;
    }

    #endregion
}