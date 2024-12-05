using Microsoft.EntityFrameworkCore;
using AvtoService_3cursAA.Actions;
using AvtoService_3cursAA.ActionsEmployee;
using AvtoService_3cursAA.ActionsForEmployee;
using AvtoService_3cursAA.Classes;
using AvtoService_3cursAA.Model;
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
using System.Windows.Media.TextFormatting;

namespace AvtoService_3cursAA.PagesMenuOperator
{
    /// <summary>
    /// Логика взаимодействия для ClientsPage.xaml
    /// </summary>
    public partial class ClientsOperator : Page
    {
        private Avtoservice3cursAaContext dbContext;
        private Client _selectUser;
        private Employee _thisUser;

        private DataFilterSorterClients _dataFilterSorter;
        public ClientsOperator(Employee employee)
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

            clientList = _dataFilterSorter.ApplySorter(clientList);
            clientList = _dataFilterSorter.ApplySearch(clientList);

            ClientsDataGrid.ItemsSource = null;
            ClientsDataGrid.ItemsSource = clientList;

            if (clientList.Count == 0)
            {
                textFound.Visibility = Visibility.Visible;
            }
            else
            {
                textFound.Visibility = Visibility.Hidden;
            }
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

        private void ComboBoxSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ClientsDataGrid.ItemsSource != null)
                UpdateClientsList();
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

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            ActionsData.DeleteClient(_selectUser as Client);
            _selectUser = null;
            UpdateClientsList();
        }
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectUser == null)
                MessageBox.Show("Выберите пользователя для изменения", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                EditClient editClient = new EditClient(_selectUser);
                editClient.ShowDialog();

                UpdateClientsList();
                _selectUser = null;
            }
        }

        private void ClientsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectUser = (Client)ClientsDataGrid.SelectedItem;
        }

        private void AddClient_Click(object sender, RoutedEventArgs e)
        {
            AddClient addClient = new AddClient();
            addClient.ShowDialog();

            UpdateClientsList();
            _selectUser = null;
        }
    }
}
