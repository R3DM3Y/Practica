using System;
using System.Collections.Generic;

namespace Practica.Models;

public partial class Property
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public decimal Area { get; set; }

    public string Address { get; set; } = null!;

    public int? PropertyTypeId { get; set; }

    public int? StatusId { get; set; }

    public int? RepairTypeId { get; set; }

    public int? Rooms { get; set; }

    public int? Floor { get; set; }

    public int? TotalFloors { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? PhotoPath { get; set; }

    public virtual PropertyType? PropertyType { get; set; }

    public virtual TypeOfRepair? RepairType { get; set; }

    public virtual Sale? Sale { get; set; }

    public virtual PropertyStatus? Status { get; set; }
}
