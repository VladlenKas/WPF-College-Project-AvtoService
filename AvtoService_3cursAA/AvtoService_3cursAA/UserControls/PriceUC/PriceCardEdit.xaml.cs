using AvtoService_3cursAA.Actions;
using AvtoService_3cursAA.Model;
using AvtoService_3cursAA.PagesMenuOperator;
using Microsoft.EntityFrameworkCore;
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

namespace AvtoService_3cursAA.UserControls.PriceUC
{
    /// <summary>
    /// Логика взаимодействия для PriceView.xaml
    /// </summary>
    public partial class PriceCardEdit : UserControl
    {
        public Price Price { get; private set; }

        private Price _price;
        private PriceOperator _parentWindow;
        private Avtoservice3cursAaContext dbContext;

        public event EventHandler<PriceEventArgs> RemovePriceRequested; // Событие для удаления цены

        public PriceCardEdit(Price price, PriceOperator priceOperator)
        {
            _price = price;
            _parentWindow = priceOperator;

            InitializeComponent();
            DataLoad();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            EditPrice editPrice = new(_price);
            editPrice.ShowDialog();

            DataLoad();
            RemovePriceRequested?.Invoke(this, new PriceEventArgs { Price = this.Price }); // Уведомляем родительское окно
        }

        private void DataLoad()
        {
            dbContext = new();
            _price = dbContext.Prices.First(r => r.IdPrice == _price.IdPrice);
            DataContext = _price;

            if (_price.Photo == null)
            {
                string file = "pack://application:,,,/AvtoService_3cursAA;component/Images/NoImagePrice.jpg";
                ImageBorder.ImageSource = new BitmapImage(new Uri(file, UriKind.Absolute));
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы уверены, что хотите удалить услугу?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) 
                == MessageBoxResult.Yes)
            {
                DeletePrice();
                MessageBox.Show("Услуга успешно удалена!", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                RemovePriceRequested?.Invoke(this, new PriceEventArgs { Price = this.Price }); // Уведомляем родительское окно
            }
        }

        private void DeletePrice()
        {
            dbContext = new();
            var deletePrice = dbContext.Prices.First(r => r.IdPrice == _price.IdPrice);
            deletePrice.IsDeleted = true; 
            dbContext.SaveChanges();
        }
    }

    public class PriceEventArgs : EventArgs
    {
        public Price Price { get; set; }
    }
}
