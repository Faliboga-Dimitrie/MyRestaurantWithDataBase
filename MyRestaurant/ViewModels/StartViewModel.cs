using MyRestaurant.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MyRestaurant.Views;
using System.Windows;

namespace MyRestaurant.ViewModels
{
    public class StartViewModel
    {
        public ICommand ViewMenuCommand { get; }
        public ICommand RegisterCommand { get; }
        public ICommand LoginCommand { get; }

        public ICommand ExitCommand { get; }

        public StartViewModel()
        {
            ViewMenuCommand = new RelayCommand<object>(ViewMenu);
            RegisterCommand = new RelayCommand<object>(Register);
            LoginCommand = new RelayCommand<object>(Login);
            ExitCommand = new RelayCommand<object>(Exit);
        }

        private void ViewMenu(object obj)
        {
            var window = new MenuWindow();
            window.Show();
            Application.Current.Windows[0]?.Close(); // Close current StartWindow
        }

        private void Register(object obj)
        {
            var window = new RegisterWindow();
            window.Show();
            Application.Current.Windows[0]?.Close(); // Close current StartWindow
        }

        private void Login(object obj)
        {
            var window = new LoginWindow();
            window.Show();
            Application.Current.Windows[0]?.Close(); // Close current StartWindow
        }

        private void Exit(object obj)
        {
            Application.Current.Shutdown();
        }
    }
}
