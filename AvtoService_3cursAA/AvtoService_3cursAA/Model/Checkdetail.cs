using System;
using System.Collections.Generic;

namespace AvtoService_3cursAA.Model;

public partial class Checkdetail
{
    public int IdCheckdetail { get; set; }

    public int? IdSale { get; set; }

    public int IdDetail { get; set; }

    public virtual Detail IdDetailNavigation { get; set; } = null!;

    public virtual Sale? IdSaleNavigation { get; set; }
}
