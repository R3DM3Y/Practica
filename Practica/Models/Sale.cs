using System;
using System.Collections.Generic;

namespace Practica.Models;

public partial class Sale
{
    public int Id { get; set; }

    public int? PropertyId { get; set; }

    public int? ClientId { get; set; }

    public decimal SalePrice { get; set; }

    public DateOnly SaleDate { get; set; }

    public decimal? Commission { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Client? Client { get; set; }

    public virtual Property? Property { get; set; }
}
