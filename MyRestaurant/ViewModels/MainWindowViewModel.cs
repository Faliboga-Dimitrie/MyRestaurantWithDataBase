using MyRestaurant.Helpers;
using MyRestaurant.Models;
using MyRestaurant.Models.BuisnessLogicLayer;
using MyRestaurant.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MyRestaurant.ViewModels
{
    public class MainWindowViewModel : BasePropertyChanged
    {
        private UtilizatoriBLL _utilizatoriBLL;
        private Utilizatori _selectedUtilizator;

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
        public ICommand ShowOrdersCommand { get; }

        public ICommand LogoutCommand { get; }

        public ICommand ModifyPreparatCommand { get; }

        public ICommand ModifyAlergeniCommand { get; }

        public ICommand ModifyCategorieCommand { get; }

        public ICommand ModifyMenuCommand { get; }

        public MainWindowViewModel(Utilizatori utilizatori)
        {
            _utilizatoriBLL = new UtilizatoriBLL();
            SelectedUtilizator = utilizatori;
            ShowViewMenuCommand = new RelayCommand<object>(ShowViewMenu);
            ShowPlaceOrderCommand = new RelayCommand<object>(ShowPlaceOrder);
            ShowOrdersCommand = new RelayCommand<object>(ShowMyOrders);
            LogoutCommand = new RelayCommand<object>(Logout);
            ModifyPreparatCommand = new RelayCommand<object>(ShowModifyPreparat);
            ModifyAlergeniCommand = new RelayCommand<object>(ShowModifyAlergeni);
            ModifyCategorieCommand = new RelayCommand<object>(ShowModifyCategorie);
            ModifyMenuCommand = new RelayCommand<object>(ShowModifyMenu);
        }

        private void ShowViewMenu(object obj)
        {
            Window window = new MenuWindow(_selectedUtilizator);
            window.Show();
            Application.Current.Windows[0]?.Close();
        }

        private void ShowPlaceOrder(object obj)
        {
            Window window = new PlaceOrderView(_selectedUtilizator);
            window.Show();
            Application.Current.Windows[0]?.Close();
        }

        private void ShowMyOrders(object obj)
        {
            Window window = new ViewOrdersView(_selectedUtilizator);
            window.Show();
            Application.Current.Windows[0]?.Close();
        }

        private void Logout(object obj)
        {
            _selectedUtilizator = null;
            var window = new StartWindow();
            window.Show();
            Application.Current.Windows[0]?.Close();
        }

        public void ShowModifyPreparat(object obj)
        {
            var window = new ModifyPreparatWindow(_selectedUtilizator);
            window.Show();
            Application.Current.Windows[0]?.Close();
        }

        public void ShowModifyAlergeni(object obj)
        {
            var window = new ModifyAlergeniView(_selectedUtilizator);
            window.Show();
            Application.Current.Windows[0]?.Close();
        }

        public void ShowModifyCategorie(object obj)
        {
            var window = new ModifyCategoriiWindow(_selectedUtilizator);
            window.Show();
            Application.Current.Windows[0]?.Close();
        }

        public void ShowModifyMenu(object obj)
        {
            var window = new ModifyMenuView(_selectedUtilizator);
            window.Show();
            Application.Current.Windows[0]?.Close();
        }
    }
}
