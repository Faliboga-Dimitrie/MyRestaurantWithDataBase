using Microsoft.EntityFrameworkCore;
using MyRestaurant.Models.DataAcessLayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRestaurant.Models.BuisnessLogicLayer
{
    public class PreparateBLL
    {
        private MyRestaurantDAL context = new MyRestaurantDAL();
        public ObservableCollection<Preparate> PreparateList { get; set; }
        public string ErrorMessage { get; set; }
        public void AddMethode(object obj, List<Alergeni> selectedAlergeni)
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
                        var existingAlergeni = context.Alergenis
                    .Where(a => selectedAlergeni.Select(sa => sa.Idalergen).Contains(a.Idalergen))
                    .ToList();

                        // Assign tracked entities to the preparat
                        preparate.Idalergens = existingAlergeni;
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

        public void UpdateMethode(object obj, List<Alergeni> selectedAlergeni)
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
                        //context.UpdatePreparate(preparate.Idpreparat, preparate.Denumire, preparate.Pret, preparate.CantitatePortie, preparate.CantitateTotala, preparate.Idcategorie);
                        //ErrorMessage = string.Empty;
                        // Attach preparat to context if not tracked
                        var existingPreparat = context.Preparates
                            .Include(p => p.Idalergens)
                            .FirstOrDefault(p => p.Idpreparat == preparate.Idpreparat);

                        var existingAlergeni = context.Alergenis
                    .Where(a => selectedAlergeni.Select(sa => sa.Idalergen).Contains(a.Idalergen))
                    .ToList();

                        if (existingPreparat != null)
                        {
                            // Update scalar properties
                            existingPreparat.Denumire = preparate.Denumire;
                            existingPreparat.Pret = preparate.Pret;
                            existingPreparat.CantitatePortie = preparate.CantitatePortie;
                            existingPreparat.CantitateTotala = preparate.CantitateTotala;
                            existingPreparat.Idcategorie = preparate.Idcategorie;

                            // Update allergens
                            existingPreparat.Idalergens.Clear();
                            foreach (var alergen in existingAlergeni)
                            {
                                existingPreparat.Idalergens.Add(alergen);
                            }

                            context.SaveChanges();
                            ErrorMessage = string.Empty;
                        }
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

        public void RemoveAlergenFromPreparat(int preparatId, int alergenId)
        {
            var preparat = context.Preparates
                .Include(p => p.Idalergens)
                .FirstOrDefault(p => p.Idpreparat == preparatId);

            if (preparat == null)
            {
                ErrorMessage = "Preparatul nu a fost gasit.";
                return;
            }

            var alergenToRemove = preparat.Idalergens.FirstOrDefault(a => a.Idalergen == alergenId);
            if (alergenToRemove != null)
            {
                preparat.Idalergens.Remove(alergenToRemove);
                context.SaveChanges();
                ErrorMessage = string.Empty;
            }
            else
            {
                ErrorMessage = "Alergenul nu este asociat cu preparatul.";
            }
        }

        public ObservableCollection<Preparate> GetAllPreparate()
        {
            var preparateWithAlergeni = context.Preparates
                .Include(p => p.Idalergens)
                .Include(p => p.IdcategorieNavigation)
                .ToList();

            return new ObservableCollection<Preparate>(preparateWithAlergeni);
        }
    }
}
