using System;
using System.Collections.Generic;

namespace AvtoService_3cursAA.Model;

public partial class Car
{
    public int IdCar { get; set; }

    public string Brand { get; set; } = null!;

    public string Model { get; set; } = null!;

    public string Title => $"{Brand} {Model}";

    public string Country { get; set; } = null!;

    public short Year { get; set; }

    public string? Description { get; set; }

    public byte[]? Photo { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<Carclient> Carclients { get; set; } = new List<Carclient>();
}
