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
    internal class ClientsManager
    {
        public event PropertyChangedEventHandler? PropertyChanged; // реализуем интерфейс
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private static Avtoservice3cursAaContext dbContext;

        private ObservableCollection<Client> _clients; // исходный список услуг в комбобокс
        private ObservableCollection<Client> _filteredClients; // отфильтрованный список услуг в комбобокс
        private TextBox _searchTextBox; // текстбокс внутри комбобокса

        private PriceCollection PriceCollection; // юзер контролы с услугами

        // поля для конструктора
        private ItemsControl _listViewItems;
        private ComboBox _comboBoxClients;
        internal TextBlock _placeHolder;
        private CheckAdmin _parentWindow;

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
                //_comboBoxClients.SelectionChanged -= ComboBoxClients_SelectionChanged;
                _searchTextBox.Text = value;
                OnPropertyChanged(); // Оповещаем участников об изменении
                FilterItems(); // Вызываем метод фильтрации при изменении текста
                //_comboBoxClients.SelectionChanged += ComboBoxClients_SelectionChanged;

            }
        }

        public ClientsManager(ComboBox comboBoxClients, TextBlock placeHolder, CheckAdmin parentWindow)
        {
            dbContext = new();

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
            _clients = new ObservableCollection<Client>(dbContext.Clients);
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
                if (_comboBoxClients.SelectedValue == null) return;

                _placeHolder.Visibility = Visibility.Hidden;

                _searchTextBox.TextChanged -= SearchTextBox_TextChanged;
                _searchTextBox.Text = string.Empty;
                _searchTextBox.TextChanged += SearchTextBox_TextChanged;
            }
        }
        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_comboBoxClients == null) return;

            _placeHolder.Visibility = string.IsNullOrWhiteSpace(_searchTextBox.Text) ? Visibility.Visible : Visibility.Hidden;
            FilterText = _searchTextBox.Text; // Вызываем метод фильтрации при изменении текста
        }
        #endregion
    }
}
