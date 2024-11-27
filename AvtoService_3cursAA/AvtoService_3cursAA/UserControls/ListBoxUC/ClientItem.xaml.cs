using AvtoService_3cursAA.Actions.Cars;
using AvtoService_3cursAA.Model;
using AvtoService_3cursAA.PagesMenuAdmin;
using AvtoService_3cursAA.PagesMenuOperator;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace AvtoService_3cursAA.UserControls.CheckUC
{
    /// <summary>
    /// Логика взаимодействия для ClientItem.xaml
    /// </summary>
    public partial class ClientItemForEditCar : UserControl
    {
        public Client Client
        {
            get => _client;
        }

        private Client _client;
        private EditCar _parentWindow;

        public ClientItemForEditCar(Client client, EditCar parentWindow)
        {
            _client = client;
            _parentWindow = parentWindow;

            InitializeComponent();
            DataContext = client;
        }

        private void Delete_Click(object sender, RoutedEventArgs e) => _parentWindow.DeletePriceInPriceView(_client);
    }
}
