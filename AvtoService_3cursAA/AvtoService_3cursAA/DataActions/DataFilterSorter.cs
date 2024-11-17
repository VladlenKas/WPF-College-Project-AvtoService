using Microsoft.EntityFrameworkCore.Query.Internal;
using AvtoService_3cursAA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AvtoService_3cursAA.Classes
{
    /// <summary>
    /// Принимает входящие листы с данными о ItemSorce фильтра и сортировки.
    /// Также принимает выбранные элементы в обоих Combobox. И список, который нужно отсортировать
    /// Проверяет, сортировка Asc или Desc. После чего возвращает полностью
    /// отсортированный и отфильтрованный лист
    /// </summary>
    public abstract class DataFilterSorter
    {
        protected TextBox _searchTextBox;
        protected ComboBox _filterComboBox;
        protected ComboBox _sorterComboBox;
        protected CheckBox _ascendingCheckBox;
        protected bool _ascending;

        protected TextBox _startCostTextBox;
        protected TextBox _finishCostTextBox;

        protected DataFilterSorter(TextBox searchTextBox, ComboBox filterComboBox, ComboBox sorterComboBox, 
            CheckBox ascendingCheckBox, TextBox startCostTextBox, TextBox finishCostTextBox)
        {
            _searchTextBox = searchTextBox;
            _filterComboBox = filterComboBox;
            _sorterComboBox = sorterComboBox;
            _ascendingCheckBox = ascendingCheckBox;
            if (ascendingCheckBox != null)
                _ascending = (bool)_ascendingCheckBox.IsChecked;
            else
                _ascending = false;

            _startCostTextBox = startCostTextBox;
            _finishCostTextBox = finishCostTextBox;
        }
    }

    /// <summary>
    /// Фильтрация, поиск и сортировка для сотрдуников
    /// </summary>
    public class DataFilterSorterEmployees : DataFilterSorter
    {
        public DataFilterSorterEmployees(TextBox searchTextBox, ComboBox filterComboBox, ComboBox sorterComboBox, CheckBox ascendingCheckBox)
        : base(searchTextBox, filterComboBox, sorterComboBox, ascendingCheckBox, null, null) { }
        // Очистка 
        public void ApplyClear()
        {
            _searchTextBox.Text = string.Empty;
            _ascendingCheckBox.IsChecked = false;
            _sorterComboBox.SelectedIndex = 0;
            _filterComboBox.SelectedIndex = 0;
        }

        // Поиск 
        public List<Employee> ApplySearch(List<Employee> employees)
        {
            string search = _searchTextBox.Text.ToLower();
            if (!string.IsNullOrEmpty(search))
            {
                employees = employees.Where(r =>
                {
                    return r.Firstname.ToLower().StartsWith(search)
                    || r.Name.ToLower().StartsWith(search)
                    || (r.Patronymic != null && r.Patronymic.ToLower().StartsWith(search));
                }).ToList();
            }
            return employees;
        }

        // Фильтрафия
        public List<Employee> ApplyFilter(List<Employee> employees)
        {
            if (_filterComboBox.SelectedIndex != 0)
            {
                string role = _filterComboBox.SelectedValue.ToString();
                employees = employees.Where(u => u.IdRoleNavigation.Name == role).ToList();
            }
            return employees;
        }

        // Сортировка
        public List<Employee> ApplySorter(List<Employee> employees)
        {
            int sortIndex = _sorterComboBox.SelectedIndex;

            if (!_ascending)
            {
                switch (sortIndex)
                {
                    case 2:
                        return employees.OrderBy(e => e.Name).ToList();
                    case 3:
                        return employees.OrderBy(e => e.Firstname).ToList();
                    case 4:
                        return employees.OrderBy(e => e.Patronymic).ToList();
                    case 5:
                        return employees.OrderBy(e => e.Birthday).ToList();
                    case 6:
                        return employees.OrderBy(e => e.Seniority).ToList();
                    case 7:
                        return employees.OrderBy(e => e.Passport).ToList();
                    case 8:
                        return employees.OrderBy(e => e.Phone).ToList();
                    case 9:
                        return employees.OrderBy(e => e.Login).ToList();
                    case 10:
                        return employees.OrderBy(e => e.Password).ToList();
                    default:
                        return employees.OrderBy(e => e.IdEmployee).ToList();
                }
            }
            else
            {
                switch (sortIndex)
                {
                    case 2:
                        return employees.OrderByDescending(e => e.Name).ToList();
                    case 3:
                        return employees.OrderByDescending(e => e.Firstname).ToList();
                    case 4:
                        return employees.OrderByDescending(e => e.Patronymic).ToList();
                    case 5:
                        return employees.OrderByDescending(e => e.Birthday).ToList();
                    case 6:
                        return employees.OrderByDescending(e => e.Seniority).ToList();
                    case 7:
                        return employees.OrderByDescending(e => e.Passport).ToList();
                    case 8:
                        return employees.OrderByDescending(e => e.Phone).ToList();
                    case 9:
                        return employees.OrderByDescending(e => e.Login).ToList();
                    case 10:
                        return employees.OrderByDescending(e => e.Password).ToList();
                    default:
                        return employees.OrderByDescending(e => e.IdEmployee).ToList();
                }
            }
        }
    }

    /// <summary>
    /// Фильтрация, поиск и сортировка для клиентов
    /// </summary>
    public class DataFilterSorterClients : DataFilterSorter
    {
        public DataFilterSorterClients(TextBox searchTextBox, ComboBox sorterComboBox, CheckBox ascendingCheckBox)
        : base(searchTextBox, null, sorterComboBox, ascendingCheckBox, null, null) { }

        // Очистка 
        public void ApplyClear()
        {
            _searchTextBox.Text = string.Empty;
            _ascendingCheckBox.IsChecked = false;
            _sorterComboBox.SelectedIndex = 0;
        }

        // Поиск 
        public List<Client> ApplySearch(List<Client> clients)
        {
            string search = _searchTextBox.Text.ToLower();
            if (!string.IsNullOrEmpty(search))
            {
                clients = clients.Where(r =>
                {
                    return r.Firstname.ToLower().StartsWith(search)
                    || r.Name.ToLower().StartsWith(search)
                    || (r.Patronymic != null && r.Patronymic.ToLower().StartsWith(search));
                }).ToList();
            }
            return clients;
        }

        // Сортировка
        public List<Client> ApplySorter(List<Client> clients)
        {
            int sortIndex = _sorterComboBox.SelectedIndex;

            if (!_ascending)
            {
                switch (sortIndex)
                {
                    case 2:
                        return clients.OrderBy(e => e.Name).ToList();
                    case 3:
                        return clients.OrderBy(e => e.Firstname).ToList();
                    case 4:
                        return clients.OrderBy(e => e.Patronymic).ToList();
                    case 5:
                        return clients.OrderBy(e => e.Birthday).ToList();
                    case 6:
                        return clients.OrderBy(e => e.Phone).ToList();
                    default:
                        return clients.OrderBy(e => e.IdClient).ToList();
                }
            }
            else
            {
                switch (sortIndex)
                {
                    case 2:
                        return clients.OrderByDescending(e => e.Name).ToList();
                    case 3:
                        return clients.OrderByDescending(e => e.Firstname).ToList();
                    case 4:
                        return clients.OrderByDescending(e => e.Patronymic).ToList();
                    case 5:
                        return clients.OrderByDescending(e => e.Birthday).ToList();
                    case 6:
                        return clients.OrderByDescending(e => e.Phone).ToList();
                    default:
                        return clients.OrderByDescending(e => e.IdClient).ToList();
                }
            }
        }
    }

    /// <summary>
    /// Поиск по цене
    /// </summary>
    public class PriceFilter : DataFilterSorter
    {
        public PriceFilter(TextBox searchTextBox, ComboBox sorterComboBox, CheckBox ascendingCheckBox, TextBox startCostTextBox, TextBox finishCostTextBox)
            : base(searchTextBox, null, sorterComboBox, ascendingCheckBox, startCostTextBox, finishCostTextBox) { }

        // Поиск по названию
        public List<Price> ApplySearch(List<Price> prices)
        {
            string search = _searchTextBox.Text.ToLower();
            if (!string.IsNullOrEmpty(search))
            {
                prices = prices.Where(r => r.Name.ToLower().StartsWith(search)).ToList();
            }
            return prices;
        }

        // Начальная точка цены
        public List<Price> ApplyStartCost(List<Price> prices)
        {
            string startCost = _startCostTextBox.Text.ToLower();
            if (!string.IsNullOrEmpty(startCost))
            {
                if (int.TryParse(startCost, out int parsedStartCost))
                {
                    prices = prices.Where(r => r.Cost >= parsedStartCost).ToList();
                }
            }
            return prices;
        }

        // Конечная точка цены
        public List<Price> ApplyFinishCost(List<Price> prices)
        {
            string finishCost = _finishCostTextBox.Text.ToLower();
            if (!string.IsNullOrEmpty(finishCost))
            {
                if (int.TryParse(finishCost, out int parsedFinishCost))
                {
                    prices = prices.Where(r => r.Cost <= parsedFinishCost).ToList();
                }
            }
            return prices;
        }

        // Сортировка
        public List<Price> ApplySorter(List<Price> prices)
        {
            int sortIndex = _sorterComboBox.SelectedIndex;

            if (!_ascending)
            {
                switch (sortIndex)
                {
                    case 2:
                        return prices.OrderBy(e => e.Name).ToList();
                    case 3:
                        return prices.OrderBy(e => e.Cost).ToList();
                    default:
                        return prices.OrderBy(e => e.IdPrice).ToList();
                }
            }
            else
            {
                switch (sortIndex)
                {
                    case 2:
                        return prices.OrderByDescending(e => e.Name).ToList();
                    case 3:
                        return prices.OrderByDescending(e => e.Cost).ToList();
                    default:
                        return prices.OrderByDescending(e => e.IdPrice).ToList();
                }
            }
        }

        // Очистка
        public void ApplyClear()
        {
            _searchTextBox.Text = string.Empty;
            _startCostTextBox.Text = string.Empty;
            _finishCostTextBox.Text = string.Empty;
            _sorterComboBox.SelectedIndex = 0;
            _ascendingCheckBox.IsChecked = false;
        }
    }
}
