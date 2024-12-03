using AvtoService_3cursAA.Model;
using AvtoService_3cursAA.PagesMenuAdmin;
using AvtoService_3cursAA.Actions;
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
using AvtoService_3cursAA.Actions.Cars;
using AvtoService_3cursAA.PagesMenuOperator;
using Microsoft.EntityFrameworkCore;

namespace AvtoService_3cursAA.UserControls.CarUC
{
    /// <summary>
    /// Логика взаимодействия для CarCardEdit.xaml
    /// </summary>
    public partial class CarCardEdit : UserControl
    {
        public Car Car { get; private set; }

        private Car _car;
        private CarOperator _parentWindow;
        private Avtoservice3cursAaContext dbContext;

        public event EventHandler<CarEventArgs> RemoveCarRequested; // Событие для удаления машины

        public CarCardEdit(Car car, CarOperator carOperator)
        {
            dbContext = new();
            dbContext.Cars.Include(c => c.Carclients).Load(); 

            _car = car;
            _parentWindow = carOperator;

            InitializeComponent();
            DataLoad();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            dbContext.Cars.Include(c => c.Carclients).Load(); 
            EditCar editCar = new EditCar(_car);
            editCar.ShowDialog();

            DataLoad();
            RemoveCarRequested?.Invoke(this, new CarEventArgs { Car = this.Car }); // Уведомляем родительское окно
        }

        private void DataLoad()
        {
            dbContext = new();
            _car = dbContext.Cars.First(r => r.IdCar == _car.IdCar);
            DataContext = _car;

            if (_car.Photo == null)
            {
                string file = "pack://application:,,,/AvtoService_3cursAA;component/Images/NoImageCar.jpg";
                ImageBorder.ImageSource = new BitmapImage(new Uri(file, UriKind.Absolute));
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы уверены, что хотите удалить машину?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question)
                == MessageBoxResult.Yes)
            {
                DeleteCar();
                MessageBox.Show("Машина успешно удалена!", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                RemoveCarRequested?.Invoke(this, new CarEventArgs { Car = this.Car }); // Уведомляем родительское окно
            }
        }

        private void DeleteCar()
        {
            using (var context = new Avtoservice3cursAaContext())
            {
                context.Cars.Include(c => c.Carclients).Load();

                // Найти все записи Carclient, связанные с удаляемой машиной
                var carClientsToRemove = context.Cars
                    .Where(cc => cc.IdCar == _car.IdCar)
                    .SelectMany(c => c.Carclients)
                    .ToList();

                // Удалить все связанные записи Carclient
                if (carClientsToRemove.Count != 0)
                {
                    foreach (var item in carClientsToRemove)
                    {
                        var carclient = context.Carclients.First(cc => cc.IdCar == item.IdCar);
                        carclient.IsDeleted = true;
                        context.Update(carclient);
                    }
                }

                // Найти саму машину для удаления
                var carToRemove = context.Cars.First(c => c.IdCar == _car.IdCar);
                if (carToRemove != null)
                {
                    carToRemove.IsDeleted = true;
                    context.SaveChanges();
                } 
            }
        }

        private void Info_Click(object sender, RoutedEventArgs e)
        {
            ClientsMessege clientsMessege = new ClientsMessege(_car);
            clientsMessege.ShowDialog();
        }
    }

    public class CarEventArgs : EventArgs
    {
        public Car Car { get; set; }
    }
}
