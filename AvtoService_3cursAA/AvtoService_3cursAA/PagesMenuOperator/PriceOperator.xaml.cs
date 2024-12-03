using AvtoService_3cursAA.Actions.Details;
using AvtoService_3cursAA.Actions.Prices;
using AvtoService_3cursAA.ActionsEmployee;
using AvtoService_3cursAA.ActionsForEmployee;
using AvtoService_3cursAA.Classes;
using AvtoService_3cursAA.DataActions;
using AvtoService_3cursAA.Model;
using AvtoService_3cursAA.UserControls.PriceUC;
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
    /// Логика взаимодействия для PriceAdmin.xaml
    /// </summary>
    public partial class PriceOperator : Page
    {
        private Avtoservice3cursAaContext dbContext;
        private Employee _selectUser;
        private Employee _thisUser;

        private PriceFilter priceFilter;

        public PriceOperator(Employee employee)
        {
            this._thisUser = employee;

            InitializeComponent();
            DataLoad();
            UpdateItemsListView();
        }

        private void UpdateItemsListView()
        {
            dbContext = new Avtoservice3cursAaContext();

            priceFilter = new PriceFilter(SearchTextBox, ComboBoxSort, SortCheckBox, StartCostTextBox, FinishCostTextBox);

            ObservableCollection<Price> itemsList = new ObservableCollection<Price>(dbContext.Prices.ToList());

            itemsList = priceFilter.ApplySorter(itemsList);
            itemsList = priceFilter.ApplyStartCost(itemsList);
            itemsList = priceFilter.ApplyFinishCost(itemsList);
            itemsList = priceFilter.ApplySearch(itemsList);

            ListViewItems.Items.Clear();

            foreach (var item in itemsList)
            {
                var priceCardEdit = new PriceCardEdit(item, this);
                priceCardEdit.RemovePriceRequested += PriceCardEdit_RemovePriceRequested; // Подписка на событие удаления
                ListViewItems.Items.Add(priceCardEdit);
            }
        }

        private void DataLoad()
        {
            dbContext = new Avtoservice3cursAaContext();

            dbContext.Prices.Load();

            var sorterList = FillDataFilterSorter.FillSorterPrices();
            ComboBoxSort.ItemsSource = sorterList;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            UserFio.Text = $"{_thisUser.FullName}";
        }

        // Обработчик события удаления цены
        private void PriceCardEdit_RemovePriceRequested(object sender, PriceEventArgs e)
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
                priceFilter.ApplyClear();
                UpdateItemsListView();
            }
        }
        private void SearchByCost_Click(object sender, RoutedEventArgs e)
        {
            if (ListViewItems.Items != null)
                UpdateItemsListView();
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            ActionsTextBox.ValidateInputNumbers(e);
        }

        private void TextBox_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            ActionsTextBox.ValidatePasteNumbers(e);
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

        private void AddPrice_Click(object sender, RoutedEventArgs e)
        {
            AddPrice window = new AddPrice();
            window.ShowDialog();

            UpdateItemsListView();
        }
    }
}

