using MyRestaurant.Models;
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
using MyRestaurant.ViewModels;

namespace MyRestaurant.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(Utilizatori user)
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel(user);
        }

        private void EditMenuButton_Click(object sender, RoutedEventArgs e)
        {
            if (EditMenuButton.ContextMenu != null)
            {
                EditMenuButton.ContextMenu.PlacementTarget = EditMenuButton; // position relative to button
                EditMenuButton.ContextMenu.IsOpen = true; // open the menu
            }
        }

    }
}
