using AvtoService_3cursAA.ActionsForEmployee;
using AvtoService_3cursAA.DataActions;
using AvtoService_3cursAA.Model;
using AvtoService_3cursAA.PagesMenuAdmin.Collections;
using AvtoService_3cursAA.PagesMenuOperator.DataManager;
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

namespace AvtoService_3cursAA.Actions.Cars
{
    /// <summary>
    /// Логика взаимодействия для EditCar.xaml
    /// </summary>
    public partial class EditCar : Window
    {
        private string Brand => BrandTextBox.Text;
        private string Model => ModelTextBox.Text;
        private string Country => CountryTextBox.Text;
        private short Year
        {
            get
            {
                if (short.TryParse(YearTextBox.Text, out short value))
                {
                    // Получаем текущий год
                    int currentYear = DateTime.Now.Year;

                    // Проверяем, что год находится в допустимом диапазоне
                    if (value >= 1990 && value <= currentYear)
                    {
                        return value; // Год валиден
                    }
                }

                return 0;
            }
        }

        private string Description => DescriptionTextBox.Text;
        private ImageSource _imageNull { get; set; }
        private ImageSource? _imageCarThis;
        private ImageSource Image
        {
            get
            {
                return ImageCar.Source;
            }
            set
            {
                ImageCar.Source = value;
            }
        }

        string _file = "pack://application:,,,/AvtoService_3cursAA;component/Images/NoImageCar.jpg";
        public Car _selectedCarEdit;
        private ClientManager clientManager;
        Avtoservice3cursAaContext dbContext;

        public EditCar(Car selectedCar)
        {
            _selectedCarEdit = selectedCar;

            dbContext = new();
            InitializeComponent();

            DataContext = _selectedCarEdit;
            clientManager = new ClientManager(ListSelectClients, ClientsComboBox, this, TextForCar, _selectedCarEdit);
        }
        public void DeletePriceInPriceView(Client client) => clientManager.DeleteClientInItemsView(client);

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            // Находим автомобиль для сверки сведений
            var _oldCar = dbContext.Cars
                .Include(c => c.Carclients)
                .Where(c => c.Carclients.Any(cc => cc.IsDeleted != true))
                .Single(c => c.IdCar == _selectedCarEdit.IdCar);

            // Сравниваем, менялись ли изображения
            bool carImageWasChanged = !ActionsData.AreImagesEqual(_imageCarThis, Image); // Предполагается, что у вас есть поле Image в модели Car

            // Проверяем, менялись ли пользователи
            bool clientsChenged = false;
            foreach (var clientUserControl in clientManager.ClientCollection.Clients)
            {
                var carclientOld = dbContext.AllCarclients
                    .Where(cc => cc.IsDeleted != true)
                    .SingleOrDefault(cc => cc.IdClient == clientUserControl.Client.IdClient && cc.IdCar == _oldCar.IdCar && cc.IsDeleted != true);

                if (carclientOld == null ||
                    ListSelectClients.Items.Count != dbContext.Carclients.Where(cc => cc.IdCar == _oldCar.IdCar).ToList().Count)
                {
                    clientsChenged = true;
                    break;
                }
            }

            // Проверка на изменение данных автомобиля
            bool isCarDataChanged =
                _oldCar.Brand != Brand ||
                _oldCar.Model != Model ||
                _oldCar.Country != Country ||
                _oldCar.Year != Year ||
                _oldCar.Description != Description ||
                clientsChenged ||
                carImageWasChanged;

            // Проверка на валидность данных
            if (!DataValidate()) return;

            if (isCarDataChanged)
            {
                // Данные изменились, вызываем метод редактирования
                if (!ActionsData.AreImagesEqual(Image, _imageNull)) // Предполагается, что _imageNull определен как изображение по умолчанию
                {
                    ActionsData.EditCar(Brand, Model, Country, Year, Description, Image, _selectedCarEdit, clientManager.ReturnClients());
                }
                else
                {
                    ActionsData.EditCar(Brand, Model, Country, Year, Description, null, _selectedCarEdit, clientManager.ReturnClients());
                }
            }
            else
            {
                // Данные не изменились, продолжаем редактирование 
                var button = MessageBox.Show("Данные не изменились. Продолжить редактирование?", "Редактирование",
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

            if (string.IsNullOrWhiteSpace(Brand) || string.IsNullOrWhiteSpace(Model) || string.IsNullOrWhiteSpace(Country))
            {
                errorsList.Add("Заполните все обязательные поля");
            }

            if (dbContext.Cars.Any(r => r.Brand.Replace(" ", "").ToLower() == Brand.Replace(" ", "").ToLower()
                && r.Model.Replace(" ", "").ToLower() == Model.Replace(" ", "").ToLower()
                && r.IdCar != _selectedCarEdit.IdCar))
            {
                errorsList.Add("Такая машина уже существует");
            }

            if (Year == 0)
            {
                errorsList.Add("Год должен быть не меньше 1990 и не больше текущего года");
            }

            if (clientManager.ReturnClients().Count == 0)
            {
                errorsList.Add("Машина должна иметь хотя бы одного привязанного клиента");
            }

            if (clientManager.ReturnClients().Count > 3)
            {
                errorsList.Add("Машина не может иметь больше трех клиентов");
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

        private void NumberTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            ActionsTextBox.ValidateInputNumbers(e);
        }

        private void NumberTextBox_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            ActionsTextBox.ValidatePasteNumbers(e);
        }

        private void ImageChange_Click(object sender, RoutedEventArgs e)
        {
            ActionsData.OpenImage(ImageCar);
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            ImageCar.Source = new BitmapImage(new Uri(_file, UriKind.Absolute));
        }

        private void DescriptionTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            ActionsTextBox.ValidateInputDescription(e);
        }

        private void DescriptionTextBox_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            ActionsTextBox.ValidatePasteDescription(e);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _imageNull = new BitmapImage(new Uri(_file, UriKind.Absolute));

            if (_selectedCarEdit.Photo == null)
            {
                _imageCarThis = new BitmapImage(new Uri(_file, UriKind.Absolute));
                ImageCar.Source = new BitmapImage(new Uri(_file, UriKind.Absolute));
            }
            else
            {
                _imageCarThis = ImageCar.Source;
            }
        }
    }
}
