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

namespace MyRestaurant.ViewModels
{
    public class AddPreparatViewModel : BasePropertyChanged
    {
        private string _name;
        private decimal _price;
        private int _quantityPerPortion;
        private int _totalQuantity;
        private string _categoryName;
        private string _errorMessage;
        private PreparateBLL _preparateBLL;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }
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
        public string CategoryName
        {
            get => _categoryName;
            set
            {
                _categoryName = value;
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

        public ICommand AddPreparat { get; }

        public AddPreparatViewModel()
        {
            _preparateBLL = new PreparateBLL();
            PreparateList = _preparateBLL.GetAllPreparate();
            AddPreparat = new RelayCommand<object>(Add);
        }

        public void Add(object parameter)
        {
            //if(CategoryName)

            if(parameter is Preparate preparat)
            {
                _preparateBLL.AddMethode(preparat);
                ErrorMessage = _preparateBLL.ErrorMessage;
                if(!string.IsNullOrEmpty(ErrorMessage))
                {
                    MessageBox.Show(ErrorMessage, "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                MessageBox.Show("Preparatul a fost adaugat cu success!", "Adaugare", MessageBoxButton.OK, MessageBoxImage.Information);

            }
        }
    }
}
