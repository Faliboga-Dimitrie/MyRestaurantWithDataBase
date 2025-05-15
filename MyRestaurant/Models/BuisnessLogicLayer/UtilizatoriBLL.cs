using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyRestaurant.Models.DataAcessLayer;

namespace MyRestaurant.Models.BuisnessLogicLayer
{
    public class UtilizatoriBLL
    {
        private MyRestaurantDAL context = new MyRestaurantDAL();

        public ObservableCollection<Utilizatori> UtilizatoriList { get; set; }

        public string ErrorMessage { get; set; }

        public void AddMethode(object obj)
        {
            Utilizatori utilizatori = obj as Utilizatori;
            if (utilizatori != null)
            {
                if(string.IsNullOrEmpty(utilizatori.Nume))
                {
                    ErrorMessage = "Numele nu poate fi gol!";
                }
                else if (string.IsNullOrEmpty(utilizatori.Prenume))
                {
                    ErrorMessage = "Prenumele nu poate fi gol!";
                }
                else if (string.IsNullOrEmpty(utilizatori.Email))
                {
                    ErrorMessage = "Emailul nu poate fi gol!";
                }
                else if (string.IsNullOrEmpty(utilizatori.Telefon))
                {
                    ErrorMessage = "Telefonul nu poate fi gol!";
                }
                else if (string.IsNullOrEmpty(utilizatori.AdresaLivrare))
                {
                    ErrorMessage = "Adresa de livrare nu poate fi goala!";
                }
                else if (string.IsNullOrEmpty(utilizatori.Parola))
                {
                    ErrorMessage = "Parola nu poate fi goala!";
                }
                else if(string.IsNullOrEmpty(utilizatori.TipUtilizator))
                {
                    ErrorMessage = "Tipul de utilizator nu poate fi gol!";
                }
                else if (context.Utilizatoris.Any(u => u.Email == utilizatori.Email))
                {
                    ErrorMessage = "Un utilizator cu acest email exista deja!";
                }
                else if (context.Utilizatoris.Any(u => u.Telefon == utilizatori.Telefon))
                {
                    ErrorMessage = "Un utilizator cu acest numar de telefon exista deja!";
                }
                else if (context.Utilizatoris.Any(u => u.Nume == utilizatori.Nume && u.Prenume == utilizatori.Prenume))
                {
                    ErrorMessage = "Un utilizator cu acest nume si prenume exista deja!";
                }
                else
                {
                    try
                    {
                        context.Utilizatoris.Add(utilizatori);
                        context.SaveChanges();
                        UtilizatoriList.Add(utilizatori);
                        ErrorMessage = string.Empty;
                    }
                    catch (Exception ex)
                    {
                        ErrorMessage = "A aparut o eroare la adaugarea utilizatorului: " + ex.Message;
                    }
                }
            }
        }

        public ObservableCollection<Utilizatori> GetAllUtilizatori()
        {
            return [.. context.Utilizatoris];
        }
    }
}
