using Microsoft.EntityFrameworkCore;
using AvtoService_3cursAA.ActionsEmployee;
using AvtoService_3cursAA.Classes;
using AvtoService_3cursAA.Model;
using AvtoService_3cursAA.PagesMenuAdmin;
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
using System.Windows.Shapes;

namespace AvtoService_3cursAA
{
    /// <summary>
    /// Логика взаимодействия для MenuAdmin.xaml
    /// </summary>
    public partial class MenuAdmin : Window
    {
        private Employee _employee;
        public MenuAdmin(Employee employee)
        {
            this._employee = employee;
            InitializeComponent();

            ChoosePage(0);
        }

        private void ChoosePage(int numberPage)
        {
            switch (numberPage)
            {
                case 0:
                    ContentFrame.Navigate(new EmployeesAdmin(_employee));
                    break;
                case 1:
                    ContentFrame.Navigate(new ClientsAdmin(_employee));
                    break;
                case 2:
                    ContentFrame.Navigate(new PriceAdmin(_employee));
                    break;
                case 3:
                    ContentFrame.Navigate(new CheckAdmin(_employee));
                    break;
                case 4:
                    ContentFrame.Navigate(new DetailAdmin(_employee));
                    break;
                case 5:
                    ContentFrame.Navigate(new CarAdmin(_employee));
                    break;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Title = $"Меню сотрудника {_employee.FullName}. Вы вошли как: {_employee.IdRoleNavigation.Name}";
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Authorization auth = new Authorization();
            auth.Show();
        }

        private void Exit_Click(object sender, RoutedEventArgs e) => this.Close();

        private void ButtonEmployees_Click(object sender, RoutedEventArgs e)
        {
            ChoosePage(0);
        }

        private void ButtonClients_Click(object sender, RoutedEventArgs e)
        {
            ChoosePage(1);
        }

        private void ButtonPrice_Click(object sender, RoutedEventArgs e)
        {
            ChoosePage(2);
        }

        private void ButtonCheck_Click(object sender, RoutedEventArgs e)
        {
            ChoosePage(3);
        }

        private void ButtonDetail_Click(object sender, RoutedEventArgs e)
        {
            ChoosePage(4);
        }

        private void ButtonCar_Click(object sender, RoutedEventArgs e)
        {
            ChoosePage(5);
        }
    }
}
