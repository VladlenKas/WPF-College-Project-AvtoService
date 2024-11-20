using AvtoService_3cursAA.ActionsForEmployee;
using AvtoService_3cursAA.Classes;
using AvtoService_3cursAA.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AvtoService_3cursAA.PagesMenuMechanic
{
    /// <summary>
    /// Логика взаимодействия для ClientsAdmin.xaml
    /// </summary>
    public partial class ClientsMechanic : Page
    {
        private Avtoservice3cursAaContext dbContext;
        private Employee _thisUser;

        private DataFilterSorterClients _dataFilterSorter;
        public ClientsMechanic(Employee employee)
        {
            this._thisUser = employee;

            InitializeComponent();
            DataLoad();
            UpdateClientsList();
        }
        private void UpdateClientsList()
        {
            dbContext = new();

            this._dataFilterSorter = new DataFilterSorterClients(SearchTextBox, ComboBoxSort, SortCheckBox);
            var clientList = dbContext.Clients
                            .Include(c => c.Carclients)
                            .ThenInclude(cc => cc.IdCarNavigation)
                            .ToList();

            foreach (var client in clientList)
            {
                client.UpdateCar(); // Обновляем поле Car
            }

            clientList = _dataFilterSorter.ApplySorter(clientList);
            clientList = _dataFilterSorter.ApplySearch(clientList);

            ClientsDataGrid.ItemsSource = null;
            ClientsDataGrid.ItemsSource = clientList;
        }

        private void DataLoad()
        {
            // dbContext load
            dbContext = new();
            dbContext.Clients
            .Include(c => c.Carclients)
            .ThenInclude(cc => cc.IdCarNavigation)
            .Load();

            // ComboBoxes load
            var sorterList = FillDataFilterSorter.FillSorterClient();
            ComboBoxSort.ItemsSource = sorterList;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            UserFio.Text = $"{_thisUser.FullName}";
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ClientsDataGrid.ItemsSource != null)
                UpdateClientsList();
        }

        private void ClearDataButton_Click(object sender, RoutedEventArgs e)
        {
            if (ClientsDataGrid.ItemsSource != null)
            {
                _dataFilterSorter.ApplyClear();
                UpdateClientsList();
            }
        }

        private void SortCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (ClientsDataGrid.ItemsSource != null)
                UpdateClientsList();
        }

        private void ComboBoxSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ClientsDataGrid.ItemsSource != null)
                UpdateClientsList();
        }
    }
}
