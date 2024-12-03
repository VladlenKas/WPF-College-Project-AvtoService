using AvtoService_3cursAA.ActionsForEmployee;
using AvtoService_3cursAA.Classes;
using AvtoService_3cursAA.DataActions;
using AvtoService_3cursAA.Model;
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

namespace AvtoService_3cursAA.PagesMenuAdmin
{
    /// <summary>
    /// Логика взаимодействия для DetailAdmin.xaml
    /// </summary>
    public partial class DetailAdmin : Page
    {
        private Avtoservice3cursAaContext dbContext;
        private Employee _selectUser;
        private Employee _thisUser;

        private DetailFilter detailFilter;

        public DetailAdmin(Employee employee)
        {
            this._thisUser = employee;

            InitializeComponent();
            DataLoad();
            UpdateItemsListView();
        }

        private void UpdateItemsListView()
        {
            dbContext = new Avtoservice3cursAaContext();

            detailFilter = new DetailFilter(SearchTextBox, ComboBoxSort, SortCheckBox, StartCostTextBox, FinishCostTextBox);
            ObservableCollection<Detail> itemsList = new ObservableCollection<Detail>(dbContext.Details.ToList());

            itemsList = detailFilter.ApplySorter(itemsList);
            itemsList = detailFilter.ApplyStartCost(itemsList);
            itemsList = detailFilter.ApplyFinishCost(itemsList);
            itemsList = detailFilter.ApplySearch(itemsList);

            ListViewItems.Items.Clear();
            foreach (var item in itemsList)
            {
                ListViewItems.Items.Add(new DetailCardView(item));
            }
        }

        private void DataLoad()
        {
            // dbContext load
            dbContext = new Avtoservice3cursAaContext();
            dbContext.Details.Load();

            // ComboBoxes load
            var sorterList = FillDataFilterSorter.FillSorterDetails();
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
                detailFilter.ApplyClear();
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
    }
}
