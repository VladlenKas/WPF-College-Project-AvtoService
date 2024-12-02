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

namespace AvtoService_3cursAA.UserControls.CarUC
{
    /// <summary>
    /// Логика взаимодействия для CarCardView.xaml
    /// </summary>
    public partial class CarCardView : UserControl
    {
        private Avtoservice3cursAaContext dbContext;
        private Car _car;
        public CarCardView(Car car)
        {
            InitializeComponent();

            dbContext = new();
            dbContext.Cars.Include(c => c.Carclients).Load();
            _car = dbContext.Cars.Single(r => r.IdCar == car.IdCar);

            DataContext = _car;

            if (_car.Photo == null)
            {
                string file = "pack://application:,,,/AvtoService_3cursAA;component/Images/NoImagePrice.jpg";
                ImageBorder.ImageSource = new BitmapImage(new Uri(file, UriKind.Absolute));
            }
        }

        private void Info_Click(object sender, RoutedEventArgs e)
        {
            ClientsMessege clientsMessege = new ClientsMessege(_car);
            clientsMessege.ShowDialog();
        }
    }
}
