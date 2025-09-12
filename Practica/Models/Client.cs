using System;
using System.Collections.Generic;

namespace Practica.Models;

public partial class Client
{
    public int Id { get; set; }

    public string LastName { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string? MiddleName { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public int? RoleId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string Password { get; set; } = null!;

    public virtual Role? Role { get; set; }

    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();
}
