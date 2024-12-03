using AvtoService_3cursAA.ActionsForEmployee;
using AvtoService_3cursAA.CustomsElementsWpf;
using AvtoService_3cursAA.DataActions;
using AvtoService_3cursAA.Model;
using Microsoft.EntityFrameworkCore;
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
    /// Логика взаимодействия для EditClient.xaml
    /// </summary>
    public partial class EditClient : Window
    {
        private string Name => NameTextBox.Text;
        private string Firstname => FirstnameTextBox.Text;
        private string Patronymic => PatronymicTextBox.Text;
        private string Birthday => BirthdayTextBox.Text;
        private string Phone => PhoneTextBox.Text;

        Client _selectedClient;
        Avtoservice3cursAaContext dbContext;
        public EditClient(Client selectedClient)
        {
            _selectedClient = selectedClient;
            dbContext = new();

            InitializeComponent();

            NameTextBox.Text = _selectedClient.Name;
            FirstnameTextBox.Text = _selectedClient.Firstname;
            PatronymicTextBox.Text = _selectedClient.Patronymic;
            BirthdayTextBox.Text = _selectedClient.Birthday.ToString().Replace("/", ".");
            PhoneTextBox.Text = _selectedClient.Phone.ToString();
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            // Проверка на изменение данных клиента
            bool isClientDataChanged =
                _selectedClient.Name != Name ||
                _selectedClient.Firstname != Firstname ||
                _selectedClient.Patronymic != Patronymic ||
                _selectedClient.Birthday != DateOnly.ParseExact(Birthday, "dd.MM.yyyy") ||
                _selectedClient.Phone != Phone;

            // Проверка на валидность данных
            if (!DataValidate()) return;

            if (isClientDataChanged)
            {
                // Данные изменились, вызываем метод редактирования
                ActionsData.EditClient(Name, Firstname, Patronymic, Birthday, Phone, _selectedClient);
            }
            else
            {
                // Данные не изменились, продолжаем редактирование 
                var button = MessageBox.Show("Данные не изменились. Продолжить редактирование?.", "Редактирование",
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

            if (new[] { Name, Firstname, Birthday, Phone }.Any(string.IsNullOrWhiteSpace))
            {
                errorsList.Add("Заполните все обязательные поля");
            }

            if (!DateOnly.TryParseExact(Birthday, "dd.MM.yyyy", out DateOnly dateOnly))
            {
                errorsList.Add("Указан неверный формат даты\nПожалуйста, заполните дату в соответствии с форматом в подсказке");
            }
            else
            {
                int age = ActionsTextBox.CalculateAge(dateOnly);
                if (age < 18 || age > 100)
                {
                    errorsList.Add("Возраст должен быть от 18 до 100 лет");
                }
            }

            if (Phone.Replace(" ", "").Length < 11)
            {
                errorsList.Add("Номер телефона должен состоять из 11 цифр");
            }
            if (dbContext.Clients.Any(r => r.Phone == Phone.Replace(" ", "") && r.IdClient != _selectedClient.IdClient))
            {
                errorsList.Add("Выбранный номер телефона уже существует. Пожалуйста, выберите другой");
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
    }
}
