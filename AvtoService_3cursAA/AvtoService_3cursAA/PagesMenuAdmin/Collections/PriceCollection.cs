using AvtoService_3cursAA.Model;
using AvtoService_3cursAA.UserControls.CheckUC;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvtoService_3cursAA.PagesMenuAdmin.Collections
{
    internal class PriceCollection
    {
        private CheckAdmin _parentWindow;
        public ObservableCollection<PriceItem> Prices { get; set; }
        private List<Price> _pricesList;
        public PriceCollection(CheckAdmin parentWindow)
        {
            _parentWindow = parentWindow;
            Prices = new ObservableCollection<PriceItem>();
            _pricesList = new List<Price>();
        }

        public void AddPrice(Price price)
        {
            Prices.Add(new PriceItem(price, _parentWindow));
            _pricesList.Add(price);
        }

        public void RemovePrice(Price price)
        {
            int index = _pricesList.FindIndex(p => p.IdPrice == price.IdPrice);
            Prices.RemoveAt(index);
            _pricesList.RemoveAt(index);
        }
    }
}
