using AvtoService_3cursAA.ActionsForEmployee;
using AvtoService_3cursAA.Classes;
using AvtoService_3cursAA.Model;
using AvtoService_3cursAA.UserControls.CarUC;
using AvtoService_3cursAA.UserControls.DetailUC;
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

namespace AvtoService_3cursAA.PagesMenuOperator
{
    /// <summary>
    /// Логика взаимодействия для CarOperator.xaml
    /// </summary>
    public partial class CarOperator : Page
    {
        private Avtoservice3cursAaContext dbContext;
        private Employee _thisUser;

        private CarFilter carFilter; // Изменено на CarFilter

        public CarOperator(Employee employee)
        {
            this._thisUser = employee;

            InitializeComponent();
            DataLoad();
            UpdateItemsListView();
        }

        private void UpdateItemsListView()
        {
            dbContext = new Avtoservice3cursAaContext();

            carFilter = new CarFilter(SearchTextBox, ComboBoxSort, SortCheckBox); // Используем CarFilter

            ObservableCollection<Car> itemsList = new ObservableCollection<Car>(dbContext.Cars.ToList()); // Изменено на Cars

            itemsList = carFilter.ApplySorter(itemsList);
            itemsList = carFilter.ApplySearch(itemsList); // Удалены методы фильтрации по стоимости

            ListViewItems.Items.Clear();

            foreach (var item in itemsList)
            {
                var carCardEdit = new CarCardEdit(item, this); // Изменено на CarCardEdit
                carCardEdit.RemoveCarRequested += CarCardEdit_RemoveCarRequested; // Подписка на событие удаления
                ListViewItems.Items.Add(carCardEdit);
            }
        }

        private void DataLoad()
        {
            dbContext = new Avtoservice3cursAaContext();

            dbContext.Cars.Load(); // Изменено на Cars

            var sorterList = FillDataFilterSorter.FillSorterCars(); // Изменено на FillSorterCars
            ComboBoxSort.ItemsSource = sorterList;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            UserFio.Text = $"{_thisUser.FullName}";
        }

        // Обработчик события удаления машины
        private void CarCardEdit_RemoveCarRequested(object sender, CarEventArgs e)
        {
            UpdateItemsListView(); // Обновляем список после удаления
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

        private void AddCar_Click(object sender, RoutedEventArgs e) // Изменено на AddCar
        {
            /*AddCar window = new AddCar(); // Изменено на AddCar
            window.ShowDialog();
*/
            UpdateItemsListView();
        }
    }
}

