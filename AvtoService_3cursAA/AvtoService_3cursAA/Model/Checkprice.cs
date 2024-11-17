using System;
using System.Collections.Generic;

namespace AvtoService_3cursAA.Model;

public partial class Checkprice
{
    public int IdCheckprice { get; set; }

    public int? IdSale { get; set; }

    public int IdPrice { get; set; }

    public virtual Price IdPriceNavigation { get; set; } = null!;

    public virtual Sale? IdSaleNavigation { get; set; }
}
