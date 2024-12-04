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

namespace AvtoService_3cursAA.Actions.Details
{
    /// <summary>
    /// Логика взаимодействия для AddDetail.xaml
    /// </summary>
    public partial class AddDetail : Window
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
        private ImageSource Image => ImageDetail.Source;

        string _file = "pack://application:,,,/AvtoService_3cursAA;component/Images/NoImageDetail.jpg";
        Avtoservice3cursAaContext dbContext;

        public AddDetail()
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
            ActionsData.AddDetail(Name, Cost, Count, Image);
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

            if (dbContext.Details.Any(r => r.Name.Replace(" ", "").ToLower() == Name.Replace(" ", "").ToLower()))
            {
                errorsList.Add("Такая деталь уже существует, измените наименование");
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
    }
}
