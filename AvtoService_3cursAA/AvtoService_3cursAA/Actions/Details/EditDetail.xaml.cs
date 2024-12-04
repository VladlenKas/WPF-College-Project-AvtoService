using AvtoService_3cursAA.ActionsForEmployee;
using AvtoService_3cursAA.DataActions;
using AvtoService_3cursAA.Model;
using System;
using System.Collections.Generic;
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

namespace AvtoService_3cursAA.Actions
{
    /// <summary>
    /// Логика взаимодействия для EditDetail.xaml
    /// </summary>
    public partial class EditDetail : Window
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
        private int Count
        {
            get
            {
                if (int.TryParse(CountTextBox.Text, out int value))
                {
                    return value;
                }
                return 0;
            }
        }
        private ImageSource _imageNull { get; set; }
        private ImageSource? _imageDetailThis;
        private ImageSource Image
        {
            get
            {
                return ImageDetail.Source;
            }
            set
            {
                ImageDetail.Source = value; 
            }
        }

        string _file = "pack://application:,,,/AvtoService_3cursAA;component/Images/NoImageDetail.jpg";
        public Detail _selectedDetailEdit;
        Avtoservice3cursAaContext dbContext;

        public EditDetail(Detail selectedDetail)
        {
            _selectedDetailEdit = selectedDetail;

            dbContext = new();
            InitializeComponent();

            DataContext = _selectedDetailEdit;
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            // Находим эту же деталь для сверки сведений
            var _oldDetail = dbContext.Details.Single(d => d.IdDetail == _selectedDetailEdit.IdDetail);
            // Сравниваем, менялись ли изображения
            bool imageWasChanged = !ActionsData.AreImagesEqual(_imageDetailThis, Image);

            // Проверка на изменение данных детали
            bool isDetailDataChanged =
                _oldDetail.Name != Name ||
                _oldDetail.Cost != Cost ||
                _oldDetail.Count != Count ||
                imageWasChanged; 

            // Проверка на валидность данных
            if (!DataValidate()) return;

            if (isDetailDataChanged)
            {
                // Данные изменились, вызываем метод редактирования
                if (!ActionsData.AreImagesEqual(Image, _imageNull))
                {
                    ActionsData.EditDetail(Name, Cost, Count, Image, _selectedDetailEdit);
                }
                else
                {
                    ActionsData.EditDetail(Name, Cost, Count, null, _selectedDetailEdit);
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

            if (Cost == 0)
            {
                errorsList.Add("Товар не может быть бесплатным");
            }
            if (string.IsNullOrWhiteSpace(Name))
            {
                errorsList.Add("Заполните поле с названием");
            }

            if (dbContext.Details.Any(r => r.Name.Replace(" ", "").ToLower() == Name.Replace(" ", "").ToLower()
                && r.IdDetail != _selectedDetailEdit.IdDetail))
            {
                errorsList.Add("Такая деталь уже существует");
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
            ActionsTextBox.ValidateInputTitle(e);
        }

        private void NumberTextBox_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            ActionsTextBox.ValidatePasteTitle(e);
        }

        private void ImageChange_Click(object sender, RoutedEventArgs e)
        {
            ActionsData.OpenImage(ImageDetail);
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            ImageDetail.Source = new BitmapImage(new Uri(_file, UriKind.Absolute));
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _imageNull = new BitmapImage(new Uri(_file, UriKind.Absolute));

            if (_selectedDetailEdit.Photo == null)
            {
                _imageDetailThis = new BitmapImage(new Uri(_file, UriKind.Absolute));
                ImageDetail.Source = new BitmapImage(new Uri(_file, UriKind.Absolute));
            }
            else
            {
                _imageDetailThis = ImageDetail.Source;
            }
        }
    }
}
