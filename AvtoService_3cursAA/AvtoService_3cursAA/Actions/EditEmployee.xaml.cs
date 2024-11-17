using Microsoft.EntityFrameworkCore;
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
using AvtoService_3cursAA.CustomsElementsWpf;

namespace AvtoService_3cursAA.ActionsEmployee
{
    /// <summary>
    /// Логика взаимодействия для EditEmployee.xaml
    /// </summary>
    public partial class EditEmployee : Window
    {
        private string Name => NameTextBox.Text;
        private string Firstname => FirstnameTextBox.Text;
        private string Patronymic => PatronymicTextBox.Text;
        private string Birthday => BirthdayTextBox.Text;
        private string Seniority => SeniorityTextBox.Text;
        private string Role => RoleComboBox.Text;
        private string Passport => PassportTextBox.Text.Replace(" ", "");
        private string Phone => PhoneTextBox.Text.Replace(" ", "");
        private string Login => LoginTextBox.Text;
        private string Password => PassVisTextBox.Visibility is Visibility.Visible ? PassVisTextBox.Text : PassHidTextBox.Password;

        private Avtoservice3cursAaContext dbContext;
        private Employee _selectedEmployee;
        private Employee _thisEmployee;

        public EditEmployee(Employee selectedEmployee, Employee thisEmployee)
        {
            _selectedEmployee = selectedEmployee;
            _thisEmployee = thisEmployee;

            InitializeComponent();
            dbContext = new();

            if (_selectedEmployee.IdEmployee == _thisEmployee.IdEmployee)
            {
                LoginTextBox.IsHitTestVisible = false;
                LoginTextBox.Foreground = new SolidColorBrush(Colors.Gray);
                RoleComboBox.IsHitTestVisible = false;
                RoleComboBox.Foreground = new SolidColorBrush(Colors.Gray);
            }

            NameTextBox.Text = _selectedEmployee.Name;
            FirstnameTextBox.Text = _selectedEmployee.Firstname;
            PatronymicTextBox.Text = _selectedEmployee.Patronymic;
            BirthdayTextBox.Text = _selectedEmployee.Birthday.ToString().Replace("/", ".");
            SeniorityTextBox.Text = _selectedEmployee.Seniority.ToString();
            PassportTextBox.Text = _selectedEmployee.Passport;
            PhoneTextBox.Text = _selectedEmployee.Phone;
            RoleComboBox.SelectedItem = _selectedEmployee.IdRoleNavigation.Name;
            LoginTextBox.Text = _selectedEmployee.Login;
            PassHidTextBox.Password = _selectedEmployee.Password;

            var rolesList = dbContext.Roles.Select(r => r.Name).ToList();
            RoleComboBox.ItemsSource = null;
            RoleComboBox.ItemsSource = rolesList;
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            if (!DataValidate()) return;
            ActionsUsers.EditEmployee(Name, Firstname, Patronymic, Birthday, Seniority, Role, Passport, Phone, Login, Password, _selectedEmployee);

            this.Close();
        }

        private bool DataValidate()
        {
            dbContext = new Avtoservice3cursAaContext();
            List<string> errorsList = new List<string>();

            if (new[] { Name, Firstname, Birthday, Seniority, Role, Login, Password }.Any(string.IsNullOrWhiteSpace))
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
                errorsList.Add("Паспорт должен состоять из 10 цифр");
            }
            if (dbContext.Employees.Any(r => r.Passport == Passport && r.IdEmployee != _selectedEmployee.IdEmployee))
            {
                errorsList.Add("Выбранный номер телефона уже существует. Пожалуйста, выберите другой");
            }

            if (Phone.Length < 11)
            {
                errorsList.Add("Номер телефона должен состоять из 11 цифр");
            }
            if (dbContext.Employees.Any(r => r.Phone == Phone && r.IdEmployee != _selectedEmployee.IdEmployee))
            {
                errorsList.Add("Выбранный номер телефона уже существует. Пожалуйста, выберите другой");
            }

            if (dbContext.Employees.Any(r => r.Login == Login && r.IdEmployee != _selectedEmployee.IdEmployee))
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
