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
    /// Логика взаимодействия для DateTextBox.xaml
    /// </summary>
    public partial class DateTextBox : UserControl 
    {
        private bool _isUpdatingText = false;
        public string Text
        {
            get { return dateTextBox.Text; }
            set { dateTextBox.Text = value; }
        }

        public DateTextBox()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Удаление лишних символов в TextBox для даты
        /// </summary>
        /// <param name="e"></param>
        /// <param name="dateTextBox"></param>
        private void dateTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back || e.Key == Key.Delete)
            {
                string text = dateTextBox.Text;
                if (text.EndsWith("."))
                {
                    e.Handled = true; // Предотвращаем стандартное поведение Backspace или Delete

                    dateTextBox.Text = text.Substring(0, text.Length - 2); // Удалить два символа
                    dateTextBox.SelectionStart = dateTextBox.Text.Length;
                }
            }
        }

        /// <summary>
        /// Маска для TextBox с датой
        /// </summary>
        /// <param name="dateTextBox"></param>
        private void dateTextBox_TextChanged(object sender, TextChangedEventArgs args)
        {
            if (_isUpdatingText) return;

            _isUpdatingText = true;

            string text = dateTextBox.Text.Replace(".", "");

            if (text.Length > 0)
            {
                if (text.Length <= 2)
                {
                    dateTextBox.Text = text;
                }
                else if (text.Length <= 4)
                {
                    dateTextBox.Text = $"{text.Substring(0, 2)}.{text.Substring(2)}";
                }
                else if (text.Length <= 8)
                {
                    dateTextBox.Text = $"{text.Substring(0, 2)}.{text.Substring(2, 2)}.{text.Substring(4)}";
                }

                dateTextBox.SelectionStart = dateTextBox.Text.Length;
            }

            _isUpdatingText = false;
        }

        /// <summary>
        /// Позволяет вводить только числа
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dateTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            ActionsTextBox.ValidateInputNumbers(e);
        }
    }
}
