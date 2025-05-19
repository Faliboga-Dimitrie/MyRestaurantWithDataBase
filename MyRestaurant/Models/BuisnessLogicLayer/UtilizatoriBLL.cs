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
            if (utilizatori == null)
            {
                ErrorMessage = "Datele utilizatorului sunt invalide!";
                return;
            }

            var validationErrors = ValidateUtilizatori(utilizatori);

            if (validationErrors.Any())
            {
                // Join all errors into a single string separated by new lines
                ErrorMessage = string.Join(Environment.NewLine, validationErrors);
                return;
            }

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


        public ObservableCollection<Utilizatori> GetAllUtilizatori()
        {
            return [.. context.Utilizatoris];
        }

        private List<string> ValidateUtilizatori(Utilizatori utilizatori)
        {
            var errors = new List<string>();

            // Basic required field checks
            if (string.IsNullOrWhiteSpace(utilizatori.Nume))
                errors.Add("Numele nu poate fi gol!");
            if (string.IsNullOrWhiteSpace(utilizatori.Prenume))
                errors.Add("Prenumele nu poate fi gol!");
            if (string.IsNullOrWhiteSpace(utilizatori.Email))
                errors.Add("Emailul nu poate fi gol!");
            if (string.IsNullOrWhiteSpace(utilizatori.Telefon))
                errors.Add("Telefonul nu poate fi gol!");
            if (string.IsNullOrWhiteSpace(utilizatori.AdresaLivrare))
                errors.Add("Adresa de livrare nu poate fi goala!");
            if (string.IsNullOrWhiteSpace(utilizatori.Parola))
                errors.Add("Parola nu poate fi goala!");
            if (string.IsNullOrWhiteSpace(utilizatori.TipUtilizator))
                errors.Add("Tipul de utilizator nu poate fi gol!");

            // Email format validation (simple example)
            if (!string.IsNullOrWhiteSpace(utilizatori.Email) && !IsValidEmail(utilizatori.Email))
                errors.Add("Emailul nu este valid!");

            // Check uniqueness in one query to reduce DB calls
            var existingUsers = context.Utilizatoris.Where(u =>
                u.Email == utilizatori.Email ||
                u.Telefon == utilizatori.Telefon ||
                (u.Nume == utilizatori.Nume && u.Prenume == utilizatori.Prenume)).ToList();

            if (existingUsers.Any(u => u.Email == utilizatori.Email))
                errors.Add("Un utilizator cu acest email exista deja!");
            if (existingUsers.Any(u => u.Telefon == utilizatori.Telefon))
                errors.Add("Un utilizator cu acest numar de telefon exista deja!");
            if (existingUsers.Any(u => u.Nume == utilizatori.Nume && u.Prenume == utilizatori.Prenume))
                errors.Add("Un utilizator cu acest nume si prenume exista deja!");

            return errors;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
