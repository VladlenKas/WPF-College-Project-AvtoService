using System;
using System.Collections.Generic;

namespace AvtoService_3cursAA.Model;

public partial class Employee
{
    public int IdEmployee { get; set; }

    public int IdRole { get; set; }

    public string Name { get; set; } = null!;

    public string Firstname { get; set; } = null!;

    public string? Patronymic { get; set; }

    public string? FullName => $"{Firstname} {Name} {Patronymic}";

    public DateOnly Birthday { get; set; }

    public int Seniority { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Passport { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public virtual Role IdRoleNavigation { get; set; } = null!;

    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();
}
