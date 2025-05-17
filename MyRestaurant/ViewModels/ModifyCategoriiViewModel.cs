using MyRestaurant.Helpers;
using MyRestaurant.Models;
using MyRestaurant.Models.BuisnessLogicLayer;
using MyRestaurant.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MyRestaurant.ViewModels
{
    public class ModifyCategoriiViewModel : BasePropertyChanged
    {
        private string _categorieName;
        private string _errorMessage;
        private CategoryBLL _categoriiBLL;
        private Utilizatori _currentUser;
        public string CategorieName
        {
            get => _categorieName;
            set
            {
                _categorieName = value;
                OnPropertyChanged();
            }
        }

        public string OldName { get; set; } // Store the old name for comparison

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

        public ObservableCollection<Categorii> CategoriList
        {
            get => _categoriiBLL.CategoriList; // Assuming CategoriList is a property in CategoryBLL
            set => _categoriiBLL.CategoriList = value; // Assuming CategoriList is a property in CategoryBLL
        }

        private Categorii _selectedCategory;
        public Categorii SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;
                OnPropertyChanged();

                OldName = _selectedCategory?.NumeCategorie; // Store the old name when a category is selected
            }
        }


        public ICommand BackToMain { get; }
        public ICommand AddCategorieCommand { get; }
        public ICommand UpdateCategorieCommand { get; }
        public ICommand DeleteCategorieCommand { get; }


        public ModifyCategoriiViewModel(Utilizatori user)
        {
            _categoriiBLL = new CategoryBLL();
            CurrentUser = user;
            CategoriList = _categoriiBLL.GetAllCategori();
            BackToMain = new RelayCommand<object>(Back);
            AddCategorieCommand = new RelayCommand<object>(AddCategorie);
            UpdateCategorieCommand = new RelayCommand<object>(UpdateCategorie);
            DeleteCategorieCommand = new RelayCommand<object>(DeleteCategorie);
        }
        public void AddCategorie(object param)
        {
            if (string.IsNullOrEmpty(CategorieName))
            {
                ErrorMessage = "Please enter a category name.";
                return;
            }
            else if (CategoriList.Any(c => c.NumeCategorie == CategorieName))
            {
                ErrorMessage = "A category with this name already exists.";
                return;
            }

            if (param is Categorii categorie)
            {
                _categoriiBLL.AddMethode(categorie);
                ErrorMessage = _categoriiBLL.ErrorMessage;
                if (!string.IsNullOrEmpty(ErrorMessage))
                {
                    MessageBox.Show(ErrorMessage, "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                CategorieName = string.Empty; // Clear the input field after adding
                MessageBox.Show("Category added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public void UpdateCategorie(object param)
        {
            if (string.IsNullOrEmpty(CategorieName))
            {
                ErrorMessage = "Please enter a category name.";
                return;
            }

            // Check if any other category has the new name (excluding the old name)
            if (CategoriList.Any(c => c.NumeCategorie.Equals(CategorieName, StringComparison.OrdinalIgnoreCase)
                                     && !c.NumeCategorie.Equals(OldName, StringComparison.OrdinalIgnoreCase)))
            {
                ErrorMessage = "A category with this name already exists.";
                return;
            }

            if (param is Categorii categorie)
            {
                // Update the category's name with the new name
                categorie.Idcategorie = SelectedCategory.Idcategorie; // Ensure the ID is set correctly

                _categoriiBLL.UpdateMethode(categorie);
                ErrorMessage = _categoriiBLL.ErrorMessage;
                if (!string.IsNullOrEmpty(ErrorMessage))
                {
                    MessageBox.Show(ErrorMessage, "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Update the category in the ObservableCollection
                var itemToRemove = CategoriList.FirstOrDefault(c => c.NumeCategorie.Equals(OldName, StringComparison.OrdinalIgnoreCase));
                CategoriList.Remove(itemToRemove);
                CategoriList.Add(categorie); // Add the updated category
                CategorieName = string.Empty; // Clear input
                OldName = string.Empty;       // Clear old name
                MessageBox.Show("Category updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }


        public void DeleteCategorie(object param)
        {
            if (param is Categorii categorie)
            {
                categorie.Idcategorie = SelectedCategory.Idcategorie; // Ensure the ID is set correctly
                _categoriiBLL.DeleteMethode(categorie);
                ErrorMessage = _categoriiBLL.ErrorMessage;
                if (!string.IsNullOrEmpty(ErrorMessage))
                {
                    MessageBox.Show(ErrorMessage, "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                MessageBox.Show("Category deleted successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                CategorieName = string.Empty; // Clear input
                OldName = string.Empty;
            }
        }

        public void Back(object parameter)
        {
            var window = new MainWindow(CurrentUser);
            window.Show();
            Application.Current.Windows[0]?.Close();
        }
    }
}
