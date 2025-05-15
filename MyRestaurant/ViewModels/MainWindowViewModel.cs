using MyRestaurant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyRestaurant.Models.BuisnessLogicLayer;
using MyRestaurant.Helpers;
using System.Windows.Input;

namespace MyRestaurant.ViewModels
{
    public class MainWindowViewModel : BasePropertyChanged
    {
        private UtilizatoriBLL _utilizatoriBLL;
        private Utilizatori _selectedUtilizator;
        object _currentView;

        public object CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public Utilizatori SelectedUtilizator
        {
            get => _selectedUtilizator;
            set
            {
                _selectedUtilizator = value;
                OnPropertyChanged();
            }
        }

        public bool IsEmployee
        {
            get => SelectedUtilizator.TipUtilizator == "Angajat";
        }

        public ICommand ShowViewMenuCommand { get; }
        public ICommand ShowPlaceOrderCommand { get; }
        public ICommand ShowMyOrdersCommand { get; }

        public ICommand ShowEditMenuCommand { get; }

        public ICommand ShowEditOrdersCommand { get; }

        public ICommand LogoutCommand { get; }

        public MainWindowViewModel(Utilizatori utilizatori)
        {
            _utilizatoriBLL = new UtilizatoriBLL();
            SelectedUtilizator = utilizatori;
            ShowViewMenuCommand = new RelayCommand<object>(ShowViewMenu);
            ShowPlaceOrderCommand = new RelayCommand<object>(ShowPlaceOrder);
            ShowMyOrdersCommand = new RelayCommand<object>(ShowMyOrders);
            ShowEditMenuCommand = new RelayCommand<object>(ShowEditMenu);
            ShowEditOrdersCommand = new RelayCommand<object>(ShowEditOrders);
            LogoutCommand = new RelayCommand<object>(Logout);
        }

        private void ShowViewMenu(object obj)
        {
            // Logic to show the view menu
        }

        private void ShowPlaceOrder(object obj)
        {
            // Logic to show the place order
        }

        private void ShowMyOrders(object obj)
        {
            // Logic to show the my orders
        }

        private void ShowEditMenu(object obj)
        {
            // Logic to show the edit menu
        }

        private void ShowEditOrders(object obj)
        {
            // Logic to show the edit orders
        }

        private void Logout(object obj)
        {
            // Logic to logout
        }
    }
}
