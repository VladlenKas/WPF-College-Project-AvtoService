using AvtoService_3cursAA.ActionsEmployee;
using AvtoService_3cursAA.ActionsForEmployee;
using AvtoService_3cursAA.Classes;
using AvtoService_3cursAA.Model;
using AvtoService_3cursAA.UserControls.PriceUC;
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
            dbContext = new();

            priceFilter = new PriceFilter(SearchTextBox, ComboBoxSort, SortCheckBox, StartCostTextBox, FinishCostTextBox);
            var itemsList = dbContext.Prices.ToList();

            itemsList = priceFilter.ApplySorter(itemsList);
            itemsList = priceFilter.ApplyStartCost(itemsList);
            itemsList = priceFilter.ApplyFinishCost(itemsList);
            itemsList = priceFilter.ApplySearch(itemsList);

            ListViewItems.Items.Clear();
            foreach (var item in itemsList)
            {
                ListViewItems.Items.Add(new PriceCardEdit(item));
            }
        }
        private void DataLoad()
        {
            // dbContext load
            dbContext = new();
            dbContext.Prices.Load();

            // ComboBoxes load
            var sorterList = FillDataFilterSorter.FillSorterPrices();
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
                priceFilter.ApplyClear();
                UpdateItemsListView();
            }
        }
        private void StartCostTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ListViewItems.Items != null)
            {
                var cost = StartCostTextBox.Text;
                if (int.TryParse(cost, out int parsedCost))
                {
                    if (parsedCost > 999)
                        UpdateItemsListView();
                }
                else if (string.IsNullOrEmpty(cost))
                        UpdateItemsListView();
            }
        }
        private void FinishCostTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ListViewItems.Items != null)
            {
                UpdateItemsListView();
                var cost = FinishCostTextBox.Text;
                if (int.TryParse(cost, out int parsedCost))
                {
                    if (parsedCost > 999)
                        UpdateItemsListView();
                }
                else if (string.IsNullOrEmpty(cost))
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

