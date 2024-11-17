using System;
using System.Collections.Generic;

namespace AvtoService_3cursAA.Model;

public partial class Typeofrepair
{
    public int IdTypeofrepair { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();
}
