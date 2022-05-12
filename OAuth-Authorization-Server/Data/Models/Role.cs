using System;
using System.Collections.Generic;

namespace OAuth_Authorization_Server.Models;

/// <summary>
/// The user role.
/// </summary>
public class Role
{
    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    /// <value>
    /// The identifier.
    /// </value>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    /// <value>
    /// The name.
    /// </value>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the description.
    /// </summary>
    /// <value>
    /// The description.
    /// </value>
    public string Description { get; set; }

    /// <summary>
    /// Gets or sets the users assigned to this role.
    /// </summary>
    /// <value>
    /// The users assigned to this role.
    /// </value>
    public virtual ICollection<User> Users { get; set; }
}