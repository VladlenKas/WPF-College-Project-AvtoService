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
        private Price _price;
        private Avtoservice3cursAaContext _dbContext;
        public PriceCardEdit(Price price)
        {
            _price = price;

            InitializeComponent();
            DataLoad();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            EditPrice editPrice = new(_price);
            editPrice.ShowDialog();

            DataLoad();
        }

        private void DataLoad()
        {
            _dbContext = new();
            _price = _dbContext.Prices.First(r => r.IdPrice == _price.IdPrice);
            DataContext = _price;

            if (_price.Photo == null)
            {
                string file = "pack://application:,,,/AvtoService_3cursAA;component/Images/NoImagePrice.jpg";
                ImageBorder.ImageSource = new BitmapImage(new Uri(file, UriKind.Absolute));
            }
        }

        private void DeletePrice()
        {

        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы уверены, что хотите удалить услугу?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                DeletePrice();
                MessageBox.Show("Услуга успешно удалена!", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
