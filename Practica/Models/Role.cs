using System;
using System.Collections.Generic;

namespace Practica.Models;

public partial class Role
{
    public int Id { get; set; }

    public string RoleName { get; set; } = null!;

    public virtual ICollection<Client> Clients { get; set; } = new List<Client>();
}
