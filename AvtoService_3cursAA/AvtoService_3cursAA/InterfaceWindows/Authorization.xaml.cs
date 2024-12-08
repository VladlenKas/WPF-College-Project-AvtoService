using Microsoft.EntityFrameworkCore;
using AvtoService_3cursAA.DataActions;
using AvtoService_3cursAA.Model;
using System.Security.AccessControl;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AvtoService_3cursAA.CustomsElementsWpf;
using AvtoService_3cursAA.InterfaceWindows;

namespace AvtoService_3cursAA
{
    /// <summary>
    /// Interaction logic for Authorization.xaml
    /// </summary>
    public partial class Authorization : Window
    {
        private string Login => TextBoxEmail.Text;
        private string Password => TextBoxPassVisibility.Visibility is Visibility.Visible ? TextBoxPassVisibility.Text : TextBoxPassHidden.Password;

        private Avtoservice3cursAaContext dbContext;
        public Authorization()
        {
            InitializeComponent();
        }
        private void ButtonLogin_Click(object sender, RoutedEventArgs e)
        {
            var employee = CheckUser();
            if (employee is null) return;

            switch (employee.IdRole)
            {
                case 1:
                    MenuAdmin menuAdmin = new MenuAdmin(employee);
                    menuAdmin.Show();
                    this.Close();
                    break;
                case 2:
                    MenuMechanic menuMechanic = new MenuMechanic(employee);
                    menuMechanic.Show();
                    this.Close();
                    break;
                case 3:
                    MenuOperator menuOperator = new MenuOperator(employee);
                    menuOperator.Show();
                    this.Close();
                    break;
                default:
                    break;
            }
        }

        private Employee? CheckUser()
        {
            dbContext = new();
            List<string> errorsList = new();

            if (Login == string.Empty && Password == string.Empty)
            {
                errorsList.Add("Заполните все пустые поля!");
            }
            else if (Login == string.Empty)
            {
                errorsList.Add("Заполните поле с логином!!");
            }
            else if (Password == string.Empty)
            {
                errorsList.Add("Заполните поле с паролем!");
            }

            if (errorsList.Count == 0)
            {
                var employee = dbContext.Employees.Include(e => e.IdRoleNavigation).ToList().
                Find(r => r.Login == Login && (r.Password == Password));

                if (employee != null)
                {
                    MessageBox.Show($"Добро пожаловать в систему, {employee.FullName}!" + 
                        $"\nВы вошли как {employee.IdRoleNavigation.Name}", 
                        "Авторизация прошла успешно", MessageBoxButton.OK, MessageBoxImage.Information);

                    return employee;
                }
                else
                {
                    errorsList.Add("Данного пользователя не существует!");
                }
            }

            string errorText = errorsList.First();
            MessageBox.Show($"{errorText}", "Ошибка авторизации", MessageBoxButton.OK, MessageBoxImage.Error);
            return null;
        }

        private void ChechBoxPassView_Click(object sender, RoutedEventArgs e)
        {
            ActionsTextBox.HiddenPassword(ChechBoxPassView, TextBoxPassHidden, TextBoxPassVisibility);
        }
        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}