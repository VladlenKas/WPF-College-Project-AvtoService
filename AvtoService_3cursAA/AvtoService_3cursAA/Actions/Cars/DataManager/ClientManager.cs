using AvtoService_3cursAA.Model;
using AvtoService_3cursAA.PagesMenuAdmin.Collections;
using AvtoService_3cursAA.PagesMenuAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using AvtoService_3cursAA.Actions.Cars;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using AvtoService_3cursAA.DataActions;
using System.Xml.Linq;

namespace AvtoService_3cursAA.PagesMenuOperator.DataManager
{
    public class ClientManager : INotifyPropertyChanged // используем этот интерфейс для динамического обновления данных
    {
        private static Avtoservice3cursAaContext dbContext;

        private ObservableCollection<Client> _clients; // исходный список клиентов в комбобокс
        private ObservableCollection<Client> _filteredClients;// отфильтрованный список клиентов в комбобокс
        private TextBox _searchTextBox; // текстбокс внутри комбобокса

        private ClientCollection clientCollection; // юзер контролы клиентов для ClientCollection

        // поля для конструктора
        private ItemsControl _listViewItems; 
        private ComboBox _comboBoxClients;
        private EditCar _parentWindow;
        private Car _car;

        // Свойство для доступа к коллекции исходных элементов
        public ObservableCollection<Client> Clients
        {
            get { return _clients; }
            set
            {
                _clients = value;
                OnPropertyChanged(); // Оповещаем участников об изменении
            }
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

        public ClientManager(ItemsControl listViewItems, ComboBox comboBoxClients, EditCar parentWindow, Car car)
        {
            _listViewItems = listViewItems;
            _comboBoxClients = comboBoxClients;
            _parentWindow = parentWindow;
            _car = car;

            ClientManagerEdit_Load();
        }

        #region МЕТОДЫ ДЛЯ РАБОТЫ С КОЛЛЕКЦИЯМИ И СПИСКАМИ 
        /// <summary>
        /// Настройка параметров для взаимодействия с коллекциями и списками
        /// </summary>
        private void ClientManagerEdit_Load()
        {
            // Обновляем бд и подгружаем свзяи
            dbContext = new();
            dbContext.Cars.Include(c => c.Carclients).Load();

            // Загрузка всех клиентов, кроме тех, которые уже привязаны к машине
            var clientsAlreadyAssigned = _car
                .Carclients
                .Select(cc => cc.IdClient)
                .ToList();

            // инициализируем списки со всеми клиентами и список
            // для будущиъ отфильтрованных клиентов
            var listAllClients = dbContext.Clients.ToList();
            Clients = new ObservableCollection<Client>(listAllClients);
            FilteredClients = new ObservableCollection<Client>(listAllClients);

            // инициализиурем класс ClientCollection для работы с UserContol
            clientCollection = new ClientCollection(_parentWindow);
            // в качестве источника ресурсов указыаем нашу коллекицю пользователей
            // которую только что инициализировали
            _listViewItems.ItemsSource = clientCollection.Clients;

            // Удаляем уже привязанных клиентов с комбобокса и возвращаем их в коллекцию выбранных клиентов
            foreach (var client in listAllClients)
            {
                if (clientsAlreadyAssigned.Contains(client.IdClient))
                {
                    clientCollection.AddClient(client); // добавляем в itemSource ItemControl
                    Clients.Remove(client); // удаляем его из комбобокса
                }   
            }
            // обновление комбобокса
            FillClients();

            // чтобы обрабатывать вводимый текст
            _comboBoxClients.ApplyTemplate();
            var textBox = _comboBoxClients.Template.FindName("PART_EditableTextBox", _comboBoxClients) as TextBox;
            _searchTextBox = textBox;

            // подключаем тригеры
            _searchTextBox.TextChanged += SearchTextBox_TextChanged;
            _comboBoxClients.SelectionChanged += ComboBoxClients_SelectionChanged;
        }

        /// <summary>
        /// Обновление списка 
        /// </summary>
        public void FillClients()
        {
            _comboBoxClients.ItemsSource = Clients; // Присваиваем список клиентов
            _comboBoxClients.DisplayMemberPath = "FullName"; // Устанавливаем отображаемое свойство
        }

        /// <summary>
        /// Метод для добавления клиента в коллецию UserControls и
        /// удаления его из комбобокса
        /// </summary>
        public void AddClientInItemsView()
        {
            var selectedClient = _comboBoxClients.SelectedItem as Client; // инициализируем выбранный клиент

            if (selectedClient != null)
            {
                if (DataValidate(selectedClient)) 
                { 
                    clientCollection.AddClient(selectedClient); // добавляем в itemSource ItemControl
                    Clients.Remove(selectedClient); // удаляем его из комбобокса
                };
                FillClients();
            }
        }

        /// <summary>
        /// Метод для фильтрации элементов на основе текста фильтра
        /// </summary>
        private void FilterItems() 
        {
            FilteredClients.Clear(); // Очистка коллекции отфильтрованных элементов

            if (string.IsNullOrEmpty(FilterText)) // Если текст фильтра пуст или null
            {
                foreach (var product in Clients) // Добавляем все элементы из исходной коллекции
                {
                    FilteredClients.Add(product);
                }
            }
            else // Если текст фильтра не пустой
            {
                foreach (var product in Clients)
                {
                    // Проверяем, содержится ли текст фильтра в элементе (без учета регистра)
                    if (product.FullName.ToLower().Contains(FilterText.ToLower()))
                    {
                        FilteredClients.Add(product); // Добавляем совпадающий элемент в отфильтрованную коллекцию
                    }
                }
            }

            _comboBoxClients.ItemsSource = FilteredClients; // Обновляем источник данных комбобокса
            _comboBoxClients.DisplayMemberPath = "FullName"; // Устанавливаем отображаемое свойство
        }

        /// <summary>
        /// Метод для удаления клиента из коллеции UserControls и
        /// возвращения его из комбобокса
        /// </summary>
        public void DeleteClientInItemsView(Client client)
        {
            clientCollection.RemoveClient(client); // удаляем из itemSource ItemControl 
            Clients.Add(client); // добавляем в кмобобокс 
            FillClients();
            FilterText = string.Empty;
            // Обновление не требуется, поскольку ObservableCollection автоматически обновляет представление
        }

        /// <summary>
        /// Проверка на условия
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        private bool DataValidate(Client client)
        {
            dbContext = new Avtoservice3cursAaContext();

            if (client.CarList.Count >= 3)
            {
                MessageBox.Show("У данного пользователя уже есть 3 автомобиля." +
                    "\nУдалите автомобиль у выбранного клиента или выберите другого клиента",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }

        public List<Client> ReturnClients()
        {
            return clientCollection._clientList;
        }
        #endregion

        #region ОБРАБОТЧИКИ СОБЫТИЙ
        /// <summary>
        /// Обработчик события для того, чтобы отслеживать клиента,
        /// которого выбрал пользователь
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ComboBoxClients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_comboBoxClients.SelectedItem != null)
            {
                if (_listViewItems != null)
                {
                    AddClientInItemsView();
                }
            }
            FilterText = string.Empty;
            _comboBoxClients.SelectedIndex = -1; // Сбросить индекс после выбора
        }

        /// <summary>
        /// Тригер для отслеживания вводимого текста. Обновляем списки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            FilterText = _searchTextBox.Text; // Вызываем метод фильтрации при изменении текста
        }
        #endregion

        // Метод для оповещения участников об изменении
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Событие, которое необходимо объявить при 
        // наследовании интерфейса INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class ClientsManagerForAdd : INotifyPropertyChanged
    {
        private static Avtoservice3cursAaContext dbContext;

        private ObservableCollection<Client> _clients; // исходный список клиентов в комбобокс
        private ObservableCollection<Client> _filteredClients;// отфильтрованный список клиентов в комбобокс
        private TextBox _searchTextBox; // текстбокс внутри комбобокса

        private ClientCollectionForAdd clientCollection; // юзер контролы клиентов для ClientCollection

        // поля для конструктора
        private ItemsControl _listViewItems;
        private ComboBox _comboBoxClients;
        private AddCar _parentWindow;

        // Свойство для доступа к коллекции исходных элементов
        public ObservableCollection<Client> Clients
        {
            get { return _clients; }
            set
            {
                _clients = value;
                OnPropertyChanged(); // Оповещаем участников об изменении
            }
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

        public ClientsManagerForAdd(ItemsControl listViewItems, ComboBox comboBoxClients, AddCar parentWindow)
        {
            _listViewItems = listViewItems;
            _comboBoxClients = comboBoxClients;
            _parentWindow = parentWindow;

            ClientManagerEdit_Load();
        }

        #region МЕТОДЫ ДЛЯ РАБОТЫ С КОЛЛЕКЦИЯМИ И СПИСКАМИ 
        /// <summary>
        /// Настройка параметров для взаимодействия с коллекциями и списками
        /// </summary>
        private void ClientManagerEdit_Load()
        {
            // Обновляем бд и подгружаем свзяи
            dbContext = new();
            dbContext.Cars.Include(c => c.Carclients).Load();

            // инициализируем списки со всеми клиентами и список
            // для будущиъ отфильтрованных клиентов
            var listAllClients = dbContext.Clients.ToList();
            Clients = new ObservableCollection<Client>(listAllClients);
            FilteredClients = new ObservableCollection<Client>(listAllClients);

            // инициализиурем класс ClientCollection для работы с UserContol
            clientCollection = new ClientCollectionForAdd(_parentWindow);
            // в качестве источника ресурсов указыаем нашу коллекицю пользователей
            // которую только что инициализировали
            _listViewItems.ItemsSource = clientCollection.Clients;

            FillClients();

            // чтобы обрабатывать вводимый текст
            _comboBoxClients.ApplyTemplate();
            var textBox = _comboBoxClients.Template.FindName("PART_EditableTextBox", _comboBoxClients) as TextBox;
            _searchTextBox = textBox;

            // подключаем тригеры
            _searchTextBox.TextChanged += SearchTextBox_TextChanged;
            _comboBoxClients.SelectionChanged += ComboBoxClients_SelectionChanged;
        }

        /// <summary>
        /// Обновление списка 
        /// </summary>
        public void FillClients()
        {
            _comboBoxClients.ItemsSource = Clients; // Присваиваем список клиентов
            _comboBoxClients.DisplayMemberPath = "FullName"; // Устанавливаем отображаемое свойство
        }

        /// <summary>
        /// Метод для добавления клиента в коллецию UserControls и
        /// удаления его из комбобокса
        /// </summary>
        public void AddClientInItemsView()
        {
            var selectedClient = _comboBoxClients.SelectedItem as Client; // инициализируем выбранный клиент

            if (selectedClient != null)
            {
                if (DataValidate(selectedClient))
                {
                    clientCollection.AddClient(selectedClient); // добавляем в itemSource ItemControl
                    Clients.Remove(selectedClient); // удаляем его из комбобокса
                };
                FillClients();
            }
        }

        /// <summary>
        /// Метод для фильтрации элементов на основе текста фильтра
        /// </summary>
        private void FilterItems()
        {
            FilteredClients.Clear(); // Очистка коллекции отфильтрованных элементов

            if (string.IsNullOrEmpty(FilterText)) // Если текст фильтра пуст или null
            {
                foreach (var product in Clients) // Добавляем все элементы из исходной коллекции
                {
                    FilteredClients.Add(product);
                }
            }
            else // Если текст фильтра не пустой
            {
                foreach (var product in Clients)
                {
                    // Проверяем, содержится ли текст фильтра в элементе (без учета регистра)
                    if (product.FullName.ToLower().Contains(FilterText.ToLower()))
                    {
                        FilteredClients.Add(product); // Добавляем совпадающий элемент в отфильтрованную коллекцию
                    }
                }
            }

            _comboBoxClients.ItemsSource = FilteredClients; // Обновляем источник данных комбобокса
            _comboBoxClients.DisplayMemberPath = "FullName"; // Устанавливаем отображаемое свойство
        }

        /// <summary>
        /// Метод для удаления клиента из коллеции UserControls и
        /// возвращения его из комбобокса
        /// </summary>
        public void DeleteClientInItemsView(Client client)
        {
            clientCollection.RemoveClient(client); // удаляем из itemSource ItemControl 
            Clients.Add(client); // добавляем в кмобобокс 
            FillClients();
            FilterText = string.Empty;
            // Обновление не требуется, поскольку ObservableCollection автоматически обновляет представление
        }

        /// <summary>
        /// Проверка на условия
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        private bool DataValidate(Client client)
        {
            dbContext = new Avtoservice3cursAaContext();

            if (client.CarList.Count >= 3)
            {
                MessageBox.Show("У данного пользователя уже есть 3 автомобиля." +
                    "\nУдалите автомобиль у выбранного клиента или выберите другого клиента",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }

        public List<Client> ReturnClients()
        {
            return clientCollection._clientList;
        }
        #endregion

        #region ОБРАБОТЧИКИ СОБЫТИЙ
        /// <summary>
        /// Обработчик события для того, чтобы отслеживать клиента,
        /// которого выбрал пользователь
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ComboBoxClients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_comboBoxClients.SelectedItem != null)
            {
                if (_listViewItems != null)
                {
                    AddClientInItemsView();
                }
            }
            FilterText = string.Empty;
            _comboBoxClients.SelectedIndex = -1; // Сбросить индекс после выбора
        }

        /// <summary>
        /// Тригер для отслеживания вводимого текста. Обновляем списки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            FilterText = _searchTextBox.Text; // Вызываем метод фильтрации при изменении текста
        }
        #endregion

        // Метод для оповещения участников об изменении
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Событие, которое необходимо объявить при 
        // наследовании интерфейса INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
