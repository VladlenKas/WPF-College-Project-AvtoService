using AvtoService_3cursAA.ActionsForEmployee;
using AvtoService_3cursAA.Classes;
using AvtoService_3cursAA.Model;
using AvtoService_3cursAA.UserControls.CarUC;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace AvtoService_3cursAA.PagesMenuMechanic
{
    /// <summary>
    /// Логика взаимодействия для CarMechanic.xaml
    /// </summary>
    public partial class CarMechanic : Page
    {
        private Avtoservice3cursAaContext dbContext;
        private Employee _thisUser;

        private CarFilter carFilter; // Изменено на CarFilter

        public CarMechanic(Employee employee)
        {
            this._thisUser = employee;

            InitializeComponent();
            DataLoad();
            UpdateItemsListView();
        }

        private void UpdateItemsListView()
        {
            dbContext = new();

            carFilter = new CarFilter(SearchTextBox, ComboBoxSort, SortCheckBox); // Используем CarFilter
            ObservableCollection<Car> itemsList = new ObservableCollection<Car>(dbContext.Cars); // Изменено на Cars

            itemsList = carFilter.ApplySorter(itemsList);
            itemsList = carFilter.ApplySearch(itemsList);

            ListViewItems.Items.Clear();
            foreach (var item in itemsList)
            {
                ListViewItems.Items.Add(new CarCardView(item)); // Изменено на CarCardView
            }

            if (ListViewItems.Items.Count == 0)
            {
                textFound.Visibility = Visibility.Visible;
            }
            else
            {
                textFound.Visibility = Visibility.Hidden;
            }
        }

        private void DataLoad()
        {
            // dbContext load
            dbContext = new();
            dbContext.Cars.Load(); // Изменено на Cars

            // ComboBoxes load
            var sorterList = FillDataFilterSorter.FillSorterCars(); // Изменено на FillSorterCars
            ComboBoxSort.ItemsSource = sorterList;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            UserFio.Text = $"{_thisUser.FullName}";
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ListViewItems.Items != null)
                UpdateItemsListView();
        }

        private void ClearDataButton_Click(object sender, RoutedEventArgs e)
        {
            if (ListViewItems.Items != null)
            {
                carFilter.ApplyClear(); // Используем ApplyClear из CarFilter
                UpdateItemsListView();
            }
        }

        private void SortCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (ListViewItems.Items != null)
                UpdateItemsListView();
        }

        private void ComboBoxSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListViewItems.Items != null)
                UpdateItemsListView();
        }
    }
}
