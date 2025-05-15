using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MyRestaurant.Helpers;
using MyRestaurant.Models;
using MyRestaurant.Models.BuisnessLogicLayer;
using MyRestaurant.Views;


namespace MyRestaurant.ViewModels
{
    public class LoginViewModel : BasePropertyChanged
    {
        private string _email;
        private string _password;
        private string _errorMessage;
        private UtilizatoriBLL _utilizatoriBLL;

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Utilizatori> UtilizatoriList
        {
            get => _utilizatoriBLL.UtilizatoriList; // Assuming UtilizatoriList is a property in UtilizatoriBLL
            set => _utilizatoriBLL.UtilizatoriList = value; // Assuming UtilizatoriList is a property in UtilizatoriBLL
        }

        public ICommand LoginCommand { get; }
        public ICommand BackCommand { get; }

        public LoginViewModel()
        {
            _utilizatoriBLL = new UtilizatoriBLL();
            UtilizatoriList = _utilizatoriBLL.GetAllUtilizatori();
            LoginCommand = new RelayCommand<object>(Login);
            BackCommand = new RelayCommand<object>(ExecuteBack);
        }

        private void Login(object obj)
        {
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
            {
                ErrorMessage = "Email si parola sunt obligatorii!";
                return;
            }
            var user = UtilizatoriList.FirstOrDefault(u => u.Email == Email && u.Parola == Password);
            if (user != null)
            {
                var window = new MainWindow(user);
                window.Show();
                Application.Current.Windows[0]?.Close();
            }
            else
            {
                ErrorMessage = "Email sau parola gresita!";
            }
        }

        private void ExecuteBack(object parameter)
        {
            var window = new StartWindow();
            window.Show();
            Application.Current.Windows[0]?.Close(); // Close current LoginWindow
        }
    }
}
