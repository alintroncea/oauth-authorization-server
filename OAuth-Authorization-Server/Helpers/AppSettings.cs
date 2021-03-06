namespace OAuth_Authorization_Server.Helpers;

/// <summary>
/// Class used to map strongly typed settings objects.
/// </summary>
public class AppSettings
{
    /// <summary>
    /// Gets or sets the secret used for encryption. Can be any text that is at least 16 characters long.
    /// </summary>
    /// <value>
    /// The secret used for encryption.
    /// </value>
    public string Secret { get; set; }

    public string WebsiteURL { get; set; }

    public string AuthorizationEndpoint { get; set; }

    public string TokenEndpoint { get; set; }

    /// <summary>
    /// Gets or sets the name of the WebAPI.
    /// </summary>
    /// <value>
    /// The name of the WebAPI.
    /// </value>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the email address that will be used to send emails.
    /// </summary>
    /// <value>
    /// The email address that will be used to send emails.
    /// </value>
    public string Email { get; set; }

    /// <summary>
    /// Gets or sets the name of the email sender.
    /// </summary>
    /// <value>
    /// The name of the email sender.
    /// </value>
    public string EmailName { get; set; }

    /// <summary>
    /// Gets or sets the maximum count of the failed login attempts.
    /// </summary>
    /// <value>
    /// The maximum count of the failed login attempts.
    /// </value>
    public int MaxLoginFailedCount { get; set; }

    /// <summary>
    /// Gets or sets the time (in seconds) that the user has to wait if they exceeded the maximum count of
    /// the failed login attempts.
    /// </summary>
    /// <value>
    /// The login failed waiting time (in seconds) that the user has to wait if they exceeded the maximum count of
    /// the failed login attempts.
    /// </value>
    public int LoginFailedWaitingTime { get; set; }


    public string AdminUsername { get; set; }

    /// <summary>
    /// Gets or sets the administrator email.
    /// </summary>
    /// <value>
    /// The administrator email.
    /// </value>
    public string AdminEmail { get; set; }

    /// <summary>
    /// Gets or sets the administrator password.
    /// </summary>
    /// <value>
    /// The administrator password.
    /// </value>
    public string AdminPassword { get; set; }

    public string ClientId { get; set; }

    public string CallbackPath { get; set; }
}