using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyRestaurant.Helpers;
using MyRestaurant.Models.BuisnessLogicLayer;
using MyRestaurant.Models;
using System.Windows.Input;
using System.Windows;
using MyRestaurant.Views;
using MyRestaurant.Convertors;

namespace MyRestaurant.ViewModels
{
    public class ModifyPreparatViewModel : BasePropertyChanged
    {
        private string _name;
        private decimal _price;
        private int _quantityPerPortion;
        private int _totalQuantity;
        private int _selectedCategoryId;
        private string _errorMessage;
        private PreparateBLL _preparateBLL;
        private CategoryBLL _categoriiBLL;
        private Utilizatori _currentUser;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public string OldName { get; set; } // Store the old name for comparison

        public decimal Price
        {
            get => _price;
            set
            {
                _price = value;
                OnPropertyChanged();
            }
        }
        public int QuantityPerPortion
        {
            get => _quantityPerPortion;
            set
            {
                _quantityPerPortion = value;
                OnPropertyChanged();
            }
        }
        public int TotalQuantity
        {
            get => _totalQuantity;
            set
            {
                _totalQuantity = value;
                OnPropertyChanged();
            }
        }
        public int SelectedCategoryId
        {
            get => _selectedCategoryId;
            set
            {
                _selectedCategoryId = value;
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

        public ObservableCollection<Preparate> PreparateList
        {
            get => _preparateBLL.PreparateList;
            set => _preparateBLL.PreparateList = value;
        }

        public ObservableCollection<Categorii> CategoriList
        {
            get => _categoriiBLL.CategoriList;
            set => _categoriiBLL.CategoriList = value;
        }

        private Preparate _selectedPreparat;
        public Preparate SelectedPreparat
        {
            get => _selectedPreparat;
            set
            {
                _selectedPreparat = value;
                if (_selectedPreparat != null)
                {
                    OldName = _selectedPreparat.Denumire; // Store the old name for comparison
                }
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

        public ICommand AddPreparat { get; }

        public ICommand UpdatePreparat { get; }

        public ICommand BackToMain { get; }

        public ModifyPreparatViewModel(Utilizatori user)
        {
            CurrentUser = user;
            _preparateBLL = new PreparateBLL();
            PreparateList = _preparateBLL.GetAllPreparate();
            _categoriiBLL = new CategoryBLL();
            CategoriList = _categoriiBLL.GetAllCategori();
            AddPreparat = new RelayCommand<object>(Add);
            UpdatePreparat = new RelayCommand<object>(Update);
            BackToMain = new RelayCommand<object>(Back);
        }

        public void Add(object parameter)
        {
            if (!CategoriList.Any(p => p.Idcategorie == SelectedCategoryId))
            {
                MessageBox.Show("Categoria nu exista!", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (parameter is Preparate preparat)
            {
                _preparateBLL.AddMethode(preparat);
                ErrorMessage = _preparateBLL.ErrorMessage;
                if (!string.IsNullOrEmpty(ErrorMessage))
                {
                    MessageBox.Show(ErrorMessage, "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                PreparateList.Add(preparat);
                MessageBox.Show("Preparatul a fost adaugat cu success!", "Adaugare", MessageBoxButton.OK, MessageBoxImage.Information);
                Name = string.Empty; // Clear the input field after adding
                Price = 0; // Clear the input field after adding
                QuantityPerPortion = 0; // Clear the input field after adding
                TotalQuantity = 0; // Clear the input field after adding
                SelectedCategoryId = 0; // Clear the input field after adding
            }
        }

        public void Update(object parameter)
        {
            if (parameter is Preparate preparat)
            {
                _preparateBLL.UpdateMethode(preparat);
                ErrorMessage = _preparateBLL.ErrorMessage;
                if (!string.IsNullOrEmpty(ErrorMessage))
                {
                    MessageBox.Show(ErrorMessage, "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                MessageBox.Show("Preparatul a fost actualizat cu success!", "Actualizare", MessageBoxButton.OK, MessageBoxImage.Information);
                var itemToRemove = PreparateList.FirstOrDefault(c => c.Denumire.Equals(OldName, StringComparison.OrdinalIgnoreCase));
                PreparateList.Remove(itemToRemove);
                PreparateList.Add(preparat); // Add the updated category
                Name = string.Empty; // Clear input
                Price = 0;
                QuantityPerPortion = 0; // Clear input
                TotalQuantity = 0;
                SelectedCategoryId = 0; // Clear input
            }
        }

        public void DeleteMethode(object parameter)
        {
            if (parameter is Preparate preparat)
            {
                _preparateBLL.DeleteMethode(preparat);
                ErrorMessage = _preparateBLL.ErrorMessage;
                if (!string.IsNullOrEmpty(ErrorMessage))
                {
                    MessageBox.Show(ErrorMessage, "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                PreparateList.Remove(preparat); // Remove the category from the list
                MessageBox.Show("Preparatul a fost sters cu success!", "Stergere", MessageBoxButton.OK, MessageBoxImage.Information);
                Name = string.Empty; // Clear input
                Price = 0; // Clear input
                QuantityPerPortion = 0; // Clear input
                TotalQuantity = 0; // Clear input
            }
        }

        public void Back(object parameter)
        {
            var window = new MainWindow(CurrentUser);
        }
    }
}
