using System;
using System.Collections.Generic;

namespace AvtoService_3cursAA.Model;

public partial class Sale
{
    public int IdSale { get; set; }

    public int IdEmployee { get; set; }

    public int IdStatus { get; set; }

    public DateTime Date { get; set; }

    public int IdTypeofrepair { get; set; }

    public int IdCarclient { get; set; }

    public virtual ICollection<Checkdetail> Checkdetails { get; set; } = new List<Checkdetail>();

    public virtual ICollection<Checkprice> Checkprices { get; set; } = new List<Checkprice>();

    public virtual Carclient IdCarclientNavigation { get; set; } = null!;

    public virtual Employee IdEmployeeNavigation { get; set; } = null!;

    public virtual Status IdStatusNavigation { get; set; } = null!;

    public int CostForClient { get; set; }

    public int CostTotal { get; set; }

    public virtual Typeofrepair IdTypeofrepairNavigation { get; set; } = null!;
}
