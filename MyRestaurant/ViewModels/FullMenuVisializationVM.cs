using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyRestaurant.Models;
using MyRestaurant.Models.BuisnessLogicLayer;
using MyRestaurant.Views;
using MyRestaurant.Helpers;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows;


namespace MyRestaurant.ViewModels
{
    public class FullMenuVisializationVM : BasePropertyChanged
    {
        private PreparateBLL _preparateBLL;
        private MeniuPreparatBLL _meniuPreparatBLL;
        private CategoryBLL _categoryBLL;
        private AlergeniBLL _alergeniBLL;
        private FotografiBLL _fotografiBLL;
        private MeniuriBLL _meniuriBLL;


        private Utilizatori? _currentUser;

        public Utilizatori CurrentUser
        {
            get => _currentUser;
            set
            {
                _currentUser = value;
                OnPropertyChanged();
            }
        }

        private string _errorMessage;

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged();
            }
        }

        private string _searchByName;
        public string SearchByName
        {
            get => _searchByName;
            set
            {
                _searchByName = value;
                OnPropertyChanged();
            }
        }

        private string _searchByAlergen;

        public string SearchByAlergen
        {
            get => _searchByAlergen;
            set
            {
                _searchByAlergen = value;
                OnPropertyChanged();
            }
        }

        private bool _showPreparates;
        public bool ShowPreparates
        {
            get => _showPreparates;
            set { _showPreparates = value; OnPropertyChanged(); }
        }

        private bool _showMeniuri;
        public bool ShowMeniuri
        {
            get => _showMeniuri;
            set { _showMeniuri = value; OnPropertyChanged(); }
        }

        public ObservableCollection<Preparate> PreparateList
        {
            get => _preparateBLL.PreparateList;
            set => _preparateBLL.PreparateList = value;
        }

        public ObservableCollection<MeniuPreparat> MeniuPreparatList
        {
            get => _meniuPreparatBLL.MeniuPreparatList;
            set => _meniuPreparatBLL.MeniuPreparatList = value;
        }

        public ObservableCollection<Categorii> CategoriList
        {
            get => _categoryBLL.CategoriList;
            set => _categoryBLL.CategoriList = value;
        }

        public ObservableCollection<Alergeni> AlergeniList
        {
            get => _alergeniBLL.AlergeniList;
            set => _alergeniBLL.AlergeniList = value;
        }

        public ObservableCollection<Fotografi> FotografiList
        {
            get => _fotografiBLL.FotografiList;
            set => _fotografiBLL.FotografiList = value;
        }

        public ObservableCollection<Meniuri> MeniuriList
        {
            get => _meniuriBLL.MeniuriList;
            set => _meniuriBLL.MeniuriList = value;
        }

        public ObservableCollection<CategoryDisplay> FilteredCategoriList { get; set; } = new();

        public ICommand Back { get; }

        public ICommand FilterCommand { get; }

        public ICommand TogglePreparatesCommand;
        public ICommand ToggleMeniuriCommand;

        public FullMenuVisializationVM(Utilizatori? currentUser)
        {
            _preparateBLL = new PreparateBLL();
            _meniuPreparatBLL = new MeniuPreparatBLL();
            _categoryBLL = new CategoryBLL();
            _alergeniBLL = new AlergeniBLL();
            _fotografiBLL = new FotografiBLL();
            _meniuriBLL = new MeniuriBLL();

            PreparateList = _preparateBLL.GetAllPreparate();
            MeniuPreparatList = _meniuPreparatBLL.GetAllPreparates();
            CategoriList = _categoryBLL.GetAllCategoriFullData();
            AlergeniList = _alergeniBLL.GetAllAlergeni();
            FotografiList = _fotografiBLL.GetAllFotografis();
            MeniuriList = _meniuriBLL.GetMeniuriListFullData();

            CurrentUser = currentUser;
            ShowMeniuri = true;
            ShowPreparates = true;
            Back = new RelayCommand<object>(BackToMainWindow);
            FilterCommand = new RelayCommand<object>(_ => ApplyFilters());
            ApplyFilters();
            TogglePreparatesCommand = new RelayCommand<object>(ChangePreparateVisibility);
            ToggleMeniuriCommand = new RelayCommand<object>(ChangeMeniuriVisibility);
        }

        private void ApplyFilters()
        {
            FilteredCategoriList.Clear();

            foreach (var categorie in CategoriList)
            {
                var preparate = categorie.Preparates?.ToList() ?? new();
                var meniuri = categorie.Meniuris?.ToList() ?? new();

                // Filter Preparate
                var filteredPreparate = preparate.Where(p =>
                {
                    bool nameMatch = string.IsNullOrWhiteSpace(SearchByName) ||
                                     p.Denumire.Contains(SearchByName, StringComparison.OrdinalIgnoreCase);
                    bool alergenMatch = true;

                    if (!string.IsNullOrWhiteSpace(SearchByAlergen))
                    {
                        alergenMatch = !p.Idalergens.Any(a =>
                            a.NumeAlergen.Contains(SearchByAlergen, StringComparison.OrdinalIgnoreCase));
                    }

                    return nameMatch && alergenMatch;
                }).ToList();

                // Filter Meniuri
                var filteredMeniuri = meniuri.Where(m =>
                {
                    // Check if menu name matches or any preparat name matches SearchByName
                    bool nameMatch = string.IsNullOrWhiteSpace(SearchByName) ||
                                     m.Denumire.Contains(SearchByName, StringComparison.OrdinalIgnoreCase) ||
                                     m.MeniuPreparats.Any(mp =>
                                         mp.IdpreparatNavigation.Denumire.Contains(SearchByName, StringComparison.OrdinalIgnoreCase));

                    bool alergenMatch = true;

                    if (!string.IsNullOrWhiteSpace(SearchByAlergen))
                    {
                        // Exclude menus that have any preparat containing the allergen
                        alergenMatch = !m.MeniuPreparats.Any(mp =>
                            mp.IdpreparatNavigation.Idalergens.Any(a =>
                                a.NumeAlergen.Contains(SearchByAlergen, StringComparison.OrdinalIgnoreCase)));
                    }

                    return nameMatch && alergenMatch;
                }).ToList();


                if (filteredPreparate.Any() || filteredMeniuri.Any())
                {
                    FilteredCategoriList.Add(new CategoryDisplay
                    {
                        NumeCategorie = categorie.NumeCategorie,
                        Preparates = new ObservableCollection<Preparate>(filteredPreparate),
                        Meniuris = new ObservableCollection<Meniuri>(filteredMeniuri)
                    });
                }
            }

            OnPropertyChanged(nameof(FilteredCategoriList));
        }

        public void ChangePreparateVisibility(object param)
        {
            ShowPreparates = !ShowPreparates;
            OnPropertyChanged(nameof(ShowPreparates));
        }

        public void ChangeMeniuriVisibility(object param)
        {
            ShowMeniuri = !ShowMeniuri;
            OnPropertyChanged(nameof(ShowMeniuri));
        }
        public void BackToMainWindow(object param)
        {
            Window mainWindow = CurrentUser != null ? (Window)new MainWindow(CurrentUser) : new StartWindow();
            mainWindow.Show();
            Application.Current.Windows[0]?.Close();
        }
    }
}
