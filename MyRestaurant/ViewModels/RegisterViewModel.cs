using MyRestaurant.Helpers;
using MyRestaurant.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;
using MyRestaurant.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using MyRestaurant.Models.BuisnessLogicLayer;
using MyRestaurant.Helpers;
using System.Collections.ObjectModel;

namespace MyRestaurant.ViewModels
{
    public class RegisterViewModel : BasePropertyChanged
    {
        private string _nume;
        private string _prenume;
        private string _email;
        private string _telefon;
        private string _adresa;
        private string _tipUtilizator;
        private string _password;
        private UtilizatoriBLL _utilizatoriBLL;

        public ObservableCollection<Utilizatori> UtilizatoriList
        {
            get => _utilizatoriBLL.UtilizatoriList; // Assuming UtilizatoriList is a property in UtilizatoriBLL
            set => _utilizatoriBLL.UtilizatoriList = value; // Assuming UtilizatoriList is a property in UtilizatoriBLL
        }

        public string Nume
        {
            get => _nume;
            set { _nume = value; OnPropertyChanged(); }
        }

        public string Prenume
        {
            get => _prenume;
            set { _prenume = value; OnPropertyChanged(); }
        }

        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(); }
        }

        public string Telefon
        {
            get => _telefon;
            set { _telefon = value; OnPropertyChanged(); }
        }

        public string Adresa
        {
            get => _adresa;
            set { _adresa = value; OnPropertyChanged(); }
        }

        public string TipUtilizator
        {
            get => _tipUtilizator;
            set { _tipUtilizator = value; OnPropertyChanged(); }
        }

        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(); }
        }

        public ICommand RegisterCommand { get; }
        public ICommand BackCommand { get; }

        public RegisterViewModel()
        {
            RegisterCommand = new RelayCommand<object>(ExecuteRegister);
            BackCommand = new RelayCommand<object>(ExecuteBack);
            _utilizatoriBLL = new UtilizatoriBLL();
            UtilizatoriList = _utilizatoriBLL.GetAllUtilizatori();
        }

        private void ExecuteRegister(object parameter)
        {
            if (parameter is Utilizatori utilizator)
            {
                _utilizatoriBLL.AddMethode(utilizator);
                if (!string.IsNullOrEmpty(_utilizatoriBLL.ErrorMessage))
                {
                    MessageBox.Show(_utilizatoriBLL.ErrorMessage, "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                MessageBox.Show("Utilizatorul a fost inregistrat cu succes!", "Inregistrare", MessageBoxButton.OK, MessageBoxImage.Information);
                Nume = string.Empty;
                Prenume = string.Empty;
                Email = string.Empty;
                Telefon = string.Empty;
                Adresa = string.Empty;
                Password = string.Empty;
                TipUtilizator = string.Empty;
            }
        }

        private void ExecuteBack(object parameter)
        {
            var window = new StartWindow();
            window.Show();
            Application.Current.Windows[0]?.Close(); // Close current RegisterWindow
        }
    }
}