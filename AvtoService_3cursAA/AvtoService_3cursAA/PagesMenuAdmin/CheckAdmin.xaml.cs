using AvtoService_3cursAA.ActionsForEmployee;
using AvtoService_3cursAA.Model;
using AvtoService_3cursAA.PagesMenuAdmin.Collections;
using AvtoService_3cursAA.PagesMenuAdmin.DataManagers;
using AvtoService_3cursAA.UserControls.CheckUC;
using AvtoService_3cursAA.UserControls.PriceUC;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для CheckAdmin.xaml
    /// </summary>
    public partial class CheckAdmin 
    {
        private Client _selectClient;
        private Car _selectCar;
        private Typeofrepair _selectTypeofrepair;
        private Status _selectStatus;
        private Employee _thisUser;

        private static Avtoservice3cursAaContext dbContext;
        private DetailManager detailManager;
        private PriceManager priceManager;
        private ClientsAndCarsManager clientsAndCarsManager;

        private int _finalCost;
        public int FinalCost
        {
            get
            {
                if (detailManager != null && priceManager != null)
                {
                    _finalCost = 0;
                    var cost = detailManager.costDetail + priceManager.costPrice;
                    return cost;
                }
                return 0;
            }
            private set
            {
                _finalCost = 0;
                _finalCost = value;
                finalCostTextBox.Text = _finalCost.ToString();
            }
        }

        public CheckAdmin(Employee employee)
        {
            InitializeComponent();

            dbContext = new Avtoservice3cursAaContext();

            this._thisUser = employee;
            finalCostTextBox.Text = FinalCost.ToString();
        }

        #region РАБОТА С УСЛУГАМИ
        public void DeletePriceInPriceView(Price price)
        {
            priceManager.DeletePriceInPriceView(price);
            UpdateFinalCost();
        }
        #endregion

        #region РАБОТА С ДЕТАЛЯМИ
        public void DeleteDetailInDetailView(Detail detail)
        {
            detailManager.DeleteDetailInDetailView(detail);
            UpdateFinalCost();
        }

        public void LoadInDetailView(Detail detail)
        {
            detailManager.LoadDetailInDetailView(detail);
        }
        #endregion

        #region РАБОТА С ЗАЯВКАМИ
        private void CarComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            /*var comboBox = sender as ComboBox;
            if (comboBox != null && comboBox.SelectedIndex != 0)
            {
                string car = comboBox.SelectedValue.ToString();
                int idCar = ConvertStringToId(car);

                _selectCar = dbContext.Cars.First(c => c.IdCar == idCar);
            }*/
        }
        private void TypeOfRepairComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox != null && comboBox.SelectedIndex != 0)
            {
                string type = comboBox.SelectedValue.ToString();
                _selectTypeofrepair = dbContext.Typeofrepairs.First(c => c.Name == type);
            }
        }
        private void StatusComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox != null && comboBox.SelectedIndex != 0)
            {
                string status = comboBox.SelectedValue.ToString();
                _selectStatus = dbContext.Statuses.First(c => c.Name == status);
            }
        }
        #endregion

        #region МЕТОДЫ ДЛЯ СТРАНИЦЫ

        // Первоначальная подгрузка данных
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            UserFio.Text = $"{_thisUser.FullName}";
            EmployeeTextBox.Text = $"{_thisUser.FullName}";

            detailManager = new DetailManager(ListViewDetailItems, comboBoxDetail, costDetails, TextForDetails, this);
            priceManager = new PriceManager(ListViewPriceItems, comboBoxPrices, costPrices, TextForPrices, this);

            clientsAndCarsManager = new ClientsAndCarsManager(ClientComboBox, TextForClients, CarComboBox, TextForCars, this);

            var TypeOfRepairList = FillDataFilterSorter.FillTypeOfStatusRepair();
            TypeOfRepairComboBox.ItemsSource = TypeOfRepairList;    

            var StatusList = FillDataFilterSorter.FillStatus();
            StatusComboBox.ItemsSource = StatusList;
        }

        // Метод удалаяет все символы в строке кроме тех, что в квадртаных скобках
        private int ConvertStringToId(string str)
        {
            string idStr = string.Empty;
            string strNew = str;

            foreach(char c in strNew)
            {
                if (c == '[')
                {
                    strNew = strNew.Substring(1, strNew.Length - 1);
                    foreach (char c2 in strNew)
                    {
                        if (c2 == ']')
                        {
                            break;
                        }

                        idStr += c2;
                        strNew = strNew.Substring(1, strNew.Length - 1);
                    }
                    break;
                };

                strNew = strNew.Substring(1, strNew.Length - 1);
            }

            int id = int.Parse(idStr);
            return id;
        }

         // Очистка листа с деталями
        private void ClearDetailList_Click(object sender, RoutedEventArgs e)
        {
            detailManager.ClearListView();
            UpdateFinalCost();
        }

        // Очистка листа с услугами
        private void ClearPriceList_Click(object sender, RoutedEventArgs e)
        {
            priceManager.ClearListView();
            UpdateFinalCost();
        }

        // Очистка комобоксов с данными
        private void ClearDataButton_Click(object sender, RoutedEventArgs e)
        {
            TextForCars.Text = "Сначала выберите клиента";
            TextForClients.Text = "Выберите клиента";
            clientsAndCarsManager = new ClientsAndCarsManager(ClientComboBox, TextForClients, CarComboBox, TextForCars, this);
            TypeOfRepairComboBox.SelectedIndex = 0;
            StatusComboBox.SelectedIndex = 0;
        }

        // Очистка всех полей и комбобоксов
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            detailManager.ClearListView();
            UpdateFinalCost();

            priceManager.ClearListView(); 
            UpdateFinalCost();

            TextForCars.Text = "Сначала выберите клиента";
            TextForClients.Text = "Выберите клиента";
            clientsAndCarsManager = new ClientsAndCarsManager(ClientComboBox, TextForClients, CarComboBox, TextForCars, this);
            TypeOfRepairComboBox.SelectedIndex = 0;
            StatusComboBox.SelectedIndex = 0;
        }

        // Обновление итоговой цены
        internal void UpdateFinalCost()
        {
            finalCostTextBox.Text = FinalCost.ToString();
        }

        #endregion

        #region МЕТОДЫ С ФАЙЛАМИ
        private void pdfButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void wordButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void excelButton_Click(object sender, RoutedEventArgs e)
        {

        }
        #endregion

        // Оформление чеков
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
    