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
using System.Collections.ObjectModel;
using System.Windows;

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

        public ObservableCollection<Alergeni> AlergeniList
        {
            get => _alergenBLL.AlergeniList; 
            set => _alergenBLL.AlergeniList = value;
        }

        public string OldName { get; set; } // Store the old name for comparison

        private Alergeni _selectedAlergen;

        public Alergeni SelectedAlergen
        {
            get => _selectedAlergen;
            set
            {
                _selectedAlergen = value;
                OnPropertyChanged();

                OldName = _selectedAlergen?.NumeAlergen; // Store the old name for comparison
            }
        }

        public ICommand BackToMain { get; }
        public ICommand AddAlergenCommand { get; }

        public ICommand UpdateAlergenCommand { get; }

        public ICommand DeleteAlergenCommand { get; }

        public ModifyAlergenViewModel(Utilizatori user)
        {
            _alergenBLL = new AlergeniBLL();
            CurrentUser = user;
            AlergeniList = _alergenBLL.GetAlergeniList(); 
            BackToMain = new RelayCommand<object>(Back);
            AddAlergenCommand = new RelayCommand<object>(AddAlergen);
            UpdateAlergenCommand = new RelayCommand<object>(UpdateAlergen);
            DeleteAlergenCommand = new RelayCommand<object>(DeleteAlergen);
        }
        public void AddAlergen(object param)
        {
            if (string.IsNullOrEmpty(AlergenName))
            {
                ErrorMessage = "Please enter a name for the alergen.";
                return;
            }
            else if (AlergeniList.Any(a => a.NumeAlergen == AlergenName))
            {
                ErrorMessage = "An alergen with this name already exists.";
                return;
            }

            if(param is Alergeni alergen)
            {
                _alergenBLL.AddMethode(alergen);
                ErrorMessage = _alergenBLL.ErrorMessage;
                if(!string.IsNullOrEmpty(ErrorMessage))
                {
                    MessageBox.Show(ErrorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                AlergenName = string.Empty; // Clear the input field after adding
                MessageBox.Show("Alergen added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }

        public void UpdateAlergen(object param)
        {
            if (string.IsNullOrEmpty(AlergenName))
            {
                ErrorMessage = "Please enter a name for the alergen.";
                return;
            }
            // Check if any other category has the new name (excluding the old name)
            if (AlergeniList.Any(c => c.NumeAlergen.Equals(AlergenName, StringComparison.OrdinalIgnoreCase)
                                     && !c.NumeAlergen.Equals(OldName, StringComparison.OrdinalIgnoreCase)))
            {
                ErrorMessage = "A category with this name already exists.";
                return;
            }

            if (param is Alergeni alergen)
            {
                // Update the category's name with the new name
                alergen.Idalergen = SelectedAlergen.Idalergen; // Ensure the ID is set correctly

                _alergenBLL.UpdateMethode(alergen);
                ErrorMessage = _alergenBLL.ErrorMessage;
                if (!string.IsNullOrEmpty(ErrorMessage))
                {
                    MessageBox.Show(ErrorMessage, "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Update the category in the ObservableCollection
                var itemToRemove = AlergeniList.FirstOrDefault(c => c.NumeAlergen.Equals(OldName, StringComparison.OrdinalIgnoreCase));
                AlergeniList.Remove(itemToRemove);
                AlergeniList.Add(alergen); // Add the updated category
                AlergenName = string.Empty; // Clear input
                OldName = string.Empty;       // Clear old name
                MessageBox.Show("Category updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public void DeleteAlergen(object param)
        {
            if (param is Alergeni alergen)
            {
                alergen.Idalergen = SelectedAlergen.Idalergen; // Ensure the ID is set correctly
                _alergenBLL.DeleteMethode(alergen);
                ErrorMessage = _alergenBLL.ErrorMessage;
                if (!string.IsNullOrEmpty(ErrorMessage))
                {
                    MessageBox.Show(ErrorMessage, "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                MessageBox.Show("Category deleted successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                AlergenName = string.Empty; // Clear input
                OldName = string.Empty;
            }
        }

        public void Back(object parameter)
        {
            var window = new MainWindow(CurrentUser);
            window.Show();

        }
    }
}
