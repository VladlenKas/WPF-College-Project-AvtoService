using AvtoService_3cursAA.Model;
using MaterialDesignColors;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AvtoService_3cursAA.PagesMenuAdmin.ViewModel
{
    public class CheckAdminViewModel : INotifyPropertyChanged
    {
        // Событие для оповещения участников об изменении
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        // Поля для свойств
        private Avtoservice3cursAaContext dbContext;
        private ObservableCollection<Client> _clientsList;
        private Client _selectedClient;
        private string _clientsListText;

        // Свойства
        public ObservableCollection<Client> ClientsList
        {
            get { return _clientsList; }
            set
            {
                _clientsList = value;
                OnPropertyChanged();
            }
        }
        public string ClientsListText
        {
            get { return _clientsListText; }
            set
            {
                _clientsListText = value;
                OnPropertyChanged(); // Оповещаем участников об изменении
                ApplySearchClients(_clientsListText); // Вызываем метод фильтрации при изменении текста
            }
        }

        public Client SelectedClient
        {
            get { return _selectedClient; }
            set
            {
                _selectedClient = value;
                OnPropertyChanged();
                if (_selectedClient != null)
                {
                    MessageBox.Show($"Выбран клиент: {_selectedClient.FullName}");
                }
            }
        }

        public CheckAdminViewModel()
        {
            UpdateDB();
            _clientsList = new ObservableCollection<Client>(dbContext.Clients.ToList());
        }

        private void UpdateDB()
        {
            dbContext = new Avtoservice3cursAaContext();
            dbContext.Clients
                            .Include(c => c.Carclients)
                            .ThenInclude(cc => cc.IdCarNavigation).Load();
        }

        #region SEARCH METHODS

        public void ApplySearchClients(string search)
        {
            if (_selectedClient == null) return;

            _clientsList.Clear();
            if (string.IsNullOrWhiteSpace(search)) // Если текст фильтра пуст или null
            {
                foreach (var item in dbContext.Clients)
                {
                    ClientsList.Add(item);
                }
            }
            else // Если текст фильтра не пустой
            {
                foreach (var item in dbContext.Clients)
                {
                    if (item.FullName.Contains(search))
                    {
                        ClientsList.Add(item);

                    }
                }
            }
        }
        #endregion
    }
}
