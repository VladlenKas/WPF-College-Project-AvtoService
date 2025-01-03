﻿using AvtoService_3cursAA.ActionsForEmployee;
using AvtoService_3cursAA.Model;
using AvtoService_3cursAA.PagesMenuAdmin.DataManagers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System;
using System.Windows.Forms;
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
using OfficeOpenXml;
using System.IO;
using PdfSharp.Pdf.Filters;


namespace AvtoService_3cursAA.PagesMenuAdmin
{
    /// <summary>
    /// Логика взаимодействия для CheckAdmin.xaml
    /// </summary>
    public partial class CheckAdmin : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged; // реализуем интерфейс
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Поля данных
        private Typeofrepair _selectTypeofrepair;
        private Employee _thisUser;

        // Инициализация классов и контекста для работы с данными
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

        // Общая стоимость для клиента
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

                FinalCostTextBlock = _costForClient.ToString();
            }
        }

        // Текст цены для окна
        private string finalCostTextBlock;
        public string FinalCostTextBlock {
            get { return finalCostTextBlock; }
            set
            {
                if (finalCostTextBlock != value)
                {
                    finalCostTextBlock = value;
                    OnPropertyChanged(); // Уведомляем об изменении свойства
                }
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
        private Car? _selectedCar;
        public Car? SelectedCar
        {
            get { return _selectedCar; } 
            set 
            { 
                _selectedCar = value;
                VisibilityButtonAdd();
            }
        }

        private int bordersVisible = 0; // Поле для хранения выбора оформления чека
        public CheckAdmin(Employee employee)
        {
            InitializeComponent();

            dbContext = new Avtoservice3cursAaContext();

            this._thisUser = employee;
            FinalCostTextBlock = CostForClient.ToString();

            pricesRadioButton.IsChecked = true;
            detailsBorder.Visibility = Visibility.Hidden;
            bordersVisible = 1;
        }

        #region Методы для работы с данными для чека

        public void DeletePriceInPriceView(Price price)
        {
            priceManager.DeletePriceInPriceView(price);
            UpdateFinalCost();
        }

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

        #region Обработчики событий

        // Первоначальная подгрузка данных
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            UserFio.Text = $"{_thisUser.FullName}";
            EmployeeTextBox.Text = $"{_thisUser.FullName}";

            detailManager = new DetailManager(ListViewDetailItems, comboBoxDetail, costDetails, TextForDetails, this);
            priceManager = new PriceManager(ListViewPriceItems, comboBoxPrices, costPrices, TextForPrices, this);

            clientsAndCarsManager = new ClientsAndCarsManager(ClientComboBox, TextForClients, CarComboBox, TextForCars, this);

            var TypeOfRepairList = FillDataFilterSorter.FillTypeOfRepairList();
            TypeOfRepairComboBox.ItemsSource = TypeOfRepairList;

            costTextBlock.DataContext = this;
        }

        // Обработчик события для выбора типа ремонта
        private void TypeOfRepairComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox != null && comboBox.SelectedIndex != 0)
            {
                string type = comboBox.SelectedValue.ToString();
                _selectTypeofrepair = dbContext.Typeofrepairs.Single(c => c.Name == type);
            }

            UpdateFinalCost();
            VisibilityButtonAdd();
        }

        // Обработчик события для выбора типа чека
        private void CheckingRadioButtonsClick(object sender, RoutedEventArgs e)
        {
            if (sender != null)
            {
                RadioButton radioButton = sender as RadioButton;
                if (radioButton.Name is "detailsRadioButton")
                {
                    priceBorder.Visibility = Visibility.Hidden;
                    detailsBorder.Visibility = Visibility.Visible;

                    priceManager.ClearListView();
                    UpdateFinalCost();

                    bordersVisible = 2;
                }
                else if (radioButton.Name is "pricesRadioButton")
                {
                    detailsBorder.Visibility = Visibility.Hidden;
                    priceBorder.Visibility = Visibility.Visible;

                    detailManager.ClearListView();
                    UpdateFinalCost();

                    bordersVisible = 1;
                }
            }
        }

        // Очистка всех полей и комбобоксов
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Очистить все поля?", "Подтверждение",
                        MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                ClearFields();
            }
        }

        // Оформление чеков
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Вы действительно заполнили все поля верно?", "Подтверждение оформления чека",
                        MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                List<(int IdDetail, int count)> details = new(detailManager.GetDetails());
                List<Price> prices = new List<Price>(priceManager.ReturnPrices());

                if (bordersVisible == 1)
                {
                    ActionsData.AddOrderPrices(_thisUser, SelectedClient, SelectedCar, _selectTypeofrepair,
                                prices, CostForClient, CostTotal);
                }
                else if (bordersVisible == 2)
                {
                    ActionsData.AddOrderDetails(_thisUser, SelectedClient, SelectedCar, _selectTypeofrepair,
                                details, CostForClient, CostTotal);
                }

                // Делаеи кнопку недействительной для избежания дублирования чека
                AddButton.Click -= AddButton_Click;
                AddButton.Opacity = 0.5;
                AddButton.ToolTip = "Осуществите вывод чека или начните заполнять новый для оформления.";

                // Делаем выводы доступными
                excelButton.Click += SaveButton_Click;
                wordButton.Click += SaveButton_Click;
                pdfButton.Click += SaveButton_Click;

                // Делаем кнопки темнее для эффекта некликабельности
                wordGrid.Opacity = 1;
                excelGrid.Opacity = 1;
                pdfGrid.Opacity = 1;

                // Добавляем описание при наведении для ясности действий
                string text = "Вывести чек в документ ";
                wordButton.ToolTip = text + "Word.";
                excelButton.ToolTip = text + "Excel.";
                pdfButton.ToolTip = text + "Pdf.";
            }
        }

        #endregion

        #region Методы для страницы

        // Обновление итоговой цены
        internal void UpdateFinalCost()
        {
            if (_selectTypeofrepair != null && 
                _selectTypeofrepair.Name is "Гарантийный случай" &&
                TypeOfRepairComboBox.SelectedIndex != 0)
            {
                FinalCostTextBlock = $"{CostForClient} руб. (20% скидка)";
            }
            else
            {
                FinalCostTextBlock = $"{CostForClient} руб.";
            }
        }

        // Очистка всех полей
        private void ClearFields()
        {
            detailManager.ClearListView();
            UpdateFinalCost();

            priceManager.ClearListView();
            UpdateFinalCost();

            SelectedClient = null;
            SelectedCar = null;
            TextForCars.Text = "Сначала выберите клиента";
            TextForClients.Text = "Выберите клиента";
            TypeOfRepairComboBox.SelectedIndex = 0;
        }

        // Управляет видимостью кнопок
        internal void VisibilityButtonAdd()
        {
            // Удаляем события для того, чтобы кнопки были доступны только 
            // после оформления чека (они становятся доступными в другом методе)
            excelButton.Click -= SaveButton_Click;
            wordButton.Click -= SaveButton_Click;
            pdfButton.Click -= SaveButton_Click;
            // Делаем кнопки темнее для эффекта некликабельности
            wordGrid.Opacity = 0.5;
            excelGrid.Opacity = 0.5;
            pdfGrid.Opacity = 0.6;

            string text = "Для начала, оформите заказ перед выбором опции.";
            wordButton.ToolTip = text;
            excelButton.ToolTip = text;
            pdfButton.ToolTip = text;

            // Проверяем, заполнены ли все необходимые поля
            bool allFieldsFilled = CheckFields();

            // Включаем или отключаем кнопку в зависимости от состояния полей
            if (allFieldsFilled)
            {
                // Удаляем обработчик события, чтобы не возникало повторений
                AddButton.Click -= AddButton_Click;

                // Добавляем обработчик события
                AddButton.Click += AddButton_Click;
            }
            else
            {
                // Удаляем обработчик события
                AddButton.Click -= AddButton_Click;
            }

            AddButton.Opacity = allFieldsFilled ? 1 : 0.5;
            AddButton.ToolTip = allFieldsFilled ? null : "Пожалуйста, заполните все поля перед созданием заказа.";
        }

        // Проверка на то, что поля заполнены
        private bool CheckFields()
        {
            if (bordersVisible == 1)
            {
                // Если доступны только услуги
                bool allFieldsFilled = SelectedClient != null &&
                                       SelectedCar != null &&
                                       TypeOfRepairComboBox.SelectedIndex != 0 &&
                                       ListViewPriceItems.Items.Count != 0;

                return allFieldsFilled;
            }
            else 
            {
                // Если доступны только детали
                bool allFieldsFilled = SelectedClient != null &&
                                       SelectedCar != null &&
                                       TypeOfRepairComboBox.SelectedIndex != 0 &&
                                       ListViewDetailItems.Items.Count != 0;

                return allFieldsFilled;
            }
        }

        #endregion

        #region Вывод в файл

        // Обработчик события для экспорта данных 
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            FileSave(sender);
        }
        #endregion

        // Сохранение файла
        private void FileSave(object sender)
        {
            // Находим максимальный айди среди всех продаж для названия нового чека продажи
            var lastSale = dbContext.Sales.OrderByDescending(s => s.IdSale).FirstOrDefault();
            var idOrder = (lastSale?.IdSale ?? 0) + 1; // Если продаж еще не было, она становится первой

            // Сохраняем имя клиента
            string client = SelectedClient.FullName;

            // Определяем тип чека
            string typeSale = "";
            bool isDetailsCheck = detailsBorder.Visibility == Visibility.Visible;
            if (isDetailsCheck)
            {
                typeSale = "детали";
            }
            else
            {
                typeSale = "услуги";
            }


            // Определяем, во что вывести
            string filter = "";
            string title = "";

            Button button = sender as Button;
            switch (button.Name)
            {
                case "excelButton":
                    filter = "Excel Files|*.xlsx*";
                    title = "Сохранить Excel файл";
                    break;

                case "pdfButton":
                    filter = "Pdf Files|*.pdf*";
                    title = "Сохранить Pdf файл";
                    break;

                case "wordButton":
                    filter = "Word Files|*.docx*";
                    title = "Сохранить Word файл";
                    break;
            }


            // Вызываем диалоговое окно для сохранения таблицы
            SaveFileDialog saveFileDialog = new SaveFileDialog()
            {
                Filter = filter,
                Title = title,
                FileName = $"Чек ({idOrder}) {client} ({typeSale})"
            };

            // Если пользователь выбрал путь для сохранения чека
            if (saveFileDialog.ShowDialog() == true)
            {
                string filePath = SelectFileType(button, saveFileDialog, idOrder); // Путь для открытия файла

                // Открываем файл при желании
                var result = MessageBox.Show("Чек успешно сохранен! Открыть его?", "Открыть?",
                    MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = filePath,
                        UseShellExecute = true // Используем оболочку Windows для открытия файла
                    });
                }
                ClearFields();
            }
        }

        // Выбор типа файла для сохранения
        private string SelectFileType(Button button, SaveFileDialog saveFileDialog, int idOrder)
        {
            bool isDetailsSale = detailsBorder.Visibility == Visibility.Visible;
            string filePath = "";

            switch (button.Name)
            {
                case "excelButton":
                    if (isDetailsSale)
                    {
                        List<(int count, string Name, int Cost)> details = new(detailManager.GetDetailsForFile());
                        filePath = FilesManager.ExcelDetails(_thisUser, SelectedClient, SelectedCar, _selectTypeofrepair,
                            details, saveFileDialog, idOrder, CostForClient, CostTotal);
                    }
                    else
                    {
                        List<Price> prices = new(priceManager.ReturnPrices());
                        filePath = FilesManager.ExcelPrices(_thisUser, SelectedClient, SelectedCar, _selectTypeofrepair,
                            prices, saveFileDialog, idOrder, CostForClient, CostTotal);
                    }
                    break;

                case "pdfButton":
                    if (isDetailsSale)
                    {
                        List<(int count, string Name, int Cost)> details = new(detailManager.GetDetailsForFile());
                        filePath = FilesManager.PdfDetails(_thisUser, SelectedClient, SelectedCar, _selectTypeofrepair,
                            details, saveFileDialog, idOrder, CostForClient, CostTotal);
                    }
                    else
                    {
                        List<Price> prices = new(priceManager.ReturnPrices());
                        filePath = FilesManager.PdfPrices(_thisUser, SelectedClient, SelectedCar, _selectTypeofrepair,
                            prices, saveFileDialog, idOrder, CostForClient, CostTotal);
                    }
                    break;

                case "wordButton":
                    if (isDetailsSale)
                    {
                        List<(int count, string Name, int Cost)> details = new(detailManager.GetDetailsForFile());
                        filePath = FilesManager.WordDetails(_thisUser, SelectedClient, SelectedCar, _selectTypeofrepair,
                            details, saveFileDialog, idOrder, CostForClient, CostTotal);
                    }
                    else
                    {
                        List<Price> prices = new(priceManager.ReturnPrices());
                        filePath = FilesManager.WordPrices(_thisUser, SelectedClient, SelectedCar, _selectTypeofrepair,
                            prices, saveFileDialog, idOrder, CostForClient, CostTotal);
                    }
                    break;
            }
            return filePath;
        }
    }
}
    