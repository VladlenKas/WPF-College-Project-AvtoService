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

namespace AvtoService_3cursAA.PagesMenuOperator.DataManager
{
    public class ClientManager : INotifyPropertyChanged
    {
        private static Avtoservice3cursAaContext dbContext;
        private ObservableCollection<Client> _clients; // исходный список клиентов в комбобокс
        private ObservableCollection<Client> _filteredClients;// отфильтрованный список клиентов в комбобокс
        private string _filterText; // текст фильтра
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

        // Свойство для хранения текста фильтра
        public string FilterText
        {
            get { return _filterText; }
            set
            {
                _filterText = value;
                OnPropertyChanged(); // Оповещаем участников об изменении
                FilterItems(); // Вызываем метод фильтрации при изменении текста
            }
        }

        public ClientManager(ItemsControl listViewItems, ComboBox comboBoxClients, EditCar parentWindow, Car car)
        {
            dbContext = new();
            dbContext.Cars.Include(c => c.Carclients).Load();

            // Загрузка всех клиентов, кроме тех, которые уже привязаны к машине
            var clientsAlreadyAssigned = car
                .Carclients
                .Select(cc => cc.IdClient)
                .ToList();

            _listViewItems = listViewItems;
            _comboBoxClients = comboBoxClients;
            _parentWindow = parentWindow;
            _car = car;

            var listAllClients = dbContext.Clients.ToList();
            Clients = new ObservableCollection<Client>(listAllClients);
            FilteredClients = new ObservableCollection<Client>(listAllClients);

            clientCollection = new ClientCollection(parentWindow);
            _listViewItems.ItemsSource = clientCollection.Clients;

            // Привязка к свойству Text комбобокса
            Binding binding = new Binding("Text") { Source = _comboBoxClients };
            binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            BindingOperations.EnableCollectionSynchronization(Clients, new object());
            _comboBoxClients.SetBinding(ComboBox.TextProperty, binding);

            // Подключаем обработчики событий
            _comboBoxClients.SelectionChanged += ComboBoxClients_SelectionChanged;

            // Удаляем уже привязанных клиентов с комбобокса и 
            // возвращем их в коллекцию выбранных клиентов
            foreach (var client in dbContext.Clients)
            {
                if (clientsAlreadyAssigned.Contains(client.IdClient))
                {
                    AddClientInDetailView(client);
                }
            }
        }

        public void DeleteClientInDetailView(Client client)
        {
            clientCollection.RemoveClient(client); // удаляем из itemSource ItemControl 
            Clients.Add(client); // добавляем в кмобобокс 
            FillClients();
            // Обновление не требуется, поскольку ObservableCollection автоматически обновляет представление
        }

        private void AddClientInDetailView()
        {
            var selectedClient = _comboBoxClients.SelectedItem as Client; // инициализируем выбранный клиент

            if (selectedClient != null)
            {
                clientCollection.AddClient(selectedClient); // добавляем в itemSource ItemControl
                Clients.Remove(selectedClient); // удаляем его из комбобокса
                FillClients();
            }
        }
        private void AddClientInDetailView(Client client)
        {
            if (client != null)
            {
                clientCollection.AddClient(client); // добавляем в itemSource ItemControl
                Clients.Remove(client); // удаляем его из комбобокса
                FillClients();
            }
        }


        public void ComboBoxClients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_comboBoxClients.SelectedItem != null)
            {
                if (_listViewItems != null)
                {
                    AddClientInDetailView();
                }
            }
            _comboBoxClients.SelectedIndex = -1; // Сбросить индекс после выбора
        }

        private void FillClients()
        {
            _comboBoxClients.ItemsSource = Clients; // Присваиваем список клиентов
            _comboBoxClients.DisplayMemberPath = "FullName"; // Устанавливаем отображаемое свойство
        }

        // Метод для фильтрации элементов на основе текста фильтра
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

        // Обработчик изменения текста в комбобоксе
        public void OnFilterTextChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Text")
            {
                FilterText = _comboBoxClients.Text;
                FilterItems();
            }
        }

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
