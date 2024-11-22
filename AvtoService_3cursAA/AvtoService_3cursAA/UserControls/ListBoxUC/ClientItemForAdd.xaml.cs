using AvtoService_3cursAA.Actions.Cars;
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

namespace AvtoService_3cursAA.UserControls.ListBoxUC
{
    /// <summary>
    /// Логика взаимодействия для ClientItemForAdd.xaml
    /// </summary>
    public partial class ClientItemForAdd : UserControl
    {
        public Client Client
        {
            get => _client;
        }

        private Client _client;
        private AddCar _parentWindow;

        public ClientItemForAdd(Client client, AddCar parentWindow)
        {
            _client = client;
            _parentWindow = parentWindow;

            InitializeComponent();
            DataContext = client;
        }

        private void Delete_Click(object sender, RoutedEventArgs e) => _parentWindow.DeletePriceInPriceView(_client);
    }
}
