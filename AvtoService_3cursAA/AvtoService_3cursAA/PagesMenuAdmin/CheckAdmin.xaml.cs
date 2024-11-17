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
using System.Data;
using System.Linq;
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
    public partial class CheckAdmin : Page
    {
        private Client _selectClient;
        private Car _selectCar;
        private Typeofrepair _selectTypeofrepair;
        private Status _selectStatus;
        private Employee _thisUser;

        private static Avtoservice3cursAaContext dbContext;

        private DetailManager detailManager;
        private PriceManager priceManager;
        public CheckAdmin(Employee employee)
        {
            InitializeComponent();

            dbContext = new();
            this._thisUser = employee;
        }

        // работа с данными УСЛУГ
        public void DeletePriceInPriceView(Price price) => priceManager.DeletePriceInPriceView(price);
        private void PriceComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) => priceManager?.PriceComboBox_SelectionChanged();

        // работа с данными ТОВАРОВ
        public void DeleteDetailInDetailView(Detail detail) => detailManager.DeleteDetailInDetailView(detail);
        private void ComboBoxDetail_SelectionChanged(object sender, SelectionChangedEventArgs e) => detailManager?.ComboBoxDetails_SelectionChanged();

        // работа с данными ЗАЯВКАМИ
        private void ClientComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox != null)
            {
                CarComboBox.SelectedIndex = 0;

                if (comboBox.SelectedIndex != 0)
                {
                    dbContext.Clients
                            .Include(c => c.Carclients)
                            .ThenInclude(cc => cc.IdCarNavigation).Load();

                    string fullname = comboBox.SelectedValue.ToString();
                    _selectClient = dbContext.Clients.AsEnumerable().First(r => r.FullName == fullname);

                    var CarList = FillDataFilterSorter.FillListCars(_selectClient);
                    CarComboBox.ItemsSource = CarList;
                }
                else
                {
                    var CarList = FillDataFilterSorter.FillListCars();
                    CarComboBox.ItemsSource = CarList;
                }
            }
        }
        private void CarComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox != null && comboBox.SelectedIndex != 0)
            {
                string car = comboBox.SelectedValue.ToString();
                int idCar = ConvertStringToId(car);

                _selectCar = dbContext.Cars.First(c => c.IdCar == idCar);
            }
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

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            UserFio.Text = $"{_thisUser.FullName}";
            EmployeeTextBox.Text = $"{_thisUser.FullName}";

            detailManager = new DetailManager(ListViewDetailItems, comboBoxDetail, this);
            priceManager = new PriceManager(ListViewPriceItems, comboBoxPrices, this);

            var ClientList = FillDataFilterSorter.FillListClients();
            ClientComboBox.ItemsSource = ClientList;

            var CarList = FillDataFilterSorter.FillListCars();
            CarComboBox.ItemsSource = CarList;

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

        private void ClearDetailList_Click(object sender, RoutedEventArgs e)
        {
            detailManager = new DetailManager(ListViewDetailItems, comboBoxDetail, this);

        }

        private void ClearPriceList_Click(object sender, RoutedEventArgs e)
        {
            priceManager = new PriceManager(ListViewPriceItems, comboBoxPrices, this);
        }
    }
}
