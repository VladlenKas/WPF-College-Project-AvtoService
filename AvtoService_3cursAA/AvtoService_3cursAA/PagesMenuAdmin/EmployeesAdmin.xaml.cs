using Microsoft.EntityFrameworkCore;
using AvtoService_3cursAA.ActionsEmployee;
using AvtoService_3cursAA.ActionsForEmployee;
using AvtoService_3cursAA.Classes;
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
using static MaterialDesignThemes.Wpf.Theme;
using AvtoService_3cursAA.Model;

namespace AvtoService_3cursAA.PagesMenuAdmin
{
    /// <summary>
    /// Логика взаимодействия для EmployeesPage.xaml
    /// </summary>
    public partial class EmployeesAdmin : Page
    {
        private Avtoservice3cursAaContext dbContext;
        private Employee _selectUser;
        private readonly Employee _thisUser;

        private DataFilterSorterEmployees dataFilterSorter;
        public EmployeesAdmin(Employee employee)
        {
            this._thisUser = employee;

            InitializeComponent();
            DataLoad();
            UpdateEmployeesList();
        }

        private void UpdateEmployeesList()
        {
            dbContext = new Avtoservice3cursAaContext();

            this.dataFilterSorter = new DataFilterSorterEmployees(SearchTextBox, ComboBoxFilter, ComboBoxSort, SortCheckBox);
            var employeesList = dbContext.Employees.Include(e => e.IdRoleNavigation).ToList();

            employeesList = dataFilterSorter.ApplyFilter(employeesList);
            employeesList = dataFilterSorter.ApplySorter(employeesList);
            employeesList = dataFilterSorter.ApplySearch(employeesList);

            EmployeesDataGrid.ItemsSource = null;
            EmployeesDataGrid.ItemsSource = employeesList;
        }

        private void DataLoad()
        {
            // dbContext load
            dbContext = new Avtoservice3cursAaContext();
            dbContext.Employees.Include(e => e.IdRoleNavigation).Load();

            // ComboBoxes load
            var filterList = FillDataFilterSorter.FillFilterEmployee();
            var sorterList = FillDataFilterSorter.FillSorterEmployee();
            ComboBoxFilter.ItemsSource = filterList;
            ComboBoxSort.ItemsSource = sorterList;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            UserFio.Text = $"{_thisUser.FullName}";
        }

        private void ComboBoxFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EmployeesDataGrid.ItemsSource != null)
                UpdateEmployeesList();
        }

        private void ComboBoxSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EmployeesDataGrid.ItemsSource != null)
                UpdateEmployeesList();
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (EmployeesDataGrid.ItemsSource != null)
                UpdateEmployeesList();
        }

        private void SortCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (EmployeesDataGrid.ItemsSource != null)
                UpdateEmployeesList();
        }

        private void ClearDataButton_Click(object sender, RoutedEventArgs e)
        {
            if (EmployeesDataGrid.ItemsSource != null)
            {
                dataFilterSorter.ApplyClear();
                UpdateEmployeesList();
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddEmployee addEmployee = new AddEmployee(_thisUser);
            addEmployee.ShowDialog();

            UpdateEmployeesList();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectUser == null)
                MessageBox.Show("Выберите пользователя для изменения", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                EditEmployee editEmployee = new EditEmployee(_selectUser, _thisUser);
                editEmployee.ShowDialog();

                UpdateEmployeesList();
                _selectUser = null;
            }
        }

        private void EmployeesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectUser = (Employee)EmployeesDataGrid.SelectedItem;
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectUser.IdEmployee == _thisUser.IdEmployee)
            {
                MessageBox.Show("Администратор не может удалять сам себя", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                ActionsData.DeleteEmployee(_selectUser as Employee);
                _selectUser = null;
                UpdateEmployeesList();
            }
        }
    }
}
