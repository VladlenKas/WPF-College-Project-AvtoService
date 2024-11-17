using AvtoService_3cursAA.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AvtoService_3cursAA.ActionsForEmployee
{
    public static class ActionsUsers
    {
        private static Avtoservice3cursAaContext dbContext;
        public static void DeleteUser(object user)
        {
            dbContext = new();

            MessageBoxResult result = MessageBox.Show("Вы точно хотите удалить данного пользователя?", "Подтверждение",
                       MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                dbContext.Remove(user);
                dbContext.SaveChanges();
            }
        }

        public static void AddEmployee(string name, string firstname, string? patronymic, string birthday,
            string seniority, string role, string passport, string phone, string login, string password)
        {
            dbContext = new();

            Employee employee = new Employee
            {
                Name = name,
                Firstname = firstname,
                Patronymic = patronymic,
                Birthday = DateOnly.ParseExact(birthday, "dd.MM.yyyy"),
                Seniority = int.Parse(seniority),
                Passport = passport,
                Phone = phone,
                IdRole = dbContext.Roles.First(r => r.Name == role).IdRole,
                Login = login,
                Password = password
            };

            MessageBox.Show($"Пользователь {employee.FullName} успешно добавлен!", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
            dbContext.Add(employee);
            dbContext.SaveChanges();
        }

        public static void EditEmployee(string name, string firstname, string? patronymic, string birthday,
            string seniority, string role, string passport, string phone, string login, string password, Employee employee)
        {
            dbContext = new();
            var thisEmployee = dbContext.Employees.First(r => r.IdEmployee == employee.IdEmployee);

            thisEmployee.Name = name;
            thisEmployee.Firstname = firstname;
            thisEmployee.Patronymic = patronymic;
            thisEmployee.Birthday = DateOnly.ParseExact(birthday, "dd.MM.yyyy");
            thisEmployee.Seniority = int.Parse(seniority);
            thisEmployee.Passport = passport;
            thisEmployee.Phone = phone;
            thisEmployee.IdRole = dbContext.Roles.First(r => r.Name == role).IdRole;
            thisEmployee.Login = login;
            thisEmployee.Password = password;

            MessageBox.Show($"Пользователь {thisEmployee.FullName} успешно отредактирован!", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
            dbContext.Update(thisEmployee);
            dbContext.SaveChanges();
        }

        public static void AddClient(string name, string firstname, string? patronymic, string birthday, string phone)
        {
            dbContext = new();

            Client client = new Client
            {
                Name = name,
                Firstname = firstname,
                Patronymic = patronymic,
                Birthday = DateOnly.ParseExact(birthday, "dd.MM.yyyy"),
                Phone = phone.Replace(" ", "")
            };

            MessageBox.Show($"Пользователь {client.FullName} успешно добавлен!", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
            dbContext.Add(client);
            dbContext.SaveChanges();
        }

        public static void EditClient(string name, string firstname, string? patronymic, string birthday, string phone, Client client)
        {
            dbContext = new();
            var thisClient = dbContext.Clients.First(r => r.IdClient == client.IdClient);

            thisClient.Name = name;
            thisClient.Firstname = firstname;
            thisClient.Patronymic = patronymic;
            thisClient.Birthday = DateOnly.ParseExact(birthday, "dd.MM.yyyy");
            thisClient.Phone = phone.Replace(" ", "");

            MessageBox.Show($"Пользователь {thisClient.FullName} успешно отредактирован!", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
            dbContext.Update(thisClient);
            dbContext.SaveChanges();
        }

        public static void EditPrice(string name, string cost, ImageSource image, Price price)
        {
            dbContext = new();
            var thisPrice = dbContext.Prices.First(r => r.IdPrice == price.IdPrice);

            byte[] newImage = ImageSourceToBytes(image);

            thisPrice.Name = name;
            thisPrice.Cost = int.Parse(cost);
            thisPrice.Photo = newImage;

            MessageBox.Show($"Услуга «{ thisPrice.Name}» успешно отредактирована!", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
            dbContext.Update(thisPrice);
            dbContext.SaveChanges();
        }

        public static void OpenImage(Image image)
        {
            var selectImage = new OpenFileDialog();
            selectImage.Filter = "Image files (*.jpg, *.jpeg, *.png *.webp)|*.jpg;*.jpeg;*.png;*.webp;";
            selectImage.InitialDirectory = @"C:\Users";
            if (selectImage.ShowDialog() == true)
            {
                string selectedFilePath = selectImage.FileName;
                image.Source = new BitmapImage(new Uri(selectedFilePath));
            }
        }
        public static byte[] ImageSourceToBytes(ImageSource imageSource)
        {
            byte[] bytes = null;
            var bitmapSource = imageSource as BitmapSource;
            if (bitmapSource != null)
            {
                var encoder = new JpegBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
                using (var stream = new MemoryStream())
                {
                    encoder.Save(stream);
                    bytes = stream.ToArray();
                }
            }
            return bytes;
        }
    }
}
