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
        private ImageSource Image => ImageDetail.Source;

        string _file = "pack://application:,,,/AvtoService_3cursAA;component/Images/NoImageDetail.jpg";
        public Detail _selectedDetailEdit;
        Avtoservice3cursAaContext dbContext;

        public EditDetail(Detail selectedDetail)
        {
            _selectedDetailEdit = selectedDetail;

            dbContext = new();
            InitializeComponent();

            DataContext = _selectedDetailEdit;

            if (_selectedDetailEdit.Photo == null)
            {
                ImageDetail.Source = new BitmapImage(new Uri(_file, UriKind.Absolute));
            }
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!DataValidate()) return;
            ActionsUsers.EditDetail(Name, Cost, Count, Image, _selectedDetailEdit);
            this.Close();
        }

        private bool DataValidate()
        {
            dbContext = new();
            List<string> errorsList = new();

            if (string.IsNullOrWhiteSpace(Name) || Cost == 0 || Count == 0)
            {
                errorsList.Add("Заполните все обязательные поля");
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
            ActionsTextBox.ValidateInputNumbers(e);
        }

        private void NumberTextBox_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            ActionsTextBox.ValidatePasteNumbers(e);
        }

        private void ImageChange_Click(object sender, RoutedEventArgs e)
        {
            ActionsUsers.OpenImage(ImageDetail);
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            ImageDetail.Source = new BitmapImage(new Uri(_file, UriKind.Absolute));
        }
    }
}
