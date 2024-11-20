using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AvtoService_3cursAA.Model;

public partial class Client : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    public int IdClient { get; set; }

    public string Name { get; set; } = null!;

    public string Firstname { get; set; } = null!;

    public string? Patronymic { get; set; }

    public string FullName => $"{Firstname} {Name} {Patronymic}";

    public DateOnly Birthday { get; set; }

    public string Phone { get; set; } = null!;

    public List<string> CarList
    {
        get
        {
            return Carclients?.Select(cc => ($"ID: [{cc.IdCar}] {cc.IdCarNavigation.Brand} {cc.IdCarNavigation.Model}")).ToList() ?? new List<string>();
        }
    }

    public void UpdateCar()
    {
        // Это метод для обновления свойства Car после загрузки данных
        OnPropertyChanged(nameof(Car));
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    public virtual ICollection<Carclient> Carclients { get; set; } = new List<Carclient>();
}
