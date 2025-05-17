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
                else if (context.Alergenis.Any(a => a.NumeAlergen == alergen.NumeAlergen))
                {
                    ErrorMessage = "Un alergen cu acest nume exista deja!";
                }
                else
                {
                    context.Alergenis.Add(alergen);
                    context.SaveChanges();
                    AlergeniList.Add(alergen);
                    ErrorMessage = string.Empty; // Clear error message on success
                }
            }
        }

        public void UpdateMethode(object obj)
        {
            Alergeni alergen = obj as Alergeni;
            if (alergen != null)
            {
                if(string.IsNullOrEmpty(alergen.NumeAlergen))
                {
                    ErrorMessage = "Numele nu poate fi gol!";
                }
                else if (context.Alergenis.Any(a => a.NumeAlergen == alergen.NumeAlergen && a.Idalergen != alergen.Idalergen))
                {
                    ErrorMessage = "Un alergen cu acest nume exista deja!";
                }
                else
                {
                    try
                    {
                        context.UpdateAlergen(alergen.Idalergen, alergen.NumeAlergen);
                        ErrorMessage = string.Empty; // Clear error message on success
                    }
                    catch (Exception ex)
                    {
                        ErrorMessage = "A aparut o eroare la actualizarea alergenului: " + ex.Message;
                    }
                }
            }
        }

        public void DeleteMethode(object obj)
        {
            Alergeni alergen = obj as Alergeni;
            if (alergen != null)
            {
                try
                {
                    context.DeleteAlergen(alergen.Idalergen);
                    var itemToRemove = AlergeniList.FirstOrDefault(c => c.NumeAlergen.Equals(alergen.NumeAlergen, StringComparison.OrdinalIgnoreCase));
                    AlergeniList.Remove(itemToRemove);
                    ErrorMessage = string.Empty; // Clear error message on success
                }
                catch (Exception ex)
                {
                    ErrorMessage = "A aparut o eroare la stergerea alergenului: " + ex.Message;
                }
            }
        }

        public ObservableCollection<Alergeni> GetAllAlergeni()
        {
            return [.. context.Alergenis];
        }
    }
}
