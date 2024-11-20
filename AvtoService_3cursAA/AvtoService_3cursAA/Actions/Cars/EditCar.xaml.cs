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

namespace AvtoService_3cursAA.Actions.Cars
{
    /// <summary>
    /// Логика взаимодействия для EditCar.xaml
    /// </summary>
    public partial class EditCar : Window
    {
        private string Brand => BrandTextBox.Text;
        private string Model => ModelTextBox.Text;
        private string Country => CountryTextBox.Text;
        private short Year
        {
            get
            {
                if (short.TryParse(YearTextBox.Text, out short value))
                {
                    return value;
                }
                return 0;
            }
        }
        private string Description => DescriptionTextBox.Text;
        private ImageSource Image => ImageCar.Source;

        string _file = "pack://application:,,,/AvtoService_3cursAA;component/Images/NoImageCar.jpg";
        public Car _selectedCarEdit;
        Avtoservice3cursAaContext dbContext;

        public EditCar(Car selectedCar)
        {
            _selectedCarEdit = selectedCar;

            dbContext = new();
            InitializeComponent();

            DataContext = _selectedCarEdit;

            if (_selectedCarEdit.Photo == null)
            {
                ImageCar.Source = new BitmapImage(new Uri(_file, UriKind.Absolute));
            }
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!DataValidate()) return;
            ActionsData.EditCar(Brand, Model, Country, Year, Description, Image, _selectedCarEdit);
            this.Close();
        }

        private bool DataValidate()
        {
            dbContext = new();
            List<string> errorsList = new();

            if (string.IsNullOrWhiteSpace(Brand) || string.IsNullOrWhiteSpace(Model) || string.IsNullOrWhiteSpace(Country) || Year == 0)
            {
                errorsList.Add("Заполните все обязательные поля");
            }

            if (dbContext.Cars.Any(r => r.Brand.Replace(" ", "").ToLower() == Brand.Replace(" ", "").ToLower()
                && r.Model.Replace(" ", "").ToLower() == Model.Replace(" ", "").ToLower()
                && r.IdCar != _selectedCarEdit.IdCar))
            {
                errorsList.Add("Такая машина уже существует");
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
            ActionsData.OpenImage(ImageCar);
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            ImageCar.Source = new BitmapImage(new Uri(_file, UriKind.Absolute));
        }

        private void DescriptionTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            ActionsTextBox.ValidateInputDescription(e);
        }

        private void DescriptionTextBox_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            ActionsTextBox.ValidatePasteDescription(e);
        }
    }
}
