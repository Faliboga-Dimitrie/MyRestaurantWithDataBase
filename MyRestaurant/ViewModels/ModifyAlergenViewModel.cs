using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyRestaurant.Helpers;
using MyRestaurant.Models.BuisnessLogicLayer;
using MyRestaurant.Models;
using MyRestaurant.Views;
using System.Windows.Input;

namespace MyRestaurant.ViewModels
{
    public class ModifyAlergenViewModel : BasePropertyChanged
    {
        private string _alergenName;
        private string _errorMessage;
        private AlergeniBLL _alergenBLL;
        private Utilizatori _currentUser;
        public string AlergenName
        {
            get => _alergenName;
            set
            {
                _alergenName = value;
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

        public Utilizatori CurrentUser
        {
            get => _currentUser;
            set
            {
                _currentUser = value;
                OnPropertyChanged();
            }
        }

        public ICommand BackToMain { get; }
        public ICommand AddAlergenCommand { get; }

        public ModifyAlergenViewModel(Utilizatori user)
        {
            _alergenBLL = new AlergeniBLL();
            CurrentUser = user;
        }
        public void AddAlergen()
        {
            if (string.IsNullOrEmpty(AlergenName))
            {
                ErrorMessage = "Please enter a name for the alergen.";
                return;
            }
            
        }

        public void Back(object parameter)
        {
            var window = new MainWindow(CurrentUser);
            window.Show();

        }
    }
}
