using System;
using System.Collections.Generic;

namespace OAuth_Authorization_Server.Models;

/// <summary>
/// The user model.
/// </summary>
public class User
{
    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    /// <value>
    /// The identifier.
    /// </value>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the username.
    /// </summary>
    /// <value>
    /// The username.
    /// </value>
    public string Username { get; set; }


    /// <summary>
    /// Gets or sets the email.
    /// </summary>
    /// <value>
    /// The email.
    /// </value>
    public string Email { get; set; }

    /// <summary>
    /// Gets or sets the date when the user account was created.
    /// </summary>
    /// <value>
    /// The date when the user account was created.
    /// </value>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the user that created this user.
    /// </summary>
    /// <value>
    /// The identifier of the user that created this user.
    /// </value>

    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the user that updated this user.
    /// </summary>
    /// <value>
    /// The identifier of the user that updated this user.
    /// </value>
   
    public Guid? RoleId { get; set; }

    /// <summary>
    /// Gets or sets the role.
    /// </summary>
    /// <value>
    /// The role.
    /// </value>
    public virtual Role Role { get; set; }

    /// <summary>
    /// Gets or sets the latest date when the user was successfully logged in.
    /// </summary>
    /// <value>
    /// The latest date when the user was successfully logged in.
    /// </value>
    public DateTime? LastLoginAt { get; set; }

    /// <summary>
    /// Gets or sets the date when the user login attempt failed.
    /// </summary>
    /// <value>
    /// The date when user login attempt failed.
    /// </value>
    public DateTime? LoginFailedAt { get; set; }

    /// <summary>
    /// Gets or sets the count of consecutive failed login attempts.
    /// </summary>
    /// <value>
    /// The count of consecutive failed login attempts.
    /// </value>
    public int LoginFailedCount { get; set; }
  

    /// <summary>
    /// Gets or sets the password hash.
    /// </summary>
    /// <value>
    /// The password hash.
    /// </value>
    public byte[] PasswordHash { get; set; }

    /// <summary>
    /// Gets or sets the password salt.
    /// </summary>
    /// <value>
    /// The password salt.
    /// </value>
    public byte[] PasswordSalt { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this user is active.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this user is active; otherwise, <c>false</c>.
    /// </value>
    public bool IsActive { get; set; }
  
}