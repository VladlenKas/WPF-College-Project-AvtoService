using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AvtoService_3cursAA.Model;

public class Client
{
    public int IdClient { get; set; }

    public string Name { get; set; } = null!;

    public string Firstname { get; set; } = null!;

    public string? Patronymic { get; set; }

    public string FullName => $"{Firstname} {Name} {Patronymic}";

    public DateOnly Birthday { get; set; }

    public string Phone { get; set; } = null!;

    public bool IsDeleted { get; set; }

    public List<string> CarList
    {
        get
        {
            return Carclients
                .Where(cc => cc.IsDeleted != true)?
                .Where(cc => cc.IdCarNavigation.IsDeleted != true)
                .Select(cc => cc.IdCarNavigation.Title)
                .ToList() ?? new List<string>();
        }
    }
    public virtual ICollection<Carclient> Carclients { get; set; } = new List<Carclient>();

    // Новое свойство для контроля переноса текста
    public bool IsTextWrapped
    {
        get
        {
            // Логика определения необходимости переноса текста
            return FullName.Length > 40; // Пример: перенос, если длина больше 40 символов
        }
    }
}
