using AvtoService_3cursAA.Model;
using AvtoService_3cursAA.PagesMenuAdmin.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using static MaterialDesignThemes.Wpf.Theme.ToolBar;

namespace AvtoService_3cursAA.PagesMenuAdmin.DataManagers
{
    internal class DetailManager
    {
        private static Avtoservice3cursAaContext dbContext;
        private readonly List<object> startItemsInList = new List<object>
        {
            "Выберите деталь",
            new Separator { Margin = new Thickness(0, 5, 0, 5), Width = 150 }
        };
        private List<Detail> listAllDetails; 
        private DetailCollection detailCollection;

        private ItemsControl _listViewItems;
        private ComboBox _comboBoxDetails;
        private TextBlock _costDetails;
        private PriceOpertor _parentWindow;

        public DetailManager(ItemsControl listViewItems, ComboBox comboBoxDetails, TextBlock costDetails, PriceOpertor parentWindow)
        {
            dbContext = new();

            listAllDetails = dbContext.Details.OrderBy(p => p.Name).ToList();
            _listViewItems = listViewItems;
            _comboBoxDetails = comboBoxDetails;
            _costDetails = costDetails;
            _parentWindow = parentWindow;

            FillDetails();

            detailCollection = new(parentWindow);
            _listViewItems.ItemsSource = detailCollection.Details;
        }

        public void DeleteDetailInDetailView(Detail detail)
        {
            detailCollection.RemoveDetail(detail); // удаляем из itemSource ItemControl
            listAllDetails.Add(detail); // добавляем в комбобокс
            FillDetails();
            // Обновление не требуется, поскольку ObservableCollection автоматически обновляет представление
        }

        private void AddDetailInDetailView()
        {
            var nameSelectDetail = _comboBoxDetails.SelectedItem as string; // инициализируем выбранный элемент в строку
            var selectDetail = dbContext.Details.First(p => p.Name == nameSelectDetail); // находим его в бд

            detailCollection.AddDetail(selectDetail); // добавляем в itemSource ItemControl
            listAllDetails.RemoveAt(_comboBoxDetails.SelectedIndex - 2); // удаляем его из комбобокса
        }

        public void ComboBoxDetails_SelectionChanged()
        {
            if (_comboBoxDetails.SelectedItem != null)
            {
                if (_listViewItems != null)
                {
                    if (_comboBoxDetails.SelectedIndex == 0 || _comboBoxDetails.SelectedIndex == 1) return;
                    AddDetailInDetailView();
                    FillDetails();
                }
            }
            _comboBoxDetails.SelectedIndex = 0;
        }

        private void FillDetails()
        {
            var listDetails = new List<object>(startItemsInList);

            listAllDetails = listAllDetails.OrderBy(p => p.Name).ToList();
            listDetails.AddRange(listAllDetails.Select(p => p.Name).ToList());

            _comboBoxDetails.ItemsSource = listDetails;

            int cost = 0;
            if (detailCollection != null)
            {
                foreach (var item in detailCollection.Details)
                {
                    if (item != null)
                        cost += item.Cost;
                }
            }
            _costDetails.Text = null;
            _costDetails.Text += $"\t{cost} руб.";
        }
    }
}
