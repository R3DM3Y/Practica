using System;
using System.Collections.Generic;

namespace Practica.Models;

public partial class TypeOfRepair
{
    public int Id { get; set; }

    public string RepairName { get; set; } = null!;

    public virtual ICollection<Property> Properties { get; set; } = new List<Property>();
}
