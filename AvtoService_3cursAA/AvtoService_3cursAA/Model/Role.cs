using System;
using System.Collections.Generic;

namespace AvtoService_3cursAA.Model;

public partial class Role
{
    public int IdRole { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
