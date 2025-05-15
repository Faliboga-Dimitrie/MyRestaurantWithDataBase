using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyRestaurant.Models.DataAcessLayer;

namespace MyRestaurant.Models.BuisnessLogicLayer
{
    public class AlergeniBLL
    {
        private MyRestaurantDAL context = new MyRestaurantDAL();

        public ObservableCollection<Alergeni> AlergeniList { get; set; }
        public string ErrorMessage { get; set; }
        public void AddMethode(object obj)
        {
            Alergeni alergen = obj as Alergeni;
            if (alergen != null)
            {
                if (string.IsNullOrEmpty(alergen.NumeAlergen))
                {
                    ErrorMessage = "Numele nu poate fi gol!";
                }
                //else if (string.IsNullOrEmpty(alergen.Descriere))
                //{
                //    ErrorMessage = "Descrierea nu poate fi goala!";
                //}
                //else if (context.Alergenis.Any(a => a.Nume == alergen.Nume))
                //{
                //    ErrorMessage = "Un alergen cu acest nume exista deja!";
                //}
            }
        }
    }
}
