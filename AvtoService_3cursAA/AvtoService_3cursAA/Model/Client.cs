using System;
using System.Collections.Generic;

namespace AvtoService_3cursAA.Model;

public partial class Client
{
    public int IdClient { get; set; }

    public string Name { get; set; } = null!;

    public string Firstname { get; set; } = null!;

    public string? Patronymic { get; set; }

    public string FullName => $"{Firstname} {Name} {Patronymic}";

    public DateOnly Birthday { get; set; }

    public string Phone { get; set; } = null!;

    public string? Car
    {
        get
        {
            string carsListStrings = string.Empty;
            foreach (var car in CarList)
            {
                carsListStrings += (car + " // ");
            }

            return carsListStrings.Remove(0, carsListStrings.Length - 3);
        }
    }

    public List<string> CarList
    {
        get
        {
            return Carclients.Select(cc => ($"ID: [{cc.IdCar}] {cc.IdCarNavigation.Brand} {cc.IdCarNavigation.Model}")).ToList();
        }
    }

    public virtual ICollection<Carclient> Carclients { get; set; } = new List<Carclient>();
}
