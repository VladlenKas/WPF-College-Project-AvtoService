using AvtoService_3cursAA.Model;
using AvtoService_3cursAA.PagesMenuAdmin.Collections;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AvtoService_3cursAA.PagesMenuAdmin.DataManagers
{
    internal class ClientsAndCarsManager
    {
        // Окна для работы с классами
        public static ClientsManager _clientsManager = null!;
        public static CarsManager _carsManager = null!;

        // Свойство для передачи клиента в авто
        private static Client? _client;
        public static Client? ClientCars
        {
            get { return _client; }
            set { _client = value; }
        }

        public ClientsAndCarsManager(ComboBox comboBoxClients, TextBlock placeHolderClient,
            ComboBox comboBoxCars, TextBlock placeHolderCar, CheckAdmin parentWindow)
        {
            _clientsManager = new ClientsManager(comboBoxClients, placeHolderClient, parentWindow);
            _carsManager = new CarsManager(comboBoxCars, placeHolderCar, parentWindow);
        }

        public static void UpdateSelectedItems()
        {
            _carsManager.SelectedClient = ClientCars;
            _carsManager.UpdateClient();
        }

        public class ClientsManager
        {
            public event PropertyChangedEventHandler? PropertyChanged; // реализуем интерфейс
            protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }

            private Avtoservice3cursAaContext dbContext;

            private ObservableCollection<Client> _clients; // исходный список услуг в комбобокс
            private ObservableCollection<Client> _filteredClients; // отфильтрованный список услуг в комбобокс
            private TextBox _searchTextBox; // текстбокс внутри комбобокса

            private PriceCollection PriceCollection; // юзер контролы с услугами

            // поля для конструктора
            private ItemsControl _listViewItems;
            private ComboBox _comboBoxClients;
            private TextBlock _placeHolder;
            private CheckAdmin _parentWindow;
            private Client _selectedClient;

            // Свойство для доступа к коллекции исходных элементов
            public ObservableCollection<Client> Clients
            {
                get { return _clients; }
            }

            // Свойство для доступа к отфильтрованным элементам
            public ObservableCollection<Client> FilteredClients
            {
                get { return _filteredClients; }
                set
                {
                    _filteredClients = value;
                    OnPropertyChanged(); // Оповещаем участников об изменении
                }
            }

            public string FilterText
            {
                get { return _searchTextBox.Text; }
                set
                {
                    _searchTextBox.Text = value;
                    OnPropertyChanged(); // Оповещаем участников об изменении
                    FilterItems(); // Вызываем метод фильтрации при изменении текста

                }
            }

            // в этом свойстве храним текущего клиента,
            // которого выбрал сотрудник
            public Client? SelectedClient
            {
                get { return _selectedClient; }
                set
                {
                    _selectedClient = value;
                    ClientCars = _selectedClient;
                    UpdateSelectedItems();
                }
            }

            public ClientsManager(ComboBox comboBoxClients, TextBlock placeHolder, CheckAdmin parentWindow)
            {
                dbContext = new Avtoservice3cursAaContext();
                dbContext.Clients
                    .Include(r => r.Carclients)
                    .ThenInclude(cc => cc.IdCarNavigation)
                    .Load();

                _comboBoxClients = comboBoxClients;
                _placeHolder = placeHolder;
                _parentWindow = parentWindow;

                ClientsManagerLoad();
            }

            #region МЕТОДЫ ДЛЯ РАБОТЫ С КОЛЛЕКИЦЯМИ 

            // Подгрузка начальных данных
            private void ClientsManagerLoad()
            {
                // ииициализируем коллекции
                _clients = new ObservableCollection<Client>(dbContext.Clients.ToList());
                _filteredClients = new ObservableCollection<Client>(dbContext.Clients);

                // чтобы обрабатывать вводимый текст
                _comboBoxClients.ApplyTemplate();
                var textBox = _comboBoxClients.Template.FindName("PART_EditableTextBox", _comboBoxClients) as TextBox;
                _searchTextBox = textBox;

                // подключаем тригеры
                _searchTextBox.TextChanged += SearchTextBox_TextChanged;
                _comboBoxClients.SelectionChanged += ComboBoxClients_SelectionChanged;

                _comboBoxClients.ItemsSource = FilteredClients; // передаем источник данных комбобокса
            }

            // Филььтруем комбобокс на основе вводимого текста
            private void FilterItems()
            {
                FilteredClients.Clear(); // Очистка коллекции отфильтрованных элементов

                if (string.IsNullOrEmpty(FilterText)) // Если текст фильтра пуст или null
                {
                    foreach (var client in Clients) // Добавляем все элементы из исходной коллекции
                    {
                        FilteredClients.Add(client);
                    }
                }
                else // Если текст фильтра не пустой
                {
                    foreach (var client in Clients)
                    {
                        // Проверяем, содержится ли текст фильтра в элементе (без учета регистра)
                        if (client.FullName.ToLower().Contains(FilterText.ToLower()))
                        {
                            FilteredClients.Add(client); // Добавляем совпадающий элемент в отфильтрованную коллекцию
                        }
                    }
                }
            }
            #endregion

            #region ОБРАБОТЧИКИ СОБЫТИЙ
            public void ComboBoxClients_SelectionChanged(object sender, SelectionChangedEventArgs e)
            {
                if (_comboBoxClients != null)
                {
                    if (_comboBoxClients.SelectedItem == null) return;

                    // отключаем тригеры для того, чтобы
                    // при изменении не вызывалось ни одного тригера
                    _comboBoxClients.SelectionChanged -= ComboBoxClients_SelectionChanged;
                    _searchTextBox.TextChanged -= SearchTextBox_TextChanged;

                    // передаем выбранного пользователя в свойство и сбрасываем выбор
                    SelectedClient = _comboBoxClients.SelectedItem as Client;
                    _comboBoxClients.SelectedIndex = -1;

                    // выводим выбранного пользователя в текстблок
                    _placeHolder.Visibility = Visibility.Visible;
                    _placeHolder.Text = SelectedClient.FullName;

                    // очищаем строку от поиска
                    _searchTextBox.Text = string.Empty;

                    // возобновляем тригеры
                    _comboBoxClients.SelectionChanged += ComboBoxClients_SelectionChanged;
                    _searchTextBox.TextChanged += SearchTextBox_TextChanged;
                }
            }
            private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
            {
                if (_comboBoxClients == null) return;

                SelectedClient = null;
                _placeHolder.Text = "Выберите клиента";
                _placeHolder.Visibility = string.IsNullOrWhiteSpace(_searchTextBox.Text) ? Visibility.Visible : Visibility.Hidden;
                FilterText = _searchTextBox.Text; // Вызываем метод фильтрации при изменении текста
            }
            #endregion
        }

        public class CarsManager : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler? PropertyChanged;

            protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }

            private Avtoservice3cursAaContext dbContext;

            private ObservableCollection<Car> _cars; // Исходный список машин
            private ObservableCollection<Car> _filteredCars; // Отфильтрованный список машин
            private TextBox _searchTextBox; // Текстбокс для поиска

            private ComboBox _comboBoxCars; // Комбобокс для выбора машин
            private TextBlock _placeHolder; // Плейсхолдер для отображения выбранной машины
            private CheckAdmin _parentWindow; // Ссылка на родительское окно
            private Car _selectedCar; // Выбранная машина
            private Client _selectedClient; // Выбранный пользователь

            // Свойство для доступа к коллекции исходных элементов
            public ObservableCollection<Car> Cars
            {
                get { return _cars; }
            }

            // Свойство для доступа к отфильтрованным элементам
            public ObservableCollection<Car> FilteredCars
            {
                get { return _filteredCars; }
                set
                {
                    _filteredCars = value;
                    OnPropertyChanged(); // Оповещаем участников об изменении
                }
            }

            public string FilterText
            {
                get { return _searchTextBox.Text; }
                set
                {
                    _searchTextBox.Text = value;
                    OnPropertyChanged(); // Оповещаем участников об изменении
                    FilterItems(); // Вызываем метод фильтрации при изменении текста
                }
            }

            // в этом свойстве храним текущее авто,
            // которое выбрал сотрудник
            public Car? SelectedCar
            {
                get { return _selectedCar; }
                set { _selectedCar = value; }
            }

            // в этом свойстве храним текущего клиента,
            // которого выбрал сотрудник
            public Client? SelectedClient
            {
                get { return _selectedClient; }
                set { _selectedClient = value; }
            }
            public CarsManager(ComboBox comboBoxCars, TextBlock placeHolder, CheckAdmin parentWindow)
            {
                dbContext = new Avtoservice3cursAaContext();
                dbContext.Cars
                    .Include(r => r.Carclients)
                    .ThenInclude(cc => cc.IdClientNavigation)
                    .Load();

                _comboBoxCars = comboBoxCars;
                _placeHolder = placeHolder;
                _parentWindow = parentWindow;

                CarsManagerLoad();
            }

            #region МЕТОДЫ ДЛЯ РАБОТЫ С КОЛЛЕКЦИЯМИ 
            // Проверяем, выбран ли пользователь
            public void UpdateClient()
            {
                if (SelectedClient != null)
                {
                    List<Car> cars = new List<Car>(dbContext.Cars); // исходный список машин
                    var carsId = SelectedClient.Carclients.Select(cc => cc.IdCarNavigation.IdCar); // id машин выбранного клиента
                    List<Car> filteredCars = cars.Where(car => carsId.Contains(car.IdCar)).ToList(); // все машины клиента

                    _cars = new ObservableCollection<Car>(filteredCars);
                    _filteredCars = new ObservableCollection<Car>(filteredCars);

                    // Подключаем триггеры
                    _searchTextBox.TextChanged += SearchTextBox_TextChanged;
                    _comboBoxCars.SelectionChanged += ComboBoxCars_SelectionChanged;

                    _comboBoxCars.ItemsSource = FilteredCars; // Передаем источник данных комбобокса

                    _placeHolder.Text = "Выберите машину";
                }
                else
                {
                    // Инициализируем коллекции
                    _cars = new ObservableCollection<Car>();
                    _filteredCars = new ObservableCollection<Car>();

                    _searchTextBox.TextChanged -= SearchTextBox_TextChanged;
                    _comboBoxCars.SelectionChanged -= ComboBoxCars_SelectionChanged;

                    _placeHolder.Text = "Сначала выберите клиента";
                }
            }

            // Подгрузка начальных данных
            private void CarsManagerLoad()
            {
                // Инициализируем коллекции
                _cars = new ObservableCollection<Car>();
                _filteredCars = new ObservableCollection<Car>();

                // Чтобы обрабатывать вводимый текст
                _comboBoxCars.ApplyTemplate();
                var textBox = _comboBoxCars.Template.FindName("PART_EditableTextBox", _comboBoxCars) as TextBox;
                _searchTextBox = textBox;

                _comboBoxCars.ItemsSource = FilteredCars; // Передаем источник данных комбобокса
            }

            // Фильтруем комбобокс на основе вводимого текста
            private void FilterItems()
            {
                FilteredCars.Clear(); // Очистка коллекции отфильтрованных элементов

                if (string.IsNullOrEmpty(FilterText)) // Если текст фильтра пуст или null
                {
                    foreach (var car in Cars) // Добавляем все элементы из исходной коллекции
                    {
                        FilteredCars.Add(car);
                    }
                }
                else // Если текст фильтра не пустой
                {
                    foreach (var car in Cars)
                    {
                        // Проверяем, содержится ли текст фильтра в элементе (без учета регистра)
                        if (car.Title.ToLower().Contains(FilterText.ToLower())) // Предположим, что у Car есть свойство Model
                        {
                            FilteredCars.Add(car); // Добавляем совпадающий элемент в отфильтрованную коллекцию
                        }
                    }
                }
            }

            #endregion

            #region ОБРАБОТЧИКИ СОБЫТИЙ

            public void ComboBoxCars_SelectionChanged(object sender, SelectionChangedEventArgs e)
            {
                if (_comboBoxCars != null)
                {
                    if (_comboBoxCars.SelectedItem == null) return;

                    // отключаем тригеры для того, чтобы
                    // при изменении не вызывалось ни одного тригера
                    _comboBoxCars.SelectionChanged -= ComboBoxCars_SelectionChanged;
                    _searchTextBox.TextChanged -= SearchTextBox_TextChanged;

                    // передаем выбранное авто в свойство и сбрасываем выбор
                    SelectedCar = _comboBoxCars.SelectedItem as Car;
                    _comboBoxCars.SelectedIndex = -1;

                    // выводим выбранное авто в текстблок
                    _placeHolder.Visibility = Visibility.Visible;
                    _placeHolder.Text = SelectedCar.Title;

                    // очищаем строку от поиска
                    _searchTextBox.Text = string.Empty;

                    // возобновляем тригеры
                    _comboBoxCars.SelectionChanged += ComboBoxCars_SelectionChanged;
                    _searchTextBox.TextChanged += SearchTextBox_TextChanged;
                }
            }

            private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
            {
                if (_comboBoxCars == null) return;

                SelectedCar = null;
                _placeHolder.Text = "Выберите автомобиль";
                _placeHolder.Visibility = string.IsNullOrWhiteSpace(_searchTextBox.Text) ? Visibility.Visible : Visibility.Hidden;
                FilterText = _searchTextBox.Text; // Вызываем метод фильтрации при изменении текста
            }

            #endregion
        }
    }
}

