using AvtoService_3cursAA.Actions;
using AvtoService_3cursAA.Model;
using AvtoService_3cursAA.PagesMenuAdmin;
using AvtoService_3cursAA.PagesMenuOperator;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AvtoService_3cursAA.UserControls.DetailUC
{
    /// <summary>
    /// Логика взаимодействия для DetailCardEdit.xaml
    /// </summary>
    public partial class DetailCardEdit : UserControl
    {
        public Detail Detail { get; private set; }

        private Detail _detail;
        private DetailOperator _parentWindow;
        private Avtoservice3cursAaContext dbContext;

        public event EventHandler<DetailEventArgs> RemoveDetailRequested; // Событие для удаления детали

        public DetailCardEdit(Detail detail, DetailOperator detailAdmin)
        {
            _detail = detail;
            _parentWindow = detailAdmin;

            InitializeComponent();
            DataLoad();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            EditDetail editDetail = new(_detail);
            editDetail.ShowDialog();

            DataLoad();
            RemoveDetailRequested?.Invoke(this, new DetailEventArgs { Detail = this.Detail }); // Уведомляем родительское окно
        }

        private void DataLoad()
        {
            dbContext = new();
            _detail = dbContext.Details.First(r => r.IdDetail == _detail.IdDetail);
            DataContext = _detail;

            if (_detail.Photo == null)
            {
                string file = "pack://application:,,,/AvtoService_3cursAA;component/Images/NoImageDetail.jpg";
                ImageBorder.ImageSource = new BitmapImage(new Uri(file, UriKind.Absolute));
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы уверены, что хотите удалить деталь?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question)
                == MessageBoxResult.Yes)
            {
                DeleteDetail();
                MessageBox.Show("Деталь успешно удалена!", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                RemoveDetailRequested?.Invoke(this, new DetailEventArgs { Detail = this.Detail }); // Уведомляем родительское окно
            }
        }

        private void DeleteDetail()
        {
            dbContext = new();
            var detailToRemove = dbContext.Details.First(d => d.IdDetail == _detail.IdDetail);
            if (detailToRemove != null)
            {
                detailToRemove.IsDeleted = true;
                dbContext.SaveChanges();
            }
        }
    }

    public class DetailEventArgs : EventArgs
    {
        public Detail Detail { get; set; }
    }
}
