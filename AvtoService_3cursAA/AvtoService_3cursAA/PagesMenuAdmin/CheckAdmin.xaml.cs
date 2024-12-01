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
using static AvtoService_3cursAA.PagesMenuAdmin.DataManagers.ClientsAndCarsManager;

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
        private Employee _thisUser;

        private static Avtoservice3cursAaContext dbContext;
        private DetailManager detailManager;
        private PriceManager priceManager;
        private ClientsAndCarsManager clientsAndCarsManager;

        // Общая стоимость
        private int _costTotal;
        public int CostTotal
        {
            get { return _costTotal; }
            set {  _costTotal = value; }
        }

        // Стоимость для клиента
        private int _costForClient;
        public int CostForClient
        {
            get
            {
                if (detailManager != null && priceManager != null)
                {
                    _costForClient = 0;
                    int cost = detailManager.costDetail + priceManager.costPrice; // Складываем все суммы

                    _costTotal = cost; // Передаем общую стоимость

                    // Если гарант, то делаем скидку
                    if (_selectTypeofrepair != null && 
                        _selectTypeofrepair.Name is "Гарантийный случай" &&
                        TypeOfRepairComboBox.SelectedIndex != 0)
                    {
                        cost = Convert.ToInt32((Convert.ToDouble(cost) * 0.8));
                    }
                     
                    return cost;
                }
                return 0;
            }
            private set
            {
                // Сбрасываем старую цену для клиента и присваиваем новую
                _costForClient = 0;
                _costForClient = value;

                // Сбрасываем старую итоговую стоимость и присваиваем новую
                _costTotal = 0;
                _costTotal = value;

                finalCostTextBlock.Text = _costForClient.ToString();
            }
        }

        // Выранный клиент
        public Client? SelectedClient
        {
            get 
            { 
                if (clientsAndCarsManager != null)
                {
                    return clientsAndCarsManager.SelectedClient; 
                }
                return null;
            }
            set 
            {
                clientsAndCarsManager.SelectedClient = value;
            }
        }

        // Выбранное авто
        public Car? SelectedCar
        {
            get 
            {
                if (clientsAndCarsManager != null)
                {
                    return clientsAndCarsManager.SelectedCar; 
                }
                return null;
            }
            set 
            {
                clientsAndCarsManager.SelectedCar = value;
            }
        }

        public CheckAdmin(Employee employee)
        {
            InitializeComponent();

            dbContext = new Avtoservice3cursAaContext();

            this._thisUser = employee;
            finalCostTextBlock.Text = CostForClient.ToString();
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

        #region РАБОТА С ДАННЫМИ
        private void TypeOfRepairComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox != null && comboBox.SelectedIndex != 0)
            {
                string type = comboBox.SelectedValue.ToString();
                _selectTypeofrepair = dbContext.Typeofrepairs.First(c => c.Name == type);
            }

            UpdateFinalCost();
            VisibilityButtonAdd();
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
        }

        // Очистка всех полей и комбобоксов
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            ClearFields();
        }

        // Обновление итоговой цены
        internal void UpdateFinalCost()
        {
            if (_selectTypeofrepair != null && 
                _selectTypeofrepair.Name is "Гарантийный случай" &&
                TypeOfRepairComboBox.SelectedIndex != 0)
            {
                finalCostTextBlock.Text = $"{CostForClient} руб + 20% скидка";
            }
            else
            {
                finalCostTextBlock.Text = $"{CostForClient} руб.";
            }
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

        private void PdfSave()
        {
            bool allFieldsFilled = CheckFields();
            if (!allFieldsFilled) return;


        }

        private void WordSave()
        {
            bool allFieldsFilled = CheckFields();
            if (!allFieldsFilled) return;


        }

        private void ExcelSave()
        {
            bool allFieldsFilled = CheckFields();
            if (!allFieldsFilled) return;


        }
        #endregion

        // Оформление чеков
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Вы действительно заполнили все поля верно?", "Подтверждение оформления чека",
                        MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                List<(int IdDetail, int count)> details = new(detailManager.GetDetails());
                List <Price> prices = new List<Price>(priceManager.ReturnPrices());

                ActionsData.AddOrder(_thisUser, SelectedClient, SelectedCar, _selectTypeofrepair,
                    prices, details, CostForClient, CostTotal);
            }

            ClearFields();
        }

        // Очистка всех полей
        private void ClearFields()
        {
            detailManager.ClearListView();
            UpdateFinalCost();

            priceManager.ClearListView();
            UpdateFinalCost();

            TextForCars.Text = "Сначала выберите клиента";
            TextForClients.Text = "Выберите клиента";
            clientsAndCarsManager = new ClientsAndCarsManager(ClientComboBox, TextForClients, CarComboBox, TextForCars, this);
            TypeOfRepairComboBox.SelectedIndex = 0;
        }

        // Проверка на то, что все поля заполнены
        internal void VisibilityButtonAdd()
        {
            // Проверяем, заполнены ли все необходимые поля
            bool allFieldsFilled = CheckFields();

            // Включаем или отключаем кнопку в зависимости от состояния полей
            if (allFieldsFilled)
            {
                // Добавляем обработчик события
                AddButton.Click += AddButton_Click;
            }
            else
            {
                // Удаляем обработчик события
                AddButton.Click -= AddButton_Click;
            }

            // Устанавливаем подсказку для кнопки
            AddButton.Opacity = allFieldsFilled ? 1 : 0.5;
            AddButton.ToolTip = allFieldsFilled ? null : "Пожалуйста, заполните все поля перед созданием заказа.";
        }

        private bool CheckFields()
        {
            // Проверяем, заполнены ли все необходимые поля
            bool allFieldsFilled = SelectedClient != null &&
                                   SelectedCar != null &&
                                   TypeOfRepairComboBox.SelectedIndex != 0 &&
                                   ListViewPriceItems.Items.Count != 0 &&
                                   ListViewDetailItems.Items.Count != 0;

            return allFieldsFilled;
        }
    }
}
    