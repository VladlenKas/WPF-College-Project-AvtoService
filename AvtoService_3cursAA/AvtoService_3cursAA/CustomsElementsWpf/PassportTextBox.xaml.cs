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
    /// Логика взаимодействия для PassportTextBox.xaml
    /// </summary>
    public partial class PassportTextBox : UserControl
    {
        bool _isUpdatingText = false;
        public string Text
        {
            get { return passportTextBox.Text; }
            set { passportTextBox.Text = value; }
        }

        public PassportTextBox()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Удаление лишних символов в TextBox для паспорта
        /// </summary>
        /// <param name="e"></param>
        /// <param name="passportTextBox"></param>
        private void PassportTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back || e.Key == Key.Delete)
            {
                string text = passportTextBox.Text;
                if (text.EndsWith(" "))
                {
                    e.Handled = true; // Предотвращаем стандартное поведение Backspace или Delete

                    passportTextBox.Text = text.Substring(0, text.Length - 2); // Удалить два символа
                    passportTextBox.SelectionStart = passportTextBox.Text.Length;
                }
            }
        }

        /// <summary>
        /// Маска для TextBox с паспортом
        /// </summary>
        /// <param name="passportTextBox"></param>
        private void PassportTextBox_TextChanged(object sender, TextChangedEventArgs args)
        {
            passportTextBox.PreviewTextInput += PassportTextBox_PreviewTextInput;

            if (_isUpdatingText) return;
            _isUpdatingText = true;

            string text = passportTextBox.Text.Replace(" ", "");

            if (text.Length > 0)
            {
                // Формат: 8020 121212
                if (text.Length == 4)
                {
                    passportTextBox.Text = $"{text.Substring(0, 4)} {text.Substring(4)}";
                }
                else if (text.Length == 10)
                {
                    passportTextBox.Text = $"{text.Substring(0, 4)} {text.Substring(4, 6)}";
                }

                passportTextBox.SelectionStart = passportTextBox.Text.Length;
            }

            _isUpdatingText = false;
        }

        // <summary>
        /// Позволяет вводить только числа
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PassportTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            ActionsTextBox.ValidateInputNumbers(e);
        }

        private void PassportTextBox_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            ActionsTextBox.ValidatePasteNumbers(e);
        }
    }
}
