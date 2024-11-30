using System;
using System.Collections.Generic;

namespace AvtoService_3cursAA.Model;

public partial class Carclient
{
    public int IdCarclient { get; set; }

    public int IdCar { get; set; }

    public int IdClient { get; set; }

    public bool IsDeleted { get; set; }

    public virtual Car IdCarNavigation { get; set; } = null!;

    public virtual Client IdClientNavigation { get; set; } = null!;

    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();
}
