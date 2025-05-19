using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyRestaurant.Helpers;
using MyRestaurant.Models.BuisnessLogicLayer;
using MyRestaurant.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;
using MyRestaurant.Views;
using System.Windows;

namespace MyRestaurant.ViewModels
{
    public class DisplayAndOrModifyOrdersVM : BasePropertyChanged
    {
        private string _errorMessage;
        private ComenziBLL _comenziBLL;
        private PreparateBLL _preparateBLL;
        private ComandaPreparatBLL _comandaPreparatBLL;
        private MeniuriBLL _meniuriBLL;
        private ComandaMeniuBLL _comandaMeniuBLL;
        private UtilizatoriBLL _utilizatoriBLL;
        private Utilizatori _currentUser;
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
        private ObservableCollection<Comenzi> _allOrders;
        private ObservableCollection<Comenzi> _filteredOrders;
        public ObservableCollection<Comenzi> OrdersList
        {
            get => _filteredOrders;
            set
            {
                _filteredOrders = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Utilizatori> UsersList
        {
            get => _utilizatoriBLL.UtilizatoriList;
            set => _utilizatoriBLL.UtilizatoriList = value;
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

        public ObservableCollection<string> OrderStatuses { get; set; } = new ObservableCollection<string>
        {
            "Anulata",
            "Livrata",
            "A plecat",
            "Se pregateste",
            "Inregistrata"
        };

        private string _selectedOrderStatus;
        public string SelectedOrderStatus
        {
            get => _selectedOrderStatus;
            set
            {
                _selectedOrderStatus = value;
                OnPropertyChanged();
            }
        }

        private Comenzi _selectedOrder;

        public Comenzi SelectedOrder
        {
            get => _selectedOrder;
            set
            {
                _selectedOrder = value;
                OnPropertyChanged();
            }
        }

        public bool IsEmployee
        {
            get => CurrentUser.TipUtilizator == "Angajat";
        }

        public ICommand ChangeOrderStatus { get; }
        public ICommand BackToMain { get; }

        public DisplayAndOrModifyOrdersVM(Utilizatori user)
        {
            _comenziBLL = new ComenziBLL();
            _preparateBLL = new PreparateBLL();
            _comandaPreparatBLL = new ComandaPreparatBLL();
            _meniuriBLL = new MeniuriBLL();
            _comandaMeniuBLL = new ComandaMeniuBLL();
            _utilizatoriBLL = new UtilizatoriBLL();

            CurrentUser = user;

            _allOrders = _comenziBLL.GetComenziListAllData();
            ApplyOrderFilter();
            PreparateList = _preparateBLL.GetAllPreparate();
            MeniuriList = _meniuriBLL.GetMeniuriList();
            ComandaPreparatList = _comandaPreparatBLL.GetComandaPreparatList();
            ComandaMeniuList = _comandaMeniuBLL.GetComandaMeniuList();
            UsersList = _utilizatoriBLL.GetAllUtilizatori();

            ChangeOrderStatus = new RelayCommand<object>(ChangeOrderStatusMethod);
            BackToMain = new RelayCommand<object>(Back);
        }

        private void ApplyOrderFilter()
        {
            if (IsEmployee)
            {
                OrdersList = new ObservableCollection<Comenzi>(_allOrders);
            }
            else
            {
                OrdersList = new ObservableCollection<Comenzi>(
                    _allOrders.Where(order => order.Idutilizator == CurrentUser.Idutilizator));
            }
        }

        public void ChangeOrderStatusMethod(object param)
        {
            if (SelectedOrder != null)
            {
                if (SelectedOrderStatus != null)
                {
                    if (SelectedOrder.Idcomanda < 0)
                    {
                        ErrorMessage = "Selectati o comanda valida!";
                    }
                    else
                    {
                        try
                        {
                            SelectedOrder.Stare = SelectedOrderStatus;
                            _comenziBLL.UpdateStatus(SelectedOrder);
                            OrdersList = _comenziBLL.GetComenziList();
                            ErrorMessage = string.Empty;

                            _allOrders = _comenziBLL.GetComenziList();
                            ApplyOrderFilter();
                            SelectedOrder = null; // Reset selected order after updating\
                            SelectedOrderStatus = null; // Reset selected status after updating
                        }
                        catch (Exception ex)
                        {
                            ErrorMessage = ex.Message;
                        }
                    }
                }
                else
                {
                    ErrorMessage = "Selectati un status!";
                }
            }
            else
            {
                ErrorMessage = "Selectati o comanda!";
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