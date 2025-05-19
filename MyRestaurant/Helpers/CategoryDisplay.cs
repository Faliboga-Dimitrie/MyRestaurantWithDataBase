using MyRestaurant.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MyRestaurant.Helpers
{
    public class CategoryDisplay : BasePropertyChanged
    {
        public string NumeCategorie { get; set; }
        public ObservableCollection<Preparate> Preparates { get; set; }
        public ObservableCollection<Meniuri> Meniuris { get; set; }

        private bool _showPreparates = true;
        public bool ShowPreparates
        {
            get => _showPreparates;
            set { _showPreparates = value; OnPropertyChanged(); }
        }

        private bool _showMeniuri = true;
        public bool ShowMeniuri
        {
            get => _showMeniuri;
            set { _showMeniuri = value; OnPropertyChanged(); }
        }

        public ICommand TogglePreparatesCommand => new RelayCommand<object>(_ => ShowPreparates = !ShowPreparates);
        public ICommand ToggleMeniuriCommand => new RelayCommand<object>(_ => ShowMeniuri = !ShowMeniuri);
    }
}
