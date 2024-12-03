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
using System.Windows.Shapes;

namespace AvtoService_3cursAA.UserControls.CarUC
{
    /// <summary>
    /// Логика взаимодействия для ClientsMessege.xaml
    /// </summary>
    public partial class ClientsMessege : Window
    {
        private Avtoservice3cursAaContext dbContext;
        private Car _car;
        public string Clients { get; set; }
        public string CarName { get; set; }
        public ClientsMessege(Car car)
        {
            InitializeComponent();

            dbContext = new();
            dbContext.Cars
                .Include(c => c.Carclients)
                .ThenInclude(cc => cc.IdClientNavigation)
                .Load();
            _car = dbContext.Cars.Single(r => r.IdCar == car.IdCar);

            CarName = _car.Title;

            List<Carclient> carclients = new List<Carclient>();
            carclients = _car.Carclients
                .Where(cc => cc.IsDeleted != true)
                .Where(cc => cc.IdClientNavigation.IsDeleted != true)
                .ToList();

            foreach (var item in carclients)
            {
                Clients += $"{item.IdClientNavigation.FullName}\n";
            }

            DataContext = this;
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                this.DragMove(); // Начинаем перетаскивание окна
            }
        }
    }
}
