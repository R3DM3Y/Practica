using System;
using System.Collections.Generic;

namespace Practica.Models;

public partial class PropertyType
{
    public int Id { get; set; }

    public string TypeName { get; set; } = null!;

    public virtual ICollection<Property> Properties { get; set; } = new List<Property>();
}
