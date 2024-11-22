using AvtoService_3cursAA.Actions.Cars;
using AvtoService_3cursAA.Model;
using AvtoService_3cursAA.PagesMenuOperator;
using AvtoService_3cursAA.UserControls.CheckUC;
using AvtoService_3cursAA.UserControls.ListBoxUC;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvtoService_3cursAA.PagesMenuAdmin.Collections
{
    // Редактирование
    internal class ClientCollection
    {
        private static Avtoservice3cursAaContext dbContext;
        private EditCar _parentWindow;
        public ObservableCollection<ClientItem> Clients { get; set; }
        internal List<Client> _clientList;

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

    // Добавление 
    internal class ClientCollectionForAdd
    {
        private static Avtoservice3cursAaContext dbContext;
        private AddCar _parentWindow;
        public ObservableCollection<ClientItemForAdd> Clients { get; set; }
        internal List<Client> _clientList;

        public ClientCollectionForAdd(AddCar parentWindow)
        {
            _parentWindow = parentWindow;
            Clients = new ObservableCollection<ClientItemForAdd>();
            _clientList = new List<Client>();
        }

        public void AddClient(Client client)
        {
            Clients.Add(new ClientItemForAdd(client, _parentWindow));
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
