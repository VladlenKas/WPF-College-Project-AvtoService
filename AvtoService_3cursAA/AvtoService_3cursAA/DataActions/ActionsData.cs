using AvtoService_3cursAA.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AvtoService_3cursAA.ActionsForEmployee
{
    public static class ActionsData
    {
        private static Avtoservice3cursAaContext dbContext;
        public static void DeleteClient(Client user)
        {
            using (var context = new Avtoservice3cursAaContext())
            {
                context.Carclients
                    .Include(cc => cc.IdCarNavigation)
                    .Include(cc => cc.IdClientNavigation);

                var res = MessageBox.Show("Вы точно хотите удалить данного клиента?\n\nЕсли у автомобилей, " +
                    "которые привязаны к данному клиенту, отсутствуют другие автовладельцы, то данные автомобили" +
                    " будут автоматичски удалены из базы данных", "Подтверждение",
                            MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (res == MessageBoxResult.Yes)
                {
                    string carsStr = "";
                    var client = user as Client;
                    List<Car> carsToRemove = new List<Car>();

                    // Получаем все автомобили, связанные с данным клиентом
                    var carClients = context.Carclients
                        .Where(cc => cc.IdClient == client.IdClient)
                        .ToList();

                    // Перебираем каждый автомобиль клиента
                    if (carClients.Count > 0)
                    {
                        foreach (var carClient in carClients)
                        {
                            // Находим автомобиль
                            int carId = carClient.IdCar;
                            Car car = context.Cars.Include(c => c.Carclients).Single(c => c.IdCar == carId);

                            // Находим всех клиентов этого автомобиля
                            var clientsAlreadyAssigned = car.Carclients
                                .Select(cc => cc.IdClient)
                                .ToList();

                            // Если данный клиент единственный владелец авто,
                            // то сохраняем его авто и связку в список для удаления
                            if (clientsAlreadyAssigned.Count == 1)
                            {
                                carsToRemove.Add(car);
                            }
                        }
                    }

                    // Удаляем все записи
                    foreach (var carclientRemove in carClients)
                    {
                        var carclient = context.Carclients.Single(cc => cc.IdCar == carclientRemove.IdCar && cc.IdClient == client.IdClient);
                        carclient.IsDeleted = true;
                        context.Update(carclient);
                    }

                    // Удаляем все авто
                    foreach (var car in carsToRemove)
                    {
                        carsStr += $"{car.Brand} {car.Model}, ";
                        var carToRemove = context.Cars.Single(c => c.IdCar == car.IdCar);
                        carToRemove.IsDeleted = true;
                    }

                    // Удаляем клиента
                    var clientToRemove = context.Clients.Single(c => c.IdClient == client.IdClient);
                    clientToRemove.IsDeleted = true;
                    context.SaveChanges();

                    if (carsToRemove.Count > 0)
                    {
                        MessageBox.Show($"Клиент {client.FullName} и привязанные авто - {carsStr.Remove(carsStr.Length - 2)} - успешно удалены!", 
                            "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show($"Клиент {client.FullName} успешно удален!",
                           "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
        }

        public static void DeleteEmployee(Employee employee)
        {
            dbContext = new();
            var result = MessageBox.Show("Вы точно хотите удалить данного сотрудника?", "Подтверждение",
                        MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                var deleteEmployee = dbContext.Employees.First(c => c.IdEmployee == employee.IdEmployee);
                deleteEmployee.IsDeleted = true;
                dbContext.SaveChanges();

                MessageBox.Show($"Сотрудник {employee.FullName} успешно удален!", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
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

        public static void AddPrice(string name, int cost, ImageSource image)
        {
            using (var dbContext = new Avtoservice3cursAaContext())
            {
                var newPrice = new Price
                {
                    Name = name,
                    Cost = cost,
                    Photo = ImageSourceToBytes(image)
                };

                dbContext.Add(newPrice);
                dbContext.SaveChanges();

                MessageBox.Show($"Услуга «{newPrice.Name}» успешно добавлена!", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public static void EditPrice(string name, int cost, ImageSource image, Price price)
        {
            dbContext = new();
            var thisPrice = dbContext.Prices.First(r => r.IdPrice == price.IdPrice);

            byte[] newImage = ImageSourceToBytes(image);

            thisPrice.Name = name;
            thisPrice.Cost = cost;
            thisPrice.Photo = newImage;

            MessageBox.Show($"Услуга «{ thisPrice.Name}» успешно отредактирована!", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
            dbContext.Update(thisPrice);
            dbContext.SaveChanges();
        }

        public static void AddDetail(string name, int cost, int count, ImageSource image)
        {
            using (var dbContext = new Avtoservice3cursAaContext())
            {
                var newDetail = new Detail
                {
                    Name = name,
                    Cost = cost,
                    Count = count,
                    Photo = ImageSourceToBytes(image)
                };

                dbContext.Add(newDetail);
                dbContext.SaveChanges();

                MessageBox.Show($"Деталь «{newDetail.Name}» успешно добавлена!", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public static void EditDetail(string name, int cost, int count, ImageSource? image, Detail detail)
        {
            using (var dbContext = new Avtoservice3cursAaContext())
            {
                var thisDetail = dbContext.Details.First(r => r.IdDetail == detail.IdDetail);

                byte[] newImage = ImageSourceToBytes(image);

                thisDetail.Name = name;
                thisDetail.Cost = cost;
                thisDetail.Count = count;
                thisDetail.Photo = newImage;

                MessageBox.Show($"Деталь «{thisDetail.Name}» успешно отредактирована!", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                dbContext.Update(thisDetail);
                dbContext.SaveChanges();
            }
        }

        public static void EditCar(string brand, string model, string country, short year, string description,
     ImageSource image, Car car, List<Client> clients)
        {
            using (var dbContext = new Avtoservice3cursAaContext())
            {
                var thisCar = dbContext.Cars.Single(r => r.IdCar == car.IdCar);

                byte[] newImage = ImageSourceToBytes(image);

                thisCar.Brand = brand;
                thisCar.Model = model;
                thisCar.Country = country;
                thisCar.Year = year;
                thisCar.Description = description;
                thisCar.Photo = newImage;

                // Удаляем все существующие связи Carclient для этого автомобиля
                AddClientsForCar(car, clients, dbContext);

                // Обновляем автомобиль
                dbContext.Update(thisCar);

                // Сохраняем все изменения
                dbContext.SaveChanges();

                MessageBox.Show($"Машина «{thisCar.Brand} {thisCar.Model}» успешно отредактирована!", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        public static void AddCar(string brand, string model, string country, short year, string description,
    ImageSource image, List<Client> clients)
        {
            using (var context = new Avtoservice3cursAaContext())
            {
                var newCar = new Car
                {
                    Brand = brand,
                    Model = model,
                    Country = country,
                    Year = year,
                    Description = description,
                    Photo = ImageSourceToBytes(image)
                };

                // Добавляем новый автомобиль в контекст базы данных
                context.AllCars.Add(newCar);
                context.SaveChanges(); // Сохраняем автомобиль, чтобы получить IdCar

                // Проверяем, что все клиенты существуют в базе данных
                foreach (var client in clients)
                {
                    if (!context.Clients.Any(c => c.IdClient == client.IdClient))
                    {
                        throw new Exception($"Клиент с IdClient {client.IdClient} не существует в базе данных.");
                    }
                }

                // Добавляем связи между автомобилем и клиентами
                AddClientsForCarAdd(newCar, clients, context);

                // Сохраняем все изменения
                context.SaveChanges();

                MessageBox.Show($"Машина «{newCar.Brand} {newCar.Model}» успешно добавлена!", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private static void AddClientsForCarAdd(Car car, List<Client> clients, Avtoservice3cursAaContext context)
        {
            foreach (var client in clients)
            {
                Carclient carclient = new Carclient()
                {
                    IdCar = car.IdCar,
                    IdClient = client.IdClient,
                };
                context.AllCarclients.Add(carclient);
            }
        }

        private static void AddClientsForCar(Car car, List<Client> clients, Avtoservice3cursAaContext context)
        {
            // Находим все связи этого авто
            var carClientsToRemove = context.Cars
                .Where(c => c.IdCar == car.IdCar)
                .SelectMany(cc => cc.Carclients)
                .Where(cc => cc.IsDeleted != true)
                .ToList();

            // Оставляем только те связи, которых не было
            foreach (var client in new List<Client>(clients))
            {
                var saveClient = carClientsToRemove.SingleOrDefault(c => c.IdClient == client.IdClient);
                if (saveClient != null)
                {
                    carClientsToRemove.Remove(saveClient);
                    clients.Remove(client);
                }
            }

            // Удаляем новые/удаленные записи
            if (carClientsToRemove.Count != 0)
            {
                foreach (var carclient in carClientsToRemove)
                {
                    carclient.IsDeleted = true;
                }
            }

            // Добавьте новые связи Carclient
            foreach (var client in clients)
            {
                var carclientOld = context.AllCarclients.SingleOrDefault(c => c.IdCar == car.IdCar && c.IdClient == client.IdClient);
                if (carclientOld != null)
                {
                    carclientOld.IsDeleted = false;
                }
                else
                {
                    Carclient carclient = new Carclient()
                    {
                        IdCar = car.IdCar,
                        IdClient = client.IdClient,
                    };
                    context.Add(carclient);
                }
            } 
        }

        public static void AddOrderAll(Employee employee, Client client, Car car, 
            Typeofrepair typeofrepair, List<Price> prices, List<(int IdDetail, int Count)> details,
            int costForClient, int costTotal)
        {
            using (var context = new Avtoservice3cursAaContext())
            {
                // Находим перечисление связки машина-клиент
                var carclientIQueryable = context.Carclients
                    .Where(c => (c.IdCar == car.IdCar) && (c.IdClient == client.IdClient));
                // Находим айди связки перечисления
                var carclientId = carclientIQueryable
                    .Select(cc => cc.IdCarclient)
                    .Single(); 

                // Создаем новый ордер
                Sale sale = new Sale()
                {
                    IdEmployee = employee.IdEmployee,
                    IdCarclient = carclientId,
                    IdTypeofrepair = typeofrepair.IdTypeofrepair,
                    Date = DateTime.Now,
                    CostForClient = costForClient,
                    CostTotal = costTotal
                };

                // Добавляем и сохраняем ордер в бд
                context.Add(sale);
                context.SaveChanges();

                // Перебираем все детали для создания чека деталей
                // И сохраняем чек в бд
                foreach (var detail in details)
                {
                    Checkdetail checkdetail = new Checkdetail()
                    {
                        IdSale = sale.IdSale,
                        IdDetail = detail.IdDetail,
                        DetailsCount = detail.Count
                    };

                    context.Add(checkdetail);
                }

                // Перебираем все услуги для создания чека услуг
                // И сохраняем чек в бд
                foreach (var price in prices)
                {
                    Checkprice checkprice = new Checkprice()
                    {
                        IdSale = sale.IdSale,
                        IdPrice = price.IdPrice,
                    };
                    context.Add(checkprice);
                }

                // Сохраняем все изменения
                context.SaveChanges();

                // Перебираем все детали для изменения их количества
                foreach (var detail in details)
                {
                    var newDetail = context.Details.First(d => d.IdDetail == detail.IdDetail);
                    newDetail.Count -= detail.Count;

                    context.Update(newDetail);
                }

                // Сохраняем все изменения
                context.SaveChanges();

                MessageBox.Show($"Чек, оформленный на клиента {client.FullName}, успешно сохранен!", "Закрыть",
                        MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public static void AddOrderPrices(Employee employee, Client client, Car car,
            Typeofrepair typeofrepair, List<Price> prices, int costForClient, int costTotal)
        {
            using (var context = new Avtoservice3cursAaContext())
            {
                // Находим перечисление связки машина-клиент
                var carclientIQueryable = context.Carclients
                    .Where(c => (c.IdCar == car.IdCar) && (c.IdClient == client.IdClient));
                // Находим айди связки перечисления
                var carclientId = carclientIQueryable
                    .Select(cc => cc.IdCarclient)
                    .Single();

                // Создаем новый ордер
                Sale sale = new Sale()
                {
                    IdEmployee = employee.IdEmployee,
                    IdCarclient = carclientId,
                    IdTypeofrepair = typeofrepair.IdTypeofrepair,
                    Date = DateTime.Now,
                    CostForClient = costForClient,
                    CostTotal = costTotal
                };

                // Добавляем и сохраняем ордер в бд
                context.Add(sale);
                context.SaveChanges();

                // Перебираем все услуги для создания чека услуг
                // И сохраняем чек в бд
                foreach (var price in prices)
                {
                    Checkprice checkprice = new Checkprice()
                    {
                        IdSale = sale.IdSale,
                        IdPrice = price.IdPrice,
                    };
                    context.Add(checkprice);
                }

                // Сохраняем все изменения
                context.SaveChanges();

                MessageBox.Show($"Чек, оформленный на клиента {client.FullName}, успешно сохранен!", "Закрыть",
                        MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public static void AddOrderDetails(Employee employee, Client client, Car car,
            Typeofrepair typeofrepair, List<(int IdDetail, int Count)> details,
            int costForClient, int costTotal)
        {
            using (var context = new Avtoservice3cursAaContext())
            {
                // Находим перечисление связки машина-клиент
                var carclientIQueryable = context.Carclients
                    .Where(c => (c.IdCar == car.IdCar) && (c.IdClient == client.IdClient));
                // Находим айди связки перечисления
                var carclientId = carclientIQueryable
                    .Select(cc => cc.IdCarclient)
                    .Single();

                // Создаем новый ордер
                Sale sale = new Sale()
                {
                    IdEmployee = employee.IdEmployee,
                    IdCarclient = carclientId,
                    IdTypeofrepair = typeofrepair.IdTypeofrepair,
                    Date = DateTime.Now,
                    CostForClient = costForClient,
                    CostTotal = costTotal
                };

                // Добавляем и сохраняем ордер в бд
                context.Add(sale);
                context.SaveChanges();

                // Перебираем все детали для создания чека деталей
                // И сохраняем чек в бд
                foreach (var detail in details)
                {
                    Checkdetail checkdetail = new Checkdetail()
                    {
                        IdSale = sale.IdSale,
                        IdDetail = detail.IdDetail,
                        DetailsCount = detail.Count
                    };

                    context.Add(checkdetail);
                }

                // Сохраняем все изменения
                context.SaveChanges();

                // Перебираем все детали для изменения их количества
                foreach (var detail in details)
                {
                    var newDetail = context.Details.First(d => d.IdDetail == detail.IdDetail);
                    newDetail.Count -= detail.Count;

                    context.Update(newDetail);
                }

                // Сохраняем все изменения
                context.SaveChanges();

                MessageBox.Show($"Чек, оформленный на клиента {client.FullName}, успешно сохранен!", "Закрыть",
                        MessageBoxButton.OK, MessageBoxImage.Information);
            }
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
        public static byte[]? ImageSourceToBytes(ImageSource? imageSource)
        {
            byte[]? bytes = null;

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

        public static string GetImageHash(byte[] imageBytes)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(imageBytes);
                return Convert.ToBase64String(hashBytes);
            }
        }

        public static bool AreImagesEqual(ImageSource img1, ImageSource img2)
        {
            if (img1.Width != img2.Width || img1.Height != img2.Height)
            {
                return false; // Изображения разного размера
            }

            byte[] bytesImg1 = ImageSourceToBytes(img1);
            byte[] bytesImg2 = ImageSourceToBytes(img2);

            // Сравниваем пиксели
            for (int i = 0; i < bytesImg1.Length; i++)
            {
                if (bytesImg1[i] != bytesImg2[i])
                {
                    return false; // Найдено различие
                }
            }

            return true; // Изображения идентичны
        }
    }
}
