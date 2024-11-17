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
        private string Cost => CosttextBox.Text;
        private ImageSource Image => ImagePrice.Source;

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
            if (!DataValidate()) return;
            ActionsUsers.EditPrice(Name, Cost, Image, _selectedPriceEdit);
            this.Close();
        }


        private bool DataValidate()
        {
            dbContext = new();
            List<string> errorsList = new();

            if (new[] { Name, Cost }.Any(string.IsNullOrWhiteSpace))
            {
                errorsList.Add("Заполните все обязательные поля");
            }

            if (dbContext.Prices.Any(r => r.Name.Replace(" ", "").ToLower() == Name.Replace(" ", "").ToLower()
            && r.IdPrice != _selectedPriceEdit.IdPrice))
            {
                errorsList.Add("Такая услуга уже существует");
            }

            if (int.Parse(Cost) < 1000 || int.Parse(Cost) > 9999)
            {
                errorsList.Add("Цена должна быть в диапозоне 1000 руб. - 9999 руб.");
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
            ActionsUsers.OpenImage(ImagePrice);
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            ImagePrice.Source = new BitmapImage(new Uri(_file, UriKind.Absolute));
        }
    }
}
