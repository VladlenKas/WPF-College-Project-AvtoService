using AvtoService_3cursAA.Model;
using AvtoService_3cursAA.PagesMenuAdmin.Collections;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AvtoService_3cursAA.PagesMenuAdmin.DataManagers
{
    internal class PriceManager
    {
        private static Avtoservice3cursAaContext dbContext;
        private readonly List<object> startItemsInList = new List<object>
        {
            "Выберите услугу",
            new Separator { Margin = new Thickness(0, 5, 0, 5), Width = 150 }
        };
        private List<Price> listAllPrice;
        private PriceCollection priceCollection;

        private ItemsControl _listViewItems;
        private ComboBox _comboBoxPrices;
        private CheckAdmin _parentWindow;

        public PriceManager(ItemsControl listViewItems, ComboBox comboBoxPrices, CheckAdmin parentWindow)
        {
            dbContext = new();

            listAllPrice = dbContext.Prices.OrderBy(p => p.Name).ToList();
            _listViewItems = listViewItems;
            _parentWindow = parentWindow;
            _comboBoxPrices = comboBoxPrices;

            FillPrices();

            priceCollection = new(parentWindow);
            _listViewItems.ItemsSource = priceCollection.Prices;
        }
        public void DeletePriceInPriceView(Price price)
        {
            priceCollection.RemovePrice(price); // удаляем из itemSource ItemControl 
            listAllPrice.Add(price); // добавляем в кмобобокс
            FillPrices();
            // Обновление не требуется, поскольку ObservableCollection автоматически обновляет представление
        }
        private void AddPriceInPriceView()
        {
            var nameSelectPrice = _comboBoxPrices.SelectedItem as string; // иниализируем выбранный элемент в строку
            var selectPrice = dbContext.Prices.First(p => p.Name == nameSelectPrice); // находим его в бд

            priceCollection.AddPrice(selectPrice); // добавляем в itemSource ItemControl
            listAllPrice.RemoveAt(_comboBoxPrices.SelectedIndex - 2); // удаляем его из комбобокса
        }
        public void PriceComboBox_SelectionChanged()
        {
            if (_comboBoxPrices.SelectedItem != null)
            {
                if (_listViewItems != null)
                {
                    if (_comboBoxPrices.SelectedIndex == 0 || _comboBoxPrices.SelectedIndex == 1) return;
                    AddPriceInPriceView();
                    FillPrices();
                }
            }

            _comboBoxPrices.SelectedIndex = 0;
        }
        private void FillPrices()
        {
            var listPrices = new List<object>(startItemsInList);

            listAllPrice = listAllPrice.OrderBy(p => p.Name).ToList();
            listPrices.AddRange(listAllPrice.Select(p => p.Name).ToList());

            _comboBoxPrices.ItemsSource = listPrices;
        }
    }
}
