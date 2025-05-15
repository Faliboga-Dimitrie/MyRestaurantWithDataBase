using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyRestaurant.Models.DataAcessLayer;
using System.Collections.ObjectModel;

namespace MyRestaurant.Models.BuisnessLogicLayer
{
    public class PreparateBLL
    {
        private MyRestaurantDAL context = new MyRestaurantDAL();
        public ObservableCollection<Preparate> PreparateList { get; set; }
        public string ErrorMessage { get; set; }
        public void AddMethode(object obj)
        {
            Preparate preparate = obj as Preparate;
            if (preparate != null)
            {
                if (string.IsNullOrEmpty(preparate.Denumire))
                {
                    ErrorMessage = "Numele nu poate fi gol!";
                }
                else if (string.IsNullOrEmpty(preparate.Pret.ToString()))
                {
                    ErrorMessage = "Pretul nu poate fi gol!";
                }
                else if (string.IsNullOrEmpty(preparate.CantitatePortie.ToString()))
                {
                    ErrorMessage = "Cantitatea pe portie nu poate fi goala!";
                }
                else if (string.IsNullOrEmpty(preparate.CantitateTotala.ToString()))
                {
                    ErrorMessage = "Cantitatea totala nu poate fi goala!";
                }
                else if (preparate.Pret <= 0)
                {
                    ErrorMessage = "Pretul trebuie sa fie mai mare decat 0!";
                }
                else if (preparate.CantitatePortie <= 0)
                {
                    ErrorMessage = "Cantitatea pe portie trebuie sa fie mai mare decat 0!";
                }
                else if (preparate.CantitateTotala <= 0)
                {
                    ErrorMessage = "Cantitatea totala trebuie sa fie mai mare decat 0!";
                }
                else if (context.Preparates.Any(p => p.Denumire == preparate.Denumire))
                {
                    ErrorMessage = "Un preparat cu acest nume exista deja!";
                }
                else
                {
                    try
                    {
                        context.Preparates.Add(preparate);
                        context.SaveChanges();
                        PreparateList.Add(preparate);
                        ErrorMessage = string.Empty;
                    }
                    catch (Exception ex)
                    {
                        ErrorMessage = "A aparut o eroare la adaugarea preparatului: " + ex.Message;
                    }
                }
            }
        }

        public void UpdateMethode(object obj)
        {
            Preparate preparate = obj as Preparate;
            if (preparate != null)
            {
                if (string.IsNullOrEmpty(preparate.Denumire))
                {
                    ErrorMessage = "Numele nu poate fi gol!";
                }
                else if (string.IsNullOrEmpty(preparate.Pret.ToString()))
                {
                    ErrorMessage = "Pretul nu poate fi gol!";
                }
                else if (string.IsNullOrEmpty(preparate.CantitatePortie.ToString()))
                {
                    ErrorMessage = "Cantitatea pe portie nu poate fi goala!";
                }
                else if (string.IsNullOrEmpty(preparate.CantitateTotala.ToString()))
                {
                    ErrorMessage = "Cantitatea totala nu poate fi goala!";
                }
                else if (preparate.Pret <= 0)
                {
                    ErrorMessage = "Pretul trebuie sa fie mai mare decat 0!";
                }
                else if (preparate.CantitatePortie < 0)
                {
                    ErrorMessage = "Cantitatea pe portie trebuie sa fie mai mare sau egala cu 0!";
                }
                else if (preparate.CantitateTotala < 0)
                {
                    ErrorMessage = "Cantitatea totala trebuie sa fie mai mare sau egala cu 0!";
                }
                else
                {
                    try
                    {
                        //context.Entry(preparate).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        //context.SaveChanges();
                        context.UpdatePreparate(preparate.Idpreparat, preparate.Denumire, preparate.Pret, preparate.CantitatePortie, preparate.CantitateTotala, preparate.Idcategorie);
                        ErrorMessage = string.Empty;
                    }
                    catch (Exception ex)
                    {
                        ErrorMessage = "A aparut o eroare la actualizarea preparatului: " + ex.Message;
                    }
                }
            }
        }

        public void DeleteMethode(object obj)
        {
            Preparate preparate = obj as Preparate;
            if (preparate != null)
            {
                try
                {
                    context.DeletePreparate(preparate.Idpreparat);
                    PreparateList.Remove(preparate);
                    ErrorMessage = string.Empty;
                }
                catch (Exception ex)
                {
                    ErrorMessage = "A aparut o eroare la stergerea preparatului: " + ex.Message;
                }
            }
        }

        public ObservableCollection<Preparate> GetAllPreparate()
        {
            return [.. context.Preparates];
        }
    }
}
