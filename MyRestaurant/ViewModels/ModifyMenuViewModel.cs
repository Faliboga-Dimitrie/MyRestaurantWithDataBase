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
    public class ModifyMenuViewModel : BasePropertyChanged
    {
        private MeniuriBLL _meniuriBLL;
        private MeniuPreparatBLL _meniuPreparatBLL;
        private PreparateBLL _preparateBLL;
        private CategoryBLL _categoriiBLL;


        private Utilizatori _currentUser;
        public Utilizatori CurrentUser
        {
            get => _currentUser;
            set
            {
                _currentUser = value;
                OnPropertyChanged();
            }
        }

        private Meniuri _selectedMeniu;

        public Meniuri SelectedMeniu
        {
            get { return _selectedMeniu; }
            set
            {
                _selectedMeniu = value;
                OnPropertyChanged(nameof(SelectedMeniu));
                UpdatePreparateListForMenu();
            }
        }

        private Preparate _selectedPreparat;

        public Preparate SelectedPreparat
        {
            get { return _selectedPreparat; }
            set
            {
                _selectedPreparat = value;
                OnPropertyChanged(nameof(SelectedPreparat));
            }
        }

        private int _cantitateInMeniu;

        public int CantitateInMeniu
        {
            get { return _cantitateInMeniu; }
            set
            {
                _cantitateInMeniu = value;
                OnPropertyChanged(nameof(CantitateInMeniu));
            }
        }

        private string _denumireMeniu;

        public string DenumireMeniu
        {
            get { return _denumireMeniu; }
            set
            {
                _denumireMeniu = value;
                OnPropertyChanged(nameof(DenumireMeniu));
            }
        }

        private Categorii _selectedCategorie;

        public Categorii SelectedCategorie
        {
            get { return _selectedCategorie; }
            set
            {
                _selectedCategorie = value;
                OnPropertyChanged(nameof(SelectedCategorie));
            }
        }

        private string _errorMessage;

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        // Add these properties to support ID-based binding
        private int? _selectedCategoryId;
        public int? SelectedCategoryId
        {
            get => _selectedCategoryId;
            set
            {
                _selectedCategoryId = value;
                SelectedCategorie = CategoriList.FirstOrDefault(c => c.Idcategorie == value);
                OnPropertyChanged(nameof(SelectedCategoryId));
            }
        }

        private int? _selectedPreparatId;
        public int? SelectedPreparatId
        {
            get => _selectedPreparatId;
            set
            {
                _selectedPreparatId = value;
                SelectedPreparat = PreparateList.FirstOrDefault(p => p.Idpreparat == value);
                OnPropertyChanged(nameof(SelectedPreparatId));
            }
        }


        public ObservableCollection<Meniuri> MeniuriList
        {
            get => _meniuriBLL.MeniuriList;
            set
            {
                _meniuriBLL.MeniuriList = value;
                OnPropertyChanged(nameof(MeniuriList));
            }
        }

        public ObservableCollection<MeniuPreparat> MeniuPreparatList
        {
            get => _meniuPreparatBLL.MeniuPreparatList;
            set
            {
                _meniuPreparatBLL.MeniuPreparatList = value;
                OnPropertyChanged(nameof(MeniuPreparatList));
            }
        }

        private ObservableCollection<Preparate> _preparateListForMenu = new ObservableCollection<Preparate>();
        public ObservableCollection<Preparate> PreparateListForMenu
        {
            get => _preparateListForMenu;
            set
            {
                _preparateListForMenu = value;
                OnPropertyChanged(nameof(PreparateListForMenu));
            }
        }

        private void UpdatePreparateListForMenu()
        {
            if (SelectedMeniu == null)
            {
                PreparateListForMenu = new ObservableCollection<Preparate>();
                return;
            }

            var filtered = PreparateList
        .Where(p => MeniuPreparatList.Any(mp => mp.Idpreparat == p.Idpreparat 
        && mp.Idmeniu == SelectedMeniu.Idmeniu)).ToList();

            PreparateListForMenu = new ObservableCollection<Preparate>(filtered);
        }


        public ObservableCollection<Preparate> PreparateList
        {
            get => _preparateBLL.PreparateList;
            set
            {
                _preparateBLL.PreparateList = value;
                OnPropertyChanged(nameof(PreparateList));
            }
        }

        public ObservableCollection<Categorii> CategoriList
        {
            get => _categoriiBLL.CategoriList;
            set
            {
                _categoriiBLL.CategoriList = value;
                OnPropertyChanged(nameof(CategoriList));
            }
        }

        public ICommand AddMenu { get; }
        public ICommand UpdateMenu { get; }
        public ICommand DeleteMenu { get; }

        public ICommand AddPreparat { get; }
        public ICommand DeletePreparat { get; }

        public ICommand BackCommand { get; }

        public ModifyMenuViewModel(Utilizatori currentUser)
        {
            _meniuriBLL = new MeniuriBLL();
            _meniuPreparatBLL = new MeniuPreparatBLL();
            _preparateBLL = new PreparateBLL();
            _categoriiBLL = new CategoryBLL();
            MeniuriList = _meniuriBLL.GetMeniuriList();
            PreparateList = _preparateBLL.GetAllPreparate();
            CategoriList = _categoriiBLL.GetAllCategori();
            MeniuPreparatList = _meniuPreparatBLL.GetAllPreparates();
            CurrentUser = currentUser;

            AddMenu = new AsyncRelayCommand<object>(Add);
            UpdateMenu = new RelayCommand<object>(Update);
            DeleteMenu = new RelayCommand<object>(Delete);
            AddPreparat = new RelayCommand<object>(AddPreparatForMenu);
            DeletePreparat = new RelayCommand<object>(DeletePreparatForMenu);
            BackCommand = new RelayCommand<object>(Back);
        }

        public async Task Add(object param)
        {
            if (string.IsNullOrEmpty(DenumireMeniu))
            {
                ErrorMessage = "Numele meniului nu poate fi gol!";
            }
            else if (string.IsNullOrEmpty(SelectedCategorie.NumeCategorie))
            {
                ErrorMessage = "Selectati o categorie!";
            }
            else if (MeniuriList.Any(m => m.Denumire == DenumireMeniu))
            {
                ErrorMessage = "Numele meniului exista deja!";
            }

            if (param is Meniuri meniu)
            {
                await _meniuriBLL.AddMethodeAsync(meniu);
                ErrorMessage = _meniuriBLL.ErrorMessage;
                if (!string.IsNullOrEmpty(ErrorMessage))
                {
                    MessageBox.Show(ErrorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                DenumireMeniu = string.Empty;
                SelectedCategorie = null;
                ErrorMessage = string.Empty;
                MessageBox.Show("Meniul a fost adaugat cu succes!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public void Update(object param)
        {
            if (string.IsNullOrEmpty(DenumireMeniu))
            {
                ErrorMessage = "Numele meniului nu poate fi gol!";
            }
            else if (SelectedCategorie == null)
            {
                ErrorMessage = "Selectati o categorie!";
            }
            else if (MeniuriList.Any(m => m.Denumire == DenumireMeniu))
            {
                ErrorMessage = "Numele meniului exista deja!";
            }

            if (param is Meniuri meniu)
            {
                meniu.Idmeniu = SelectedMeniu.Idmeniu;
                _meniuriBLL.UpdateMethode(meniu);
                ErrorMessage = _meniuriBLL.ErrorMessage;
                if (!string.IsNullOrEmpty(ErrorMessage))
                {
                    MessageBox.Show(ErrorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                MeniuriList.Remove(SelectedMeniu);
                MeniuriList.Add(meniu);
                DenumireMeniu = string.Empty;
                SelectedCategorie = null;
                ErrorMessage = string.Empty;
                MessageBox.Show("Meniul a fost actualizat cu succes!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public void Delete(object param)
        {
            if (SelectedMeniu != null)
            {
                _meniuriBLL.DeleteMethode(SelectedMeniu);
                ErrorMessage = _meniuriBLL.ErrorMessage;
                if (!string.IsNullOrEmpty(ErrorMessage))
                {
                    MessageBox.Show(ErrorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                DenumireMeniu = string.Empty;
                SelectedCategorie = null;
                ErrorMessage = string.Empty;
                MessageBox.Show("Meniul a fost sters cu succes!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public void AddPreparatForMenu(object param)
        {
            if (SelectedMeniu == null)
            {
                ErrorMessage = "Selectati un meniu!";
            }
            else if (SelectedPreparat == null)
            {
                ErrorMessage = "Selectati un preparat!";
            }
            else if (CantitateInMeniu <= 0)
            {
                ErrorMessage = "Cantitatea in meniu trebuie sa fie mai mare decat 0!";
            }
            else if (MeniuPreparatList.Any(mp => mp.Idpreparat == SelectedPreparat.Idpreparat && mp.Idmeniu == SelectedMeniu.Idmeniu))
            {
                ErrorMessage = "Preparatul este deja adaugat in meniu!";
            }
            else
            {
                MeniuPreparat meniuPreparat = new MeniuPreparat
                {
                    Idmeniu = SelectedMeniu.Idmeniu,
                    Idpreparat = SelectedPreparatId.HasValue ? SelectedPreparatId.Value : SelectedPreparat.Idpreparat,
                    CantitateInMeniu = CantitateInMeniu
                };

                _meniuPreparatBLL.AddMethode(meniuPreparat);
                ErrorMessage = _meniuPreparatBLL.ErrorMessage;
                if (!string.IsNullOrEmpty(ErrorMessage))
                {
                    MessageBox.Show(ErrorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                CantitateInMeniu = 0;
                ErrorMessage = string.Empty;
                UpdatePreparateListForMenu();
                MessageBox.Show("Preparatul a fost adaugat in meniu cu succes!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public void DeletePreparatForMenu(object param)
        {
            if (SelectedMeniu == null)
            {
                ErrorMessage = "Selectati un meniu!";
            }
            else if (SelectedPreparat == null)
            {
                ErrorMessage = "Selectati un preparat!";
            }
            else
            {
                MeniuPreparat meniuPreparat = new MeniuPreparat
                {
                    Idmeniu = SelectedMeniu.Idmeniu,
                    Idpreparat = SelectedPreparatId.HasValue ? SelectedPreparatId.Value : SelectedPreparat.Idpreparat,
                    CantitateInMeniu = CantitateInMeniu
                };
                _meniuPreparatBLL.DeleteMethode(meniuPreparat);
                ErrorMessage = _meniuPreparatBLL.ErrorMessage;
                if (!string.IsNullOrEmpty(ErrorMessage))
                {
                    MessageBox.Show(ErrorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                CantitateInMeniu = 0;
                SelectedPreparat = null;
                ErrorMessage = string.Empty;
                UpdatePreparateListForMenu();
                MessageBox.Show("Preparatul a fost sters din meniu cu succes!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
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
