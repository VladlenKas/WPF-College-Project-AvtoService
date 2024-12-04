using AvtoService_3cursAA.Model;
using AvtoService_3cursAA.PagesMenuAdmin;
using AvtoService_3cursAA.PagesMenuMechanic;
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

namespace AvtoService_3cursAA.InterfaceWindows
{
    /// <summary>
    /// Логика взаимодействия для MenuMechanic.xaml
    /// </summary>
    public partial class MenuMechanic : Window
    {
        private Employee _employee;
        public MenuMechanic(Employee employee)
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
                    ContentFrame.Navigate(new ClientsMechanic(_employee));
                    break;
                case 1:
                    ContentFrame.Navigate(new CarMechanic(_employee));
                    break;
                case 2:
                    ContentFrame.Navigate(new DetailMechanic(_employee));
                    break;
            }
        }
       
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Title = $"Меню менеджера. Вы вошли как: {_employee.FullName}";
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Authorization auth = new Authorization();
            auth.Show();
        }

        private void Exit_Click(object sender, RoutedEventArgs e) => this.Close();

        private void ButtonClients_Click(object sender, RoutedEventArgs e)
        {
            ChoosePage(0);
        }

        private void CarMechanic_Click(object sender, RoutedEventArgs e)
        {
            ChoosePage(1);
        }

        private void DetailMechnic_Click(object sender, RoutedEventArgs e)
        {
            ChoosePage(2);
        }
    }
}
