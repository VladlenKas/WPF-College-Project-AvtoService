using System;
using System.Collections.Generic;

namespace AvtoService_3cursAA.Model;

public partial class Status
{
    public int IdStatus { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();
}
