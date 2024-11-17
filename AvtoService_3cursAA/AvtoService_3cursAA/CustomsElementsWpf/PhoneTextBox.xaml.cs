using AvtoService_3cursAA.DataActions;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AvtoService_3cursAA.CustomsElementsWpf
{
    /// <summary>
    /// Логика взаимодействия для PhoneTextBox.xaml
    /// </summary>
    public partial class PhoneTextBox : UserControl
    {
        bool _isUpdatingText = false;
        public string Text
        {
            get { return phoneTextBox.Text; }
            set { phoneTextBox.Text = value; }
        }

        public PhoneTextBox()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Удаление лишних символов в TextBox для даты
        /// </summary>
        /// <param name="e"></param>
        /// <param name="dateTextBox"></param>
        private void PhoneTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back || e.Key == Key.Delete)
            {
                string text = phoneTextBox.Text;
                if (text.EndsWith(" "))
                {
                    e.Handled = true; // Предотвращаем стандартное поведение Backspace или Delete

                    phoneTextBox.Text = text.Substring(0, text.Length - 2); // Удалить два символа
                    phoneTextBox.SelectionStart = phoneTextBox.Text.Length;
                }
            }
        }

        /// <summary>
        /// Маска для TextBox с датой
        /// </summary>
        /// <param name="dateTextBox"></param>
        private void PhoneTextBox_TextChanged(object sender, TextChangedEventArgs args)
        {
            phoneTextBox.PreviewTextInput += PhoneTextBox_PreviewTextInput;

            if (_isUpdatingText) return;
            _isUpdatingText = true;

            string text = phoneTextBox.Text.Replace(" ", "");

            if (!text.StartsWith("8"))
            {
                text = "8" + text;
            }

            if (text.Length > 0)
            {
                // Формат: 8 999
                if (text.Length <= 3)
                {
                    phoneTextBox.Text = $"{text.Substring(0, 1)} {text.Substring(1)}";
                }
                // Формат: 8 999 111
                else if (text.Length <= 6)
                {
                    phoneTextBox.Text = $"{text.Substring(0, 1)} {text.Substring(1, 3)} {text.Substring(4)}";
                }
                // Формат: 8 999 111 22
                else if (text.Length <= 8)
                {
                    phoneTextBox.Text = $"{text.Substring(0, 1)} {text.Substring(1, 3)} {text.Substring(4, 3)} {text.Substring(7)}";
                }
                // Формат: 8 999 111 22 33
                else if (text.Length <= 10)
                {
                    phoneTextBox.Text = $"{text.Substring(0, 1)} {text.Substring(1, 3)} {text.Substring(4, 3)} {text.Substring(7, 2)} {text.Substring(9)}";
                }
                phoneTextBox.SelectionStart = phoneTextBox.Text.Length;

            }

            _isUpdatingText = false;
        }

        /// <summary>
        /// Позволяет вводить только числа
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PhoneTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            ActionsTextBox.ValidateInputNumbers(e);
        }

        private void PhoneTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (phoneTextBox.Text == string.Empty)
                phoneTextBox.Text = "8";
        }

        private void PhoneTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (phoneTextBox.Text == "8")
                phoneTextBox.Text = string.Empty;
        }

        private void phoneTextBox_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            ActionsTextBox.ValidatePasteNumbers(e);
        }
    }
}
