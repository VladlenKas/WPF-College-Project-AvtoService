using System;
using System.Collections.Generic;

namespace AvtoService_3cursAA.Model;

public partial class Detail
{
    public int IdDetail { get; set; }

    public string Name { get; set; } = null!;

    public int Cost { get; set; }

    public int Count { get; set; }

    public byte[]? Photo { get; set; }

    public bool IsDeleted { get; set; }
    
    public virtual ICollection<Checkdetail> Checkdetails { get; set; } = new List<Checkdetail>();
}
