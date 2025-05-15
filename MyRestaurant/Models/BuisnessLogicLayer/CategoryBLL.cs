using MyRestaurant.Models.DataAcessLayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRestaurant.Models.BuisnessLogicLayer
{
    public class CategoryBLL
    {
        private MyRestaurantDAL context = new MyRestaurantDAL();

        public ObservableCollection<Categorii> CategoriList { get; set; }

        public string ErrorMessage { get; set; }

        public void AddMethode(object obj)
        {
            Categorii categorie = obj as Categorii;
            if (categorie != null)
            {
                if (string.IsNullOrEmpty(categorie.NumeCategorie))
                {
                    ErrorMessage = "Numele categoriei nu poate fi gol";
                }
                else if (context.Categoriis.Any(p => p.NumeCategorie == categorie.NumeCategorie))
                {
                    ErrorMessage = "Categoria cu acest nume exista deja";
                }
                else
                {
                    try
                    {
                        context.Categoriis.Add(categorie);
                        context.SaveChanges();
                        CategoriList.Add(categorie);
                        ErrorMessage = string.Empty;
                    }
                    catch (Exception ex)
                    {
                        ErrorMessage = "A aparut o eroate la adaugarea categoriei: " + ex.Message;
                    }
                }
            }
        }

        public void UpdateMethode(object obj)
        {
            Categorii categorie = obj as Categorii;

            if (categorie != null)
            {
                if (string.IsNullOrEmpty(categorie.NumeCategorie))
                {
                    ErrorMessage = "Numele categoriei nu poate fi gol";
                }
                else if (context.Categoriis.Any(p => p.NumeCategorie == categorie.NumeCategorie && p.Idcategorie != categorie.Idcategorie))
                {
                    ErrorMessage = "Categoria cu acest nume exista deja";
                }
                else
                {
                    try
                    {
                        context.UpdateCategorie(categorie.Idcategorie, categorie.NumeCategorie);
                        ErrorMessage = string.Empty;
                    }
                    catch (Exception ex)
                    {
                        ErrorMessage = "A aparut o eroare la actualizarea categoriei: " + ex.Message;
                    }
                }
            }
        }

        public void DeleteMethode(object obj)
        {
            Categorii categorie = obj as Categorii;
            if (categorie != null)
            {
                try
                {
                    context.DeleteCategorie(categorie.Idcategorie);
                    var itemToRemove = CategoriList.FirstOrDefault(c => c.NumeCategorie.Equals(categorie.NumeCategorie, StringComparison.OrdinalIgnoreCase));
                    CategoriList.Remove(itemToRemove);
                    ErrorMessage = string.Empty;
                }
                catch (Exception ex)
                {
                    ErrorMessage = "A aparut o eroare la stergerea categoriei: " + ex.Message;
                }
            }
        }

        public ObservableCollection<Categorii> GetAllCategori()
        {
            return [.. context.Categoriis];
        }

    }
}
