using AvtoService_3cursAA.ActionsForEmployee;
using AvtoService_3cursAA.CustomsElementsWpf;
using AvtoService_3cursAA.DataActions;
using AvtoService_3cursAA.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace AvtoService_3cursAA.Actions
{
    /// <summary>
    /// Логика взаимодействия для EditPrice.xaml
    /// </summary>
    public partial class EditPrice : Window
    {
        private string Name => NameTextBox.Text;
        private int Cost
        {
            get
            {
                if (int.TryParse(CostTextBox.Text, out int value))
                {
                    return value;
                }
                return 0;
            }
        }
        private ImageSource _imageNull { get; set; }
        private ImageSource? _imagePriceThis;
        private ImageSource Image
        {
            get
            {
                return ImagePrice.Source;
            }
            set
            {
                ImagePrice.Source = value;
            }
        }

        string _file = "pack://application:,,,/AvtoService_3cursAA;component/Images/NoImagePrice.jpg";
        public Price _selectedPriceEdit;
        Avtoservice3cursAaContext dbContext;

        public EditPrice(Price selectedPrice)
        {
            _selectedPriceEdit = selectedPrice; 

            dbContext = new();
            InitializeComponent();

            DataContext = _selectedPriceEdit;

            if (_selectedPriceEdit.Photo == null)
            {
                ImagePrice.Source = new BitmapImage(new Uri(_file, UriKind.Absolute));
            }
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            // Находим товар для сверки сведений
            var _oldProduct = dbContext.Prices.Single(p => p.IdPrice == _selectedPriceEdit.IdPrice);

            // Сравниваем, менялись ли изображения
            bool productImageWasChanged = !ActionsData.AreImagesEqual(_imagePriceThis, Image); // Предполагается, что у вас есть поле Image в модели Product

            // Проверка на изменение данных товара
            bool isProductDataChanged =
                _oldProduct.Name != Name ||
                _oldProduct.Cost != Cost ||
                productImageWasChanged;

            // Проверка на валидность данных
            if (!DataValidate()) return;

            if (isProductDataChanged)
            {
                // Данные изменились, вызываем метод редактирования
                if (!ActionsData.AreImagesEqual(Image, _imageNull)) // Предполагается, что _imageNull определен как изображение по умолчанию
                {
                    ActionsData.EditPrice(Name, Cost, Image, _selectedPriceEdit);
                }
                else
                {
                    ActionsData.EditPrice(Name, Cost, null, _selectedPriceEdit);
                }
            }
            else
            {
                // Данные не изменились, продолжаем редактирование 
                var button = MessageBox.Show("Данные не изменились. Продолжить редактирование?", "Редактирование",
                    MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (button == MessageBoxResult.Yes)
                {
                    return;
                }
            }

            this.Close();
        }

        private bool DataValidate()
        {
            dbContext = new();
            List<string> errorsList = new();

            if (string.IsNullOrWhiteSpace(Name) || Cost == 0)
            {
                errorsList.Add("Заполните все обязательные поля");
            }

            if (dbContext.Prices.Any(r => r.Name.Replace(" ", "").ToLower() == Name.Replace(" ", "").ToLower()
            && r.IdPrice != _selectedPriceEdit.IdPrice))
            {
                errorsList.Add("Такая услуга уже существует");
            }

            if (errorsList.Count > 0)
            {
                string errorText = errorsList.First();
                MessageBox.Show(errorText, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

                return false;
            }
            return true;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            dbContext.Dispose();
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            ActionsTextBox.ValidateInputCyrillic(e);
        }

        private void TextBox_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            ActionsTextBox.ValidatePasteCyrillic(e);
        }

        private void NumberTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            ActionsTextBox.ValidateInputNumbers(e);
        }

        private void NumberTextBox_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            ActionsTextBox.ValidatePasteNumbers(e);
        }

        private void ImageChange_Click(object sender, RoutedEventArgs e)
        {
            ActionsData.OpenImage(ImagePrice);
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            ImagePrice.Source = new BitmapImage(new Uri(_file, UriKind.Absolute));
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _imageNull = new BitmapImage(new Uri(_file, UriKind.Absolute));

            if (_selectedPriceEdit.Photo == null)
            {
                _imagePriceThis = new BitmapImage(new Uri(_file, UriKind.Absolute));
                ImagePrice.Source = new BitmapImage(new Uri(_file, UriKind.Absolute));
            }
            else
            {
                _imagePriceThis = ImagePrice.Source;
            }
        }
    }
}
