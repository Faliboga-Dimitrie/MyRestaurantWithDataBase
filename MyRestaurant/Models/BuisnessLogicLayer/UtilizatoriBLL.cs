using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRestaurant.Models.BuisnessLogicLayer
{
    public class UtilizatoriBLL
    {
        private MyRestaurantContext context = new MyRestaurantContext();

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
                else
                {
                    context.Utilizatoris.Add(utilizatori);
                    context.SaveChanges();
                    UtilizatoriList.Add(utilizatori);
                    ErrorMessage = string.Empty;
                }
            }
        }

        public void UpdateMethode(object obj)
        {
            Utilizatori utilizatori = obj as Utilizatori;
            if(utilizatori == null)
            {
                ErrorMessage = "Selecteaza un Utilizator!";
                return;
            }
            else if(string.IsNullOrEmpty(utilizatori.Nume))
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
            else
            {
                //to be implemented, waiting after MyRestaurantDAL
            }
        }
    }
}
