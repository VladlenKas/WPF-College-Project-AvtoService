using AvtoService_3cursAA.Model;
using AvtoService_3cursAA.PagesMenuAdmin;
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
using static MaterialDesignThemes.Wpf.Theme;

namespace AvtoService_3cursAA.UserControls.CheckUC
{
    /// <summary>
    /// Логика взаимодействия для PriceItem.xaml
    /// </summary>
    public partial class PriceItem : UserControl
    {
        public int Cost
        {
            get => _price.Cost;
        }

        private Price _price;
        private CheckAdmin _parentWindow;

        public PriceItem(Price price, CheckAdmin parentWindow)
        {
            _price = price;
            _parentWindow = parentWindow;

            InitializeComponent();
            DataContext = price;
        }

        private void Delete_Click(object sender, RoutedEventArgs e) => _parentWindow.DeletePriceInPriceView(_price);
    }
}
