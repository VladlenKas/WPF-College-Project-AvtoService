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

namespace AvtoService_3cursAA.UserControls.CheckUC
{
    /// <summary>
    /// Логика взаимодействия для DetailItem.xaml
    /// </summary>
    public partial class DetailItem : UserControl
    {
        public int Cost
        {
            get => _count * _detail.Cost;
        }

        private Avtoservice3cursAaContext dbContext;
        private Detail _detail;
        private int _count = 1;
        private int _allCount;
        private PriceOpertor _parentWindow;
        public DetailItem(Detail detail, PriceOpertor parentWindow)
        {
            _detail = detail;
            _parentWindow = parentWindow;

            InitializeComponent();
            dbContext = new();

            DataContext = detail;
            _allCount = _detail.Count - 1;

            CostTextBlock.Text = $"{(_detail.Cost * _count)} руб.";
            CountTextBlock.Text = $"{_count} шт.";
        }

        private void ReduceCount_Click(object sender, RoutedEventArgs e)
        {
            _count--;
            _allCount++;
            CostTextBlock.Text = $"{(_detail.Cost * _count)} руб.";
            CountTextBlock.Text = $"{_count} шт.";

            if (_count == 0)
            {
                _parentWindow.DeleteDetailInDetailView(_detail);
            }
        }

        private void AddCount_Click(object sender, RoutedEventArgs e) 
        {
            if (_allCount > 0)
            {
                _allCount--;
                _count++;
                CostTextBlock.Text = $"{(_detail.Cost * _count)} руб.";
                CountTextBlock.Text = $"{_count} шт.";
            }
            else MessageBox.Show("Вы уже выбрали все детали, доступные на складе!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void Delete_Click(object sender, RoutedEventArgs e) => _parentWindow.DeleteDetailInDetailView(_detail);
    }
}
