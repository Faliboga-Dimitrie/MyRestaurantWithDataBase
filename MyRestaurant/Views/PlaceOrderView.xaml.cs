using Microsoft.IdentityModel.Tokens;
using MyRestaurant.Models;
using MyRestaurant.ViewModels;
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

namespace MyRestaurant.Views
{
    /// <summary>
    /// Interaction logic for PlaceOrderView.xaml
    /// </summary>
    public partial class PlaceOrderView : Window
    {
        public PlaceOrderView(Utilizatori user)
        {
            InitializeComponent();
            DataContext = new PlaceOrderViewModel(user);
        }
    }
}
