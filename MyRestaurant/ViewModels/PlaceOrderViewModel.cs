using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyRestaurant.Models.BuisnessLogicLayer;
using MyRestaurant.Helpers;
using MyRestaurant.Models;
using System.Collections.ObjectModel;
using System.Security.RightsManagement;
using System.Windows.Input;
using System.Windows;
using MyRestaurant.Views;
using System.Configuration;
using System.Xml;

namespace MyRestaurant.ViewModels
{
    public class PlaceOrderViewModel : BasePropertyChanged
    {
        private ComenziBLL _comenziBLL;
        private ComandaPreparatBLL _comandaPreparatBLL;
        private ComandaMeniuBLL _comandaMeniuBLL;
        private PreparateBLL _preparateBLL;
        private MeniuriBLL _meniuriBLL;
        private readonly DiscountSettings _discountSettings;


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


        private Meniuri _selectedMeniu;

        public Meniuri SelectedMeniu
        {
            get => _selectedMeniu;
            set
            {
                _selectedMeniu = value;
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
                OnPropertyChanged();
            }
        }

        private int _selectedPreparatQuantity;

        public int SelectedPreparatQuantity
            {
            get => _selectedPreparatQuantity;
            set
            {
                _selectedPreparatQuantity = value;
                OnPropertyChanged();
            }
        }

        private int _selectedMeniuQuantity;

        public int SelectedMeniuQuantity
        {
            get => _selectedMeniuQuantity;
            set
            {
                _selectedMeniuQuantity = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Comenzi> ComenziList
        {
            get => _comenziBLL.ComenziList;
            set => _comenziBLL.ComenziList = value;
        }

        public ObservableCollection<ComandaPreparat> ComandaPreparatList
        {
            get => _comandaPreparatBLL.ComandaPreparatList;
            set => _comandaPreparatBLL.ComandaPreparatList = value;
        }

        public ObservableCollection<ComandaMeniu> ComandaMeniuList
        {
            get => _comandaMeniuBLL.ComandaMeniuList;
            set => _comandaMeniuBLL.ComandaMeniuList = value;
        }

        public ObservableCollection<Preparate> PreparateList
        {
            get => _preparateBLL.PreparateList;
            set => _preparateBLL.PreparateList = value;
        }

        public ObservableCollection<Meniuri> MeniuriList
        {
            get => _meniuriBLL.MeniuriList;
            set => _meniuriBLL.MeniuriList = value;
        }

        private ObservableCollection<ComandaPreparatWrapper> _orderedPreparate;
        public ObservableCollection<ComandaPreparatWrapper> OrderedPreparate 
        { 
            get => _orderedPreparate;
            set
            {
                _orderedPreparate = value;
                OnPropertyChanged();
            } 
        } 

        private ObservableCollection<ComandaMeniuWrapper> _orderedMeniuri;

        public ObservableCollection<ComandaMeniuWrapper> OrderedMeniuri
        {
            get => _orderedMeniuri;
            set
            {
                _orderedMeniuri = value;
                OnPropertyChanged();
            }
        }

        public ICommand PlaceOrder { get; }
        public ICommand AddPreparat { get; }
        public ICommand AddMeniu { get; }

        public ICommand RemovePreparat { get; }

        public ICommand RemoveMeniu { get; }
        public ICommand BackToMain { get; }

        public PlaceOrderViewModel(Utilizatori user)
        {
            _comenziBLL = new ComenziBLL();
            _comandaPreparatBLL = new ComandaPreparatBLL();
            _comandaMeniuBLL = new ComandaMeniuBLL();
            _preparateBLL = new PreparateBLL();
            _meniuriBLL = new MeniuriBLL();
            _discountSettings = new DiscountSettings();

            CurrentUser = user;

            OrderedPreparate = new ObservableCollection<ComandaPreparatWrapper>();
            OrderedMeniuri = new ObservableCollection<ComandaMeniuWrapper>();

            ComenziList = _comenziBLL.GetComenziList();
            ComandaPreparatList = _comandaPreparatBLL.GetComandaPreparatList();
            ComandaMeniuList = _comandaMeniuBLL.GetComandaMeniuList();
            PreparateList = _preparateBLL.GetAllPreparate();
            MeniuriList = _meniuriBLL.GetMeniuriList();

            PlaceOrder = new AsyncRelayCommand<object>(UserPlaceOrder);
            AddPreparat = new RelayCommand<object>(AddPreparatToOrder);
            AddMeniu = new RelayCommand<object>(AddMeniuToOrder);
            RemovePreparat = new RelayCommand<object>(RemovePreparatFromOrder);
            RemoveMeniu = new RelayCommand<object>(RemoveMeniuFromOrder);
            BackToMain = new RelayCommand<object>(BackToMainWindow);
        }

        public Preparate FindPreparateForWrapper(ComandaPreparatWrapper wrapper)
        {
            if (wrapper?.ComandaPreparat == null) return null;

            int id = wrapper.ComandaPreparat.Idpreparat;

            return PreparateList.FirstOrDefault(p => p.Idpreparat == id);
        }

        public CostCalculationResult CalculateFinalCost(Comenzi comanda, ObservableCollection<Comenzi> allOrders, Utilizatori currentUser)
        {
            var result = new CostCalculationResult();
            decimal cost = comanda.Cost;

            if (cost > _discountSettings.DiscountOrderThreshold)
            {
                decimal discountAmount = cost * (_discountSettings.DiscountPercent / 100);
                cost -= discountAmount;
                result.Messages.Add($"S-a aplicat o reducere de {_discountSettings.DiscountPercent}% pentru comenzi peste {_discountSettings.DiscountOrderThreshold} lei. Economisiți {discountAmount:C}.");
            }
            else if (cost < _discountSettings.FreeDeliveryThreshold)
            {
                cost += _discountSettings.DeliveryFee;
                result.Messages.Add($"Comanda dvs. este sub pragul de {_discountSettings.FreeDeliveryThreshold} lei, se adaugă taxa de livrare de {_discountSettings.DeliveryFee} lei.");
            }

            DateTime intervalStart = DateTime.Now - _discountSettings.OrderInterval;

            int recentOrdersCount = allOrders.Count(o =>
                o.Idutilizator == currentUser.Idutilizator &&
                o.DataComanda >= intervalStart);

            if (recentOrdersCount > _discountSettings.MaxOrdersInInterval)
            {
                decimal discountAmount = cost * (_discountSettings.DiscountPercent / 100);
                cost -= discountAmount;
                result.Messages.Add($"S-a aplicat o reducere suplimentară de {_discountSettings.DiscountPercent}% pentru mai mult de {_discountSettings.MaxOrdersInInterval} comenzi în ultimele {_discountSettings.OrderInterval.TotalMinutes} minute. Economisiți {discountAmount:C}.");
            }

            result.FinalCost = cost;
            return result;
        }

        public async Task UserPlaceOrder(object param)
        {
            if (OrderedPreparate.Count <= 0 && OrderedMeniuri.Count <= 0)
            {
                ErrorMessage = "Nu ati adaugat nimic in comanda!";
                return;
            }
            var comanda = new Comenzi
            {
                Idcomanda = 0,
                CodUnic = Guid.NewGuid(),
                Idutilizator = CurrentUser.Idutilizator,
                DataComanda = DateTime.Now,
                Stare = "Se pregateste",
                Cost = 0
            };
            try
            {
                await _comenziBLL.AddMethode(comanda);
                ErrorMessage = _comenziBLL.ErrorMessage;
                if (!string.IsNullOrEmpty(ErrorMessage))
                {
                    MessageBox.Show(ErrorMessage, "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                foreach(var preparat in OrderedPreparate)
                {
                    preparat.ComandaPreparat.Idcomanda = comanda.Idcomanda;
                    await _comandaPreparatBLL.AddMethodeAsync(preparat.ComandaPreparat);
                    ErrorMessage = _comandaPreparatBLL.ErrorMessage;
                    if (!string.IsNullOrEmpty(ErrorMessage))
                    {
                        MessageBox.Show(ErrorMessage, "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    var preparatToUpdate = FindPreparateForWrapper(preparat);
                    if (preparatToUpdate != null)
                    {
                        preparatToUpdate.CantitateTotala -= preparat.ComandaPreparat.Cantitate * preparatToUpdate.CantitatePortie;
                        _preparateBLL.UpdateMethode(preparatToUpdate,null);
                        ErrorMessage = _preparateBLL.ErrorMessage;
                        if (!string.IsNullOrEmpty(ErrorMessage))
                        {
                            MessageBox.Show(ErrorMessage, "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                    }
                }

                foreach (var meniu in OrderedMeniuri)
                {
                    meniu.ComandaMeniu.Idcomanda = comanda.Idcomanda;
                    await _comandaMeniuBLL.AddMethodeAsync(meniu.ComandaMeniu);
                    ErrorMessage = _comandaPreparatBLL.ErrorMessage;
                    if (!string.IsNullOrEmpty(ErrorMessage))
                    {
                        MessageBox.Show(ErrorMessage, "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }

                await _comenziBLL.CalculateCost(comanda);
                var calculationResult = CalculateFinalCost(comanda, ComenziList, CurrentUser);
                comanda.Cost = calculationResult.FinalCost;
                _comenziBLL.UpdateCost(comanda);

                MessageBox.Show("Comanda a fost plasata cu succes!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                OrderedPreparate.Clear();
                OrderedMeniuri.Clear();

                if (calculationResult.Messages.Any())
                {
                    string message = string.Join(Environment.NewLine, calculationResult.Messages);
                    MessageBox.Show(message, "Informații despre reducere", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        public void AddPreparatToOrder(object param)
        {
            if (SelectedPreparat != null && SelectedPreparatQuantity > 0)
            {

                if (SelectedPreparatQuantity > SelectedPreparat.CantitateTotala)
                {
                    ErrorMessage = "Cantitatea selectata depaseste stocul disponibil!";
                    return;
                }
                var comandaPreparat = new ComandaPreparat
                {
                    Idcomanda = 0,
                    Idpreparat = SelectedPreparat.Idpreparat,
                    Cantitate = SelectedPreparatQuantity
                };
                OrderedPreparate.Add(new ComandaPreparatWrapper(comandaPreparat, SelectedPreparat.Denumire));
                SelectedPreparatQuantity = 0;
                SelectedPreparat = null;
                ErrorMessage = string.Empty;
            }
            else
            {
                ErrorMessage = "Selectati un preparat si o cantitate valida!";
            }
        }

        public void AddMeniuToOrder(object param)
        {
            if (SelectedMeniu != null && SelectedMeniuQuantity > 0)
            {
                var comandaMeniu = new ComandaMeniu
                {
                    Idcomanda = 0,
                    Idmeniu = SelectedMeniu.Idmeniu,
                    Cantitate = SelectedMeniuQuantity
                };
                OrderedMeniuri.Add(new ComandaMeniuWrapper(comandaMeniu, SelectedMeniu.Denumire));
                SelectedMeniuQuantity = 0;
                SelectedMeniu = null;
                ErrorMessage = string.Empty;
            }
            else
            {
                ErrorMessage = "Selectati un meniu si o cantitate valida!";
            }
        }

        public void RemovePreparatFromOrder(object param)
        {
            if( param is ComandaPreparatWrapper preparatToRemove)
            {
                OrderedPreparate.Remove(preparatToRemove);
            }
        }

        public void RemoveMeniuFromOrder(object param)
        {
            if (param is ComandaMeniuWrapper meniuToRemove)
            {
                OrderedMeniuri.Remove(meniuToRemove);
            }
        }

        public void BackToMainWindow(object param)
        {
            Window window = new MainWindow(CurrentUser);
            window.Show();
            Application.Current.Windows[0]?.Close();
        }
    }
}
