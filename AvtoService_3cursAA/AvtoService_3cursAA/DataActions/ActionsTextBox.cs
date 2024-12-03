using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AvtoService_3cursAA.DataActions
{
    public static class ActionsTextBox
    {
        public static void HiddenPassword(CheckBox checkBox, PasswordBox passHid, TextBox passVis)
        {
            if (checkBox.IsChecked == true)
            {
                // Vissible pass
                passVis.Text = passHid.Password;
                passVis.Visibility = Visibility.Visible;
                passHid.Visibility = Visibility.Hidden;
            }
            else
            {
                // Hidden pass
                passHid.Password = passVis.Text;
                passVis.Visibility = Visibility.Hidden;
                passHid.Visibility = Visibility.Visible;
            }
        }

        public static int CalculateAge(DateOnly birthDate)
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.Today);
            int age = today.Year - birthDate.Year;

            if (today.Month < birthDate.Month || (today.Month == birthDate.Month && today.Day < birthDate.Day))
            {
                age--;
            }

            return age;
        }

        public static void ValidateInputCyrillic(TextCompositionEventArgs e)
        {
            // Разрешаем только буквы
            if (!Regex.IsMatch(e.Text, @"^[а-яА-Я]+$"))
            {
                e.Handled = true; // Блокируем ввод
            }
        }
        public static void ValidatePasteCyrillic(DataObjectPastingEventArgs e)
        {
            // Если да, то передаем его в отедльную строку
            string pastedText = e.DataObject.GetData(DataFormats.Text) as string;

            if (!Regex.IsMatch(pastedText, @"^[а-яА-Я]+$"))
            {
                e.CancelCommand(); // Блокируем ввод
            }
        }

        public static void ValidateInputNumbers(TextCompositionEventArgs e)
        {
            // Разрешаем только цифры
            if (!Regex.IsMatch(e.Text, @"^\d+$"))
            {
                e.Handled = true; // Блокируем ввод
            }
        }

        public static void ValidatePasteNumbers(DataObjectPastingEventArgs e)
        {
            // Если да, то передаем его в отедльную строку
            string pastedText = e.DataObject.GetData(DataFormats.Text) as string;

            if (!Regex.IsMatch(pastedText, @"^\d+$"))
            {
                e.CancelCommand(); // Блокируем ввод
            }
        }

        public static void ValidateInputDescription(TextCompositionEventArgs e)
        {
            // Разрешаем только кириллицу, цифры и специальные символы
            if (!Regex.IsMatch(e.Text, @"^[а-яА-ЯёЁ0-9\s!@#$%^&*(),.?""':;{}[\]<>-]+$"))
            {
                e.Handled = true; // Блокируем ввод
            }
        }

        public static void ValidatePasteDescription(DataObjectPastingEventArgs e)
        {
            // Если да, то передаем его в отдельную строку
            string pastedText = e.DataObject.GetData(DataFormats.Text) as string;

            if (!Regex.IsMatch(pastedText, @"^[а-яА-ЯёЁ0-9\s!@#$%^&*(),.?""':;{}[\]<>-]+$"))
            {
                e.CancelCommand(); // Блокируем вставку
            }
        }
    }
}
