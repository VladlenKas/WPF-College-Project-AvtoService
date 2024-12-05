using AvtoService_3cursAA.Model;
using AvtoService_3cursAA.PagesMenuAdmin.Collections;
using AvtoService_3cursAA.UserControls.CheckUC;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using static MaterialDesignThemes.Wpf.Theme.ToolBar;

namespace AvtoService_3cursAA.PagesMenuAdmin.DataManagers
{
    internal class DetailManager : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged; // реализуем интерфейс
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private static Avtoservice3cursAaContext dbContext;

        private ObservableCollection<Detail> _details; // исходный список деталей в комбобокс
        private ObservableCollection<Detail> _filteredDetails; // отфильтрованный список услуг в комбобокс
        private TextBox _searchTextBox; // текстбокс внутри комбобокса

        private DetailCollection DetailCollection; // коллеция с добавленными юзерконтролами 

        // поля для констуктора
        private ItemsControl _listViewItems;
        private ComboBox _comboBoxDetails;
        private TextBlock _costDetails;
        internal TextBlock _placeHolder;
        private CheckAdmin _parentWindow;
        internal int costDetail = 0;

        // Свойство для доступа к коллекции исходных элементов
        public ObservableCollection<Detail> Details
        {
            get { return _details; }
            set
            {
                _details = value;
                OnPropertyChanged(); // Оповещаем участников об изменении
            }
        }
        // Свойство для доступа к отфильтрованным элементам
        public ObservableCollection<Detail> FilteredDetails
        {
            get { return _filteredDetails; }
            set
            {
                _filteredDetails = value;
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


        public DetailManager(ItemsControl listViewItems, ComboBox comboBoxDetails, TextBlock costDetails, 
            TextBlock placeHolder, CheckAdmin parentWindow)
        {
            dbContext = new();

            _listViewItems = listViewItems;
            _comboBoxDetails = comboBoxDetails;
            _costDetails = costDetails;
            _placeHolder = placeHolder;
            _parentWindow = parentWindow;

            DetailManagerLoad();
        }

        #region МЕТОДЫ ДЛЯ РАБОТЫ С КОЛЛЕКЦИЯМИ И СПИСКАМИ
        private void DetailManagerLoad()
        {
            // инициализируем списки со всеми элементами и список
            var listAllPrices = dbContext.Details.Where(d => d.Count != 0).ToList();
            Details = new ObservableCollection<Detail>(listAllPrices);
            FilteredDetails = new ObservableCollection<Detail>(listAllPrices);

            // инициализиурем класс ClientCollection для работы с UserContol
            DetailCollection = new DetailCollection(_parentWindow);
            // в качестве источника ресурсов указыаем нашу коллекицию пользователей
            // которую только что инициализировали
            _listViewItems.ItemsSource = DetailCollection.Details;

            // обновление комбобокса
            FillDetails();

            // чтобы обрабатывать вводимый текст
            _comboBoxDetails.ApplyTemplate();
            var textBox = _comboBoxDetails.Template.FindName("PART_EditableTextBox", _comboBoxDetails) as TextBox;
            _searchTextBox = textBox;

            // подключаем тригеры
            _searchTextBox.TextChanged += SearchTextBox_TextChanged;
            _comboBoxDetails.SelectionChanged += ComboBoxDetails_SelectionChanged;
        }

        public void DeleteDetailInDetailView(Detail detail)
        {
            DetailCollection.RemoveDetail(detail); // удаляем из itemSource ItemControl 
            Details.Add(detail); // добавляем в кмобобокс 
            FillDetails();
            FilterText = string.Empty;
            // Обновление не требуется, поскольку ObservableCollection автоматически обновляет представление
        }

        private void AddDetailnDetailView()
        {
            var selectedDetail = _comboBoxDetails.SelectedItem as Detail; // инициализируем выбранный клиент

            if (selectedDetail != null)
            {
                DetailCollection.AddDetail(selectedDetail); // добавляем в itemSource ItemControl
                Details.Remove(selectedDetail); // удаляем его из комбобокса
                FillDetails();
            }
        }

        private void FilterItems()
        {
            FilteredDetails.Clear(); // Очистка коллекции отфильтрованных элементов

            if (string.IsNullOrEmpty(FilterText)) // Если текст фильтра пуст или null
            {
                foreach (var product in Details) // Добавляем все элементы из исходной коллекции
                {
                    FilteredDetails.Add(product);
                }
            }
            else // Если текст фильтра не пустой
            {
                foreach (var product in Details)
                {
                    // Проверяем, содержится ли текст фильтра в элементе (без учета регистра)
                    if (product.Name.ToLower().Contains(FilterText.ToLower()))
                    {
                        FilteredDetails.Add(product); // Добавляем совпадающий элемент в отфильтрованную коллекцию
                    }
                }
            }

            if (FilteredDetails.Count != 0)
            {
                _comboBoxDetails.ItemsSource = FilteredDetails; // Обновляем источник данных комбобокса
                _comboBoxDetails.DisplayMemberPath = "Name"; // Устанавливаем отображаемое свойство 
            }
            else
            {
                var emptyMessage = new { Name = "Ничего не найдено" };
                _comboBoxDetails.ItemsSource = new List<object> { emptyMessage };
            }
        }

        public void LoadDetailInDetailView(Detail detail)
        {
            FillDetails();
        }

        internal List<(int IdDetail, int Count)> GetDetails()
        {
            List <(int IdDetail, int Count)> list = new();
            foreach (var item in DetailCollection.Details)
            {
                list.Add((item.IdDetail, item.Count));
            }

            return list;
        }

        internal List<(int Count, string Name, int Cost)> GetDetailsForFile()
        {
            List<(int Count, string Name, int Cost)> list = new();
            foreach (var item in DetailCollection.Details)
            {
                list.Add((item.Count, item.Name, item.Cost));
            }

            return list;
        }

        private ObservableCollection<DetailItem> ReturnDetails()
        {
            return DetailCollection.Details;
        }

        // Очистка всех выбранных деталей
        public void ClearListView()
        {
            List<Detail> details = new List<Detail>(DetailCollection._detailList);
            if (details.Count == 0) return;

            foreach (var detail in details)
            {
                DetailCollection.RemoveDetail(detail);
                Details.Add(detail); // добавляем в комбобокс 
            }
            FillDetails();
        }
        #endregion

        #region ОБРАБОТЧИКИ СОБЫТИЙ
        /// <summary>
        /// Обработчик события для того, чтобы отслеживать элемент,
        /// который выбрал пользователь
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ComboBoxDetails_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_comboBoxDetails.SelectedItem != null)
            {
                if (_listViewItems != null)
                {
                    AddDetailnDetailView();
                }
            }
            FilterText = string.Empty;
            _comboBoxDetails.SelectedIndex = -1; // Сбросить индекс после выбора
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

            _comboBoxDetails.IsDropDownOpen = true; // раскрываем комбобокс при вводе
        }
        #endregion

        internal void FillDetails()
        {
            _comboBoxDetails.ItemsSource = Details; // Обновляем источник данных комбобокса
            _comboBoxDetails.DisplayMemberPath = "Name"; // Устанавливаем отображаемое свойство

            var selectedPrices = ReturnDetails();
            int cost = 0;

            if (selectedPrices != null)
            {
                foreach (var item in selectedPrices)
                {
                    if (item != null)
                        cost += item.Cost;
                }
            }
            _costDetails.Text = null;
            _costDetails.Text += $" {cost} руб.";

            costDetail = 0;
            costDetail += cost;
            _parentWindow.UpdateFinalCost();
            _parentWindow.VisibilityButtonAdd();
        }
    }
}
