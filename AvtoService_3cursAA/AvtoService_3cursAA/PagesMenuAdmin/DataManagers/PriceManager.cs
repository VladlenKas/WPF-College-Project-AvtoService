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
    internal class PriceManager : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged; // реализуем интерфейс
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private static Avtoservice3cursAaContext dbContext;

        private ObservableCollection<Price> _prices; // исходный список услуг в комбобокс
        private ObservableCollection<Price> _filteredPrices; // отфильтрованный список услуг в комбобокс
        private TextBox _searchTextBox; // текстбокс внутри комбобокса

        private PriceCollection PriceCollection; // юзер контролы с услугами

        // поля для конструктора
        private ItemsControl _listViewItems;
        private ComboBox _comboBoxPrices;
        internal TextBlock _costPrices;
        internal TextBlock _placeHolder;
        private CheckAdmin _parentWindow;

        // Свойство для доступа к коллекции исходных элементов
        public ObservableCollection<Price> Prices
        {
            get { return _prices; }
            set
            {
                _prices = value;
                OnPropertyChanged(); // Оповещаем участников об изменении
            }
        }

        // Свойство для доступа к отфильтрованным элементам
        public ObservableCollection<Price> FilteredPrices
        {
            get { return _filteredPrices; }
            set
            {
                _filteredPrices = value;
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

        internal int costPrice = 0;
        public PriceManager(ItemsControl listViewItems, ComboBox comboBoxPrices, TextBlock costPrices, TextBlock placeHolder, CheckAdmin parentWindow)
        {
            dbContext = new();

            _listViewItems = listViewItems;
            _comboBoxPrices = comboBoxPrices;
            _costPrices = costPrices;
            _placeHolder = placeHolder;
            _parentWindow = parentWindow;

            PriceManagerLoad();
        }

        #region МЕТОДЫ ДЛЯ РАБОТЫ С КОЛЛЕКЦИЯМИ И СПИСКАМИ
        private void PriceManagerLoad()
        {
            // инициализируем списки со всеми элементами и список
            var listAllPrices = dbContext.Prices.ToList();
            Prices = new ObservableCollection<Price>(listAllPrices);
            FilteredPrices = new ObservableCollection<Price>(listAllPrices);

            // инициализиурем класс ClientCollection для работы с UserContol
            PriceCollection = new PriceCollection(_parentWindow);
            // в качестве источника ресурсов указыаем нашу коллекицю пользователей
            // которую только что инициализировали
            _listViewItems.ItemsSource = PriceCollection.Prices;

            // обновление комбобокса
            FillPrices();

            // чтобы обрабатывать вводимый текст
            _comboBoxPrices.ApplyTemplate();
            var textBox = _comboBoxPrices.Template.FindName("PART_EditableTextBox", _comboBoxPrices) as TextBox;
            _searchTextBox = textBox;

            // подключаем тригеры
            _searchTextBox.TextChanged += SearchTextBox_TextChanged;
            _comboBoxPrices.SelectionChanged += ComboBoxPrices_SelectionChanged;
        }

        public void DeletePriceInPriceView(Price price)
        {
            PriceCollection.RemovePrice(price); // удаляем из itemSource ItemControl 
            Prices.Add(price); // добавляем в кмобобокс 
            FillPrices();
            FilterText = string.Empty;
            // Обновление не требуется, поскольку ObservableCollection автоматически обновляет представление
        }

        private void AddPriceInPriceView()
        {
            var selectedPrice = _comboBoxPrices.SelectedItem as Price; // инициализируем выбранный клиент

            if (selectedPrice != null)
            {
                PriceCollection.AddPrice(selectedPrice); // добавляем в itemSource ItemControl
                Prices.Remove(selectedPrice); // удаляем его из комбобокса
                FillPrices();
            }
        }

        private void FilterItems()
        {
            FilteredPrices.Clear(); // Очистка коллекции отфильтрованных элементов

            if (string.IsNullOrEmpty(FilterText)) // Если текст фильтра пуст или null
            {
                foreach (var product in Prices) // Добавляем все элементы из исходной коллекции
                {
                    FilteredPrices.Add(product);
                }
            }
            else // Если текст фильтра не пустой
            {
                foreach (var product in Prices)
                {
                    // Проверяем, содержится ли текст фильтра в элементе (без учета регистра)
                    if (product.Name.ToLower().Contains(FilterText.ToLower()))
                    {
                        FilteredPrices.Add(product); // Добавляем совпадающий элемент в отфильтрованную коллекцию
                    }
                }
            }

            _comboBoxPrices.ItemsSource = FilteredPrices; // Обновляем источник данных комбобокса
            _comboBoxPrices.DisplayMemberPath = "Name"; // Устанавливаем отображаемое свойство
        }

        public List<Price> ReturnPrices()
        {
            return PriceCollection._pricesList;
        }

        // Очистка всех выбранных услуг
        public void ClearListView()
        {
            List<Price> prices = new List<Price>(PriceCollection._pricesList);
            if (prices.Count == 0) return;

            foreach (var price in prices)
            {
                PriceCollection.RemovePrice(price);
                Prices.Add(price); // добавляем в комбобокс 
            }

            FillPrices();
        }
        #endregion

        #region ОБРАБОТЧИКИ СОБЫТИЙ
        /// <summary>
        /// Обработчик события для того, чтобы отслеживать элемент,
        /// который выбрал пользователь
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ComboBoxPrices_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_comboBoxPrices.SelectedItem != null)
            {
                if (_listViewItems != null)
                {
                    AddPriceInPriceView();
                }
            }
            FilterText = string.Empty;
            _comboBoxPrices.SelectedIndex = -1; // Сбросить индекс после выбора
        }

        /// <summary>
        /// Тригер для отслеживания вводимого текста. Обновляем списки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _placeHolder.Visibility = string.IsNullOrWhiteSpace(_searchTextBox.Text) ? Visibility.Visible : Visibility.Hidden;
            FilterText = _searchTextBox.Text; // Вызываем метод фильтрации при изменении текста
        }
        #endregion

        private void FillPrices()
        {
            _comboBoxPrices.ItemsSource = Prices; // Присваиваем список 
            _comboBoxPrices.DisplayMemberPath = "Name"; // Устанавливаем отображаемое свойство

            var selectedPrices = ReturnPrices();
            int cost = 0;

            if (selectedPrices != null)
            {
                foreach (var item in selectedPrices)
                {
                    if (item != null)
                        cost += item.Cost;
                }
            }

            _costPrices.Text = null;
            _costPrices.Text += $" {cost} руб.";

            costPrice = 0;
            costPrice += cost;
            _parentWindow.UpdateFinalCost();
            _parentWindow.VisibilityButtonAdd();
        }
    }
}
