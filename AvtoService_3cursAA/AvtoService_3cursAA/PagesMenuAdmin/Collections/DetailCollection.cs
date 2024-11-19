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
    internal class DetailCollection
    {
        private CheckAdmin _parentWindow;
        public ObservableCollection<DetailItem> Details { get; set; }
        private List<Detail> _detailList;
        public DetailCollection(CheckAdmin parentWindow)
        {
            _parentWindow = parentWindow;
            Details = new ObservableCollection<DetailItem>();
            _detailList = new List<Detail>();
        }

        public void AddDetail(Detail detail)
        {
            Details.Add(new DetailItem(detail, _parentWindow));
            _detailList.Add(detail);
        }

        public void RemoveDetail(Detail detail)
        {
            int index = _detailList.FindIndex(d => d.IdDetail == detail.IdDetail);
            Details.RemoveAt(index);
            _detailList.RemoveAt(index);
        }
    }
}
