using Microsoft.EntityFrameworkCore.Query.Internal;
using AvtoService_3cursAA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;

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
        public ObservableCollection<Price> ApplySearch(ObservableCollection<Price> prices)
        {
            string search = _searchTextBox.Text.ToLower();
            if (!string.IsNullOrEmpty(search))
            {
                prices = new ObservableCollection<Price> (prices.Where(r => r.Name.ToLower().StartsWith(search)));
            }
            return prices;
        }

        // Начальная точка цены
        public ObservableCollection<Price> ApplyStartCost(ObservableCollection<Price> prices)
        {
            string startCost = _startCostTextBox.Text.ToLower();
            if (!string.IsNullOrEmpty(startCost))
            {
                if (int.TryParse(startCost, out int parsedStartCost))
                {
                    prices = new ObservableCollection<Price> (prices.Where(r => r.Cost >= parsedStartCost));
                }
            }
            return prices;
        }

        // Конечная точка цены
        public ObservableCollection<Price> ApplyFinishCost(ObservableCollection<Price> prices)
        {
            string finishCost = _finishCostTextBox.Text.ToLower();
            if (!string.IsNullOrEmpty(finishCost))
            {
                if (int.TryParse(finishCost, out int parsedFinishCost))
                {
                    prices = new ObservableCollection<Price>(prices.Where(r => r.Cost <= parsedFinishCost));
                }
            }
            return prices;
        }

        // Сортировка
        public ObservableCollection<Price> ApplySorter(ObservableCollection<Price> prices)
        {
            int sortIndex = _sorterComboBox.SelectedIndex;

            IEnumerable<Price> sortedPrices;

            if (!_ascending)
            {
                switch (sortIndex)
                {
                    case 2:
                        sortedPrices = prices.OrderBy(e => e.Name);
                        break;
                    case 3:
                        sortedPrices = prices.OrderBy(e => e.Cost);
                        break;
                    default:
                        sortedPrices = prices.OrderBy(e => e.IdPrice);
                        break;
                }
            }
            else
            {
                switch (sortIndex)
                {
                    case 2:
                        sortedPrices = prices.OrderByDescending(e => e.Name);
                        break;
                    case 3:
                        sortedPrices = prices.OrderByDescending(e => e.Cost);
                        break;
                    default:
                        sortedPrices = prices.OrderByDescending(e => e.IdPrice);
                        break;
                }
            }

            // Создаем новую ObservableCollection из отсортированных данных
            return new ObservableCollection<Price>(sortedPrices);
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

    public class DetailFilter : DataFilterSorter
    {
        public DetailFilter(TextBox searchTextBox, ComboBox sorterComboBox, CheckBox ascendingCheckBox, TextBox startCostTextBox, TextBox finishCostTextBox)
            : base(searchTextBox, null, sorterComboBox, ascendingCheckBox, startCostTextBox, finishCostTextBox) { }

        // Поиск по названию
        public ObservableCollection<Detail> ApplySearch(ObservableCollection<Detail> details)
        {
            string search = _searchTextBox.Text.ToLower();
            if (!string.IsNullOrEmpty(search))
            {
                details = new ObservableCollection<Detail>(details.Where(r => r.Name.ToLower().Contains(search)));
            }
            return details;
        }

        // Начальная точка цены
        public ObservableCollection<Detail> ApplyStartCost(ObservableCollection<Detail> details)
        {
            string startCost = _startCostTextBox.Text;
            if (!string.IsNullOrEmpty(startCost) && int.TryParse(startCost, out int parsedStartCost))
            {
                details = new ObservableCollection<Detail>(details.Where(r => r.Cost >= parsedStartCost));
            }
            return details;
        }

        // Конечная точка цены
        public ObservableCollection<Detail> ApplyFinishCost(ObservableCollection<Detail> details)
        {
            string finishCost = _finishCostTextBox.Text;
            if (!string.IsNullOrEmpty(finishCost) && int.TryParse(finishCost, out int parsedFinishCost))
            {
                details = new ObservableCollection<Detail>(details.Where(r => r.Cost <= parsedFinishCost));
            }
            return details;
        }

        // Сортировка
        public ObservableCollection<Detail> ApplySorter(ObservableCollection<Detail> details)
        {
            int sortIndex = _sorterComboBox.SelectedIndex;

            IEnumerable<Detail> sortedDetails;

            if (!_ascending)
            {
                switch (sortIndex)
                {
                    case 2:
                        sortedDetails = details.OrderBy(e => e.Name);
                        break;
                    case 3:
                        sortedDetails = details.OrderBy(e => e.Cost);
                        break;
                    case 4:
                        sortedDetails = details.OrderBy(e => e.Count);
                        break;
                    default:
                        sortedDetails = details.OrderBy(e => e.IdDetail);
                        break;
                }
            }
            else
            {
                switch (sortIndex)
                {
                    case 2:
                        sortedDetails = details.OrderByDescending(e => e.Name);
                        break;
                    case 3:
                        sortedDetails = details.OrderByDescending(e => e.Cost);
                        break;
                    case 4:
                        sortedDetails = details.OrderByDescending(e => e.Count);
                        break;
                    default:
                        sortedDetails = details.OrderByDescending(e => e.IdDetail);
                        break;
                }
            }

            // Создаем новую ObservableCollection из отсортированных данных
            return new ObservableCollection<Detail>(sortedDetails);
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
    public class CarFilter : DataFilterSorter
    {
        public CarFilter(TextBox searchTextBox, ComboBox sorterComboBox, CheckBox ascendingCheckBox)
            : base(searchTextBox, null, sorterComboBox, ascendingCheckBox, null, null) { }

        // поиск
        public ObservableCollection<Car> ApplySearch(ObservableCollection<Car> cars)
        {
            string search = _searchTextBox.Text.ToLower();
            if (!string.IsNullOrEmpty(search))
            {
                cars = new ObservableCollection<Car>(cars.Where(r =>
                    r.Brand.ToLower().Contains(search) ||
                    r.Model.ToLower().Contains(search) ||
                    r.Country.ToLower().Contains(search))); 
            }
            return cars;
        }

        // Сортировка
        public ObservableCollection<Car> ApplySorter(ObservableCollection<Car> cars)
        {
            int sortIndex = _sorterComboBox.SelectedIndex;

            IEnumerable<Car> sortedCars;

            if (!_ascending)
            {
                switch (sortIndex)
                {
                    case 0: // По бренду
                        sortedCars = cars.OrderBy(e => e.Brand);
                        break;
                    case 1: // По модели
                        sortedCars = cars.OrderBy(e => e.Model);
                        break;
                    case 2: // По стране производства
                        sortedCars = cars.OrderBy(e => e.Country);
                        break;
                    case 3: // По году производства
                        sortedCars = cars.OrderBy(e => e.Year);
                        break;
                    default:
                        sortedCars = cars.OrderBy(e => e.IdCar);
                        break;
                }
            }
            else
            {
                switch (sortIndex)
                {
                    case 0: // По бренду
                        sortedCars = cars.OrderByDescending(e => e.Brand);
                        break;
                    case 1: // По модели
                        sortedCars = cars.OrderByDescending(e => e.Model);
                        break;
                    case 2: // По стране производства
                        sortedCars = cars.OrderByDescending(e => e.Country);
                        break;
                    case 3: // По году производства
                        sortedCars = cars.OrderByDescending(e => e.Year);
                        break;
                    default:
                        sortedCars = cars.OrderByDescending(e => e.IdCar);
                        break;
                }
            }

            // Создаем новую ObservableCollection из отсортированных данных
            return new ObservableCollection<Car>(sortedCars);
        }

        // Очистка (если нужно)
        public void ApplyClear()
        {
            _sorterComboBox.SelectedIndex = 0; // Сбросить сортировку на первый элемент
            _ascendingCheckBox.IsChecked = false; // Сбросить порядок сортировки
        }
    }
}
