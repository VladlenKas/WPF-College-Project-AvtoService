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

namespace AvtoService_3cursAA.Actions.Prices
{
    /// <summary>
    /// Логика взаимодействия для AddPrice.xaml
    /// </summary>
    public partial class AddPrice : Window
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
        private ImageSource Image => ImagePrice.Source;

        string _file = "pack://application:,,,/AvtoService_3cursAA;component/Images/NoImagePrice.jpg";
        Avtoservice3cursAaContext dbContext;

        public AddPrice()
        {
            dbContext = new();
            InitializeComponent();
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!DataValidate()) return;
            ActionsData.AddPrice(Name, Cost, Image);
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

            if (dbContext.Prices.Any(r => r.Name.Replace(" ", "").ToLower() == Name.Replace(" ", "").ToLower()))
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
    }
}
