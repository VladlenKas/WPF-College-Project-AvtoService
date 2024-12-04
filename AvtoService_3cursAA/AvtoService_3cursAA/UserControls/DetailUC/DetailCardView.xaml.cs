using AvtoService_3cursAA.Model;
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
    /// Логика взаимодействия для DetailCardView.xaml
    /// </summary>
    
    public partial class DetailCardView : UserControl
    {
        public SolidColorBrush SolidColorBrush { get; set; }
        public DetailCardView(Detail detail, SolidColorBrush solidColorBrush)
        {
            SolidColorBrush = solidColorBrush;

            InitializeComponent();
            border.BorderBrush = SolidColorBrush;

            DataContext = detail;

            if (detail.Photo == null)
            {
                string file = "pack://application:,,,/AvtoService_3cursAA;component/Images/NoImagePrice.jpg";
                ImageBorder.ImageSource = new BitmapImage(new Uri(file, UriKind.Absolute));
            }
        }
    }
}
