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
using System.IO;

namespace MyRestaurant.ViewModels
{
    public class SelectableAlergen : BasePropertyChanged
    {
        public Alergeni Alergen { get; set; }

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                OnPropertyChanged();
            }
        }
    }


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
        private AlergeniBLL _alergeniBLL;
        private FotografiBLL _fotografiBLL;
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

        public ObservableCollection<Alergeni> AlergeniList
        {
            get => _alergeniBLL.AlergeniList;
            set
            {
                _alergeniBLL.AlergeniList = value;
                LoadSelectableAlergeni();
            }  
        }

        public ObservableCollection<Fotografi> FotografiList
        {
            get => _fotografiBLL.FotografiList;
            set
            {
                _fotografiBLL.FotografiList = value;
                FilterFotografii(); // Call the filter method when the list is set
            }
        }

        private ObservableCollection<Fotografi> _filteredFotografii;
        public ObservableCollection<Fotografi> FilteredFotografii
        {
            get => _filteredFotografii;
            set
            {
                _filteredFotografii = value;
                OnPropertyChanged();
            }
        }

        private void FilterFotografii()
        {
            if (SelectedPreparat == null)
            {
                FilteredFotografii = new ObservableCollection<Fotografi>();
            }
            else
            {
                FilteredFotografii = new ObservableCollection<Fotografi>(
                    FotografiList.Where(f => f.Idpreparat == SelectedPreparat.Idpreparat));
            }
        }

        private Alergeni _selectedAlergen;

        public Alergeni SelectedAlergen
        {
            get => _selectedAlergen;
            set
            {
                _selectedAlergen = value;
                OnPropertyChanged();
            }
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
                    Name = _selectedPreparat.Denumire;
                    Price = _selectedPreparat.Pret;
                    QuantityPerPortion = _selectedPreparat.CantitatePortie;
                    TotalQuantity = _selectedPreparat.CantitateTotala;
                    SelectedCategoryId = _selectedPreparat.Idcategorie;
                    // Load the selected alergeni for the selected preparat
                    SelectedAlergeni = AlergeniList.Where(a => _selectedPreparat.Idalergens.Any(sa => sa.Idalergen == a.Idalergen)).ToList();
                    // Load the selectable alergeni based on the selected preparat
                    LoadSelectableAlergeni();
                    FilterFotografii(); // Filter the fotografii based on the selected preparat
                }
                else
                {
                    Name = string.Empty;
                    Price = 0;
                    QuantityPerPortion = 0;
                    TotalQuantity = 0;
                    SelectedCategoryId = 0;
                    if(SelectableAlergeni != null)
                    {
                        foreach (var selectableAlergen in SelectableAlergeni)
                        {
                            selectableAlergen.IsSelected = false;
                        }
                        OnPropertyChanged(nameof(SelectableAlergeni));
                    }
                }
                OnPropertyChanged();
            }
        }

        private List<Alergeni> _selectedAlergeni = new List<Alergeni>();

        public List<Alergeni> SelectedAlergeni
        {
            get => _selectedAlergeni;
            set
            {
                _selectedAlergeni = value;
                OnPropertyChanged();
                LoadSelectableAlergeni();
            }
        }

        public ObservableCollection<SelectableAlergen> SelectableAlergeni { get; set; }

        public void LoadSelectableAlergeni()
        {
            SelectableAlergeni = new ObservableCollection<SelectableAlergen>(
                AlergeniList.Select(a => new SelectableAlergen
                {
                    Alergen = a,
                    IsSelected = SelectedAlergeni.Any(sa => sa.Idalergen == a.Idalergen)
                }));
            OnPropertyChanged(nameof(SelectableAlergeni));
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

        private Fotografi _selectedFotografie;
        public Fotografi SelectedFotografie
        {
            get => _selectedFotografie;
            set { _selectedFotografie = value; OnPropertyChanged(nameof(SelectedFotografie)); }
        }

        public ICommand AddFotografieCommand { get; }
        public ICommand DeleteFotografieCommand { get; }

        public ICommand AddPreparat { get; }

        public ICommand UpdatePreparat { get; }

        public ICommand BackToMain { get; }

        public ICommand DeletePreparat { get; }

        public ICommand DeleteAlergen { get; }

        public ICommand ClearInput { get; }

        public ModifyPreparatViewModel(Utilizatori user)
        {
            CurrentUser = user;

            _preparateBLL = new PreparateBLL();
            PreparateList = _preparateBLL.GetAllPreparate();
            _categoriiBLL = new CategoryBLL();
            CategoriList = _categoriiBLL.GetAllCategori();
            _alergeniBLL = new AlergeniBLL();
            AlergeniList = _alergeniBLL.GetAllAlergeni();
            _fotografiBLL = new FotografiBLL();
            FotografiList = _fotografiBLL.GetAllFotografis();

            AddPreparat = new RelayCommand<object>(Add);
            UpdatePreparat = new RelayCommand<object>(Update);
            DeletePreparat = new RelayCommand<object>(DeleteMethode);
            DeleteAlergen = new RelayCommand<object>(DeletAlergen);
            ClearInput = new RelayCommand<object>(ClerInput);
            AddFotografieCommand = new RelayCommand<object>(_ => OnAddFotografie(), _ => CanAddFotografie());
            DeleteFotografieCommand = new RelayCommand<object>(_ => OnDeleteFotografie(), _ => CanDeleteFotografie());
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
                SelectedAlergeni = SelectableAlergeni
                .Where(sa => sa.IsSelected)
                .Select(sa => sa.Alergen)
                .ToList();
                _preparateBLL.AddMethode(preparat, SelectedAlergeni);
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
                SelectedAlergeni = SelectableAlergeni
                .Where(sa => sa.IsSelected)
                .Select(sa => sa.Alergen)
                .ToList();
                preparat.Idpreparat = SelectedPreparat.Idpreparat; // Ensure the ID is set correctly
                _preparateBLL.UpdateMethode(preparat, SelectedAlergeni);
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
                SelectedPreparat = null; // Clear selected preparat
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

        public void DeletAlergen(object parameter)
        {
            if (parameter is Alergeni alergen && SelectedPreparat!= null)
            {
                alergen.Idalergen = AlergeniList.FirstOrDefault(a => a.NumeAlergen.Equals(alergen.NumeAlergen, StringComparison.OrdinalIgnoreCase)).Idalergen; // Ensure the ID is set correctly
                _preparateBLL.RemoveAlergenFromPreparat(SelectedPreparat.Idpreparat, alergen.Idalergen);
                ErrorMessage = _preparateBLL.ErrorMessage;
                if (!string.IsNullOrEmpty(ErrorMessage))
                {
                    MessageBox.Show(ErrorMessage, "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                var itemToRemove = SelectableAlergeni.FirstOrDefault(c => c.Alergen.NumeAlergen.Equals(alergen.NumeAlergen, StringComparison.OrdinalIgnoreCase));
                SelectableAlergeni.Remove(itemToRemove); // Remove the category from the list
                MessageBox.Show("Alergenul a fost sters cu success!", "Stergere", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public void ClerInput(object parameter)
        {
            if(SelectedPreparat != null)
            {
                SelectedPreparat = null; // Clear selected preparat
            }
        }

        private bool CanAddFotografie()
        {
            return SelectedPreparat != null;
        }

        private void OnAddFotografie()
        {
            var dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "Image files (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg|All files (*.*)|*.*";

            if (dlg.ShowDialog() == true)
            {
                byte[] imageBytes = File.ReadAllBytes(dlg.FileName);

                var fotografie = new Fotografi
                {
                    Idpreparat = SelectedPreparat.Idpreparat,
                    Fotografie = imageBytes
                };

                _fotografiBLL.AddMethode(fotografie);

                if (!string.IsNullOrEmpty(_fotografiBLL.ErrorMessage))
                {
                    MessageBox.Show(_fotografiBLL.ErrorMessage, "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    // Refresh the SelectedPreparat's Fotografis collection
                    SelectedPreparat.Fotografis.Add(fotografie);
                    OnPropertyChanged(nameof(SelectedPreparat));
                }
            }
        }

        private bool CanDeleteFotografie()
        {
            return SelectedFotografie != null;
        }

        private void OnDeleteFotografie()
        {
            if (SelectedFotografie == null) return;

            try
            {
                _fotografiBLL.DeleteMethode(SelectedFotografie);
                SelectedPreparat.Fotografis.Remove(SelectedFotografie);
                SelectedFotografie = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ErrorMessage, "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
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
