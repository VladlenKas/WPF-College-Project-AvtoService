using AvtoService_3cursAA.Actions.Cars;
using AvtoService_3cursAA.Model;
using AvtoService_3cursAA.PagesMenuOperator;
using AvtoService_3cursAA.UserControls.CheckUC;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvtoService_3cursAA.PagesMenuAdmin.Collections
{
    internal class ClientCollection
    {

        private static Avtoservice3cursAaContext dbContext;
        private EditCar _parentWindow;
        public ObservableCollection<ClientItem> Clients { get; set; }
        private List<Client> _clientList;
        public ClientCollection(EditCar parentWindow)
        {
            _parentWindow = parentWindow;
            Clients = new ObservableCollection<ClientItem>();
            _clientList = new List<Client>();
        }

        public void AddClient(Client client)
        {
            Clients.Add(new ClientItem(client, _parentWindow));
            _clientList.Add(client);
        }

        public void RemoveClient(Client client)
        {
            int index = _clientList.FindIndex(d => d.IdClient == client.IdClient);
            Clients.RemoveAt(index);
            _clientList.RemoveAt(index);
        }
    }
}
