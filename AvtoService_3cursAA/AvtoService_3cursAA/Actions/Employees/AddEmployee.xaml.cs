using AvtoService_3cursAA.ActionsForEmployee;
using AvtoService_3cursAA.DataActions;
using AvtoService_3cursAA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AvtoService_3cursAA.ActionsEmployee
{
    /// <summary>
    /// Логика взаимодействия для AddEmployee.xaml
    /// </summary>
    
    public partial class AddEmployee : Window
    {
        private string Name => NameTextBox.Text;
        private string Firstname => FirstnameTextBox.Text;
        private string Patronymic => PatronymicTextBox.Text;
        private string Birthday => BirthdayTextBox.Text;
        private string Seniority => SeniorityTextBox.Text;
        private string Role => RoleComboBox.Text;
        private string Login => LoginTextBox.Text;
        private string Passport => PassportTextBox.Text.Replace(" ", "");
        private string Phone => PhoneTextBox.Text.Replace(" ", "");
        private string Password => PassVisTextBox.Visibility is Visibility.Visible ? PassVisTextBox.Text : PassHidTextBox.Password;

        private Avtoservice3cursAaContext dbContext;
        private Employee _thisUser;
        public AddEmployee(Employee employee)
        {
            _thisUser = employee;

            InitializeComponent();

            dbContext = new();
            var rolesList = dbContext.Roles.Select(r => r.Name).ToList();
            RoleComboBox.ItemsSource = null;
            RoleComboBox.ItemsSource = rolesList;
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!DataValidate()) return;
            ActionsData.AddEmployee(Name, Firstname, Patronymic, Birthday, Seniority, Role, Passport, Phone, Login, Password);

            this.Close();
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void HidePassCheckBox_Click(object sender, RoutedEventArgs e)
        {
            ActionsTextBox.HiddenPassword(HidePassCheckBox, PassHidTextBox, PassVisTextBox);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            dbContext.Dispose();
        }

        private bool DataValidate()
        {
            dbContext = new Avtoservice3cursAaContext();
            List<string> errorsList = new List<string>();

            if (new[] { Name, Firstname, Birthday, Seniority, Role, Passport, Phone, Login, Password }.Any(string.IsNullOrWhiteSpace))
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
                else if ((age - 18) < int.Parse(Seniority))
                {
                    errorsList.Add("Введены невалидные данные для стажа работы\nПожалуйста, заполните стаж в допустимом промежутке");
                }
            }

            if (Passport.Length < 10)
            {
                errorsList.Add("Номер телефона должен состоять из 10 цифр");
            }
            if (dbContext.Employees.Any(r => r.Passport == Passport))
            {
                errorsList.Add("Выбранный паспорт уже существует. Пожалуйста, выберите другой");
            }

            if (Phone.Length < 11)
            {
                errorsList.Add("Номер телефона должен состоять из 11 цифр");
            }
            if (dbContext.Employees.Any(r => r.Phone == Phone))
            {
                errorsList.Add("Выбранный номер телефона уже существует. Пожалуйста, выберите другой");
            }

            if (dbContext.Employees.Any(r => r.Login == Login))
            {
                errorsList.Add("Пользователь с таким логином уже существует\nПожалуйста, измените логин");
            }

            if (errorsList.Count > 0)
            {
                string errorText = errorsList.First();
                MessageBox.Show(errorText, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

                return false;
            }
            return true;
        }

        private void SeniorityTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            ActionsTextBox.ValidateInputNumbers(e);
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
