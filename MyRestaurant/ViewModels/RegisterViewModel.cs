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

namespace MyRestaurant.ViewModels
{
    public class RegisterViewModel : INotifyPropertyChanged
    {
        private string _nume;
        private string _prenume;
        private string _email;
        private string _telefon;
        private string _adresa;
        private string _tipUtilizator;

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

        public ICommand RegisterCommand { get; }
        public ICommand BackCommand { get; }

        public RegisterViewModel()
        {
            RegisterCommand = new RelayCommand<object>(ExecuteRegister);
            BackCommand = new RelayCommand<object>(ExecuteBack);
        }

        private void ExecuteRegister(object parameter)
        {
            if (parameter is Utilizatori utilizator)
            {
                // Save to DB or call a service here
                // e.g., Database.Save(utilizator);
                MessageBox.Show("Utilizatorul a fost inregistrat cu succes!", "Inregistrare", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void ExecuteBack(object parameter)
        {
            var window = new StartWindow();
            window.Show();
            Application.Current.Windows[0]?.Close(); // Close current RegisterWindow
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}