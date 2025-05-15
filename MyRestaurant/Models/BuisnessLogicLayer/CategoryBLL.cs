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

        private ObservableCollection<Categorii> CategoriList { get; set; }

        private string ErrorMessage { get; set; }

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
        }

    }
}
