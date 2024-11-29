using System;
using System.Collections.Generic;

namespace AvtoService_3cursAA.Model;

public partial class Price
{
    public int IdPrice { get; set; }

    public string Name { get; set; } = null!;

    public int Cost { get; set; }

    public byte[]? Photo { get; set; }

    public bool IsDeleted { get; set; }
    
    public virtual ICollection<Checkprice> Checkprices { get; set; } = new List<Checkprice>();
}
