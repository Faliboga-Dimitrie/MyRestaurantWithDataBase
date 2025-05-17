using MyRestaurant.Models.DataAcessLayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRestaurant.Models.BuisnessLogicLayer
{
    public class FotografiBLL 
    {
        private MyRestaurantDAL context = new MyRestaurantDAL();
        public ObservableCollection<Fotografi> FotografiList { get; set; }
        public string ErrorMessage { get; set; }

        public void AddMethode(object obj)
        {
            Fotografi fotografie = obj as Fotografi;
            if (fotografie != null)
            {
                if (fotografie.Fotografie == null || fotografie.Fotografie.Length == 0)
                {
                    ErrorMessage = "Imaginea este invalidă.";
                    return;
                }

                try
                {
                    // Attach related preparat if needed (optional)
                     //context.Preparates.Attach(fotografie.IdpreparatNavigation);

                    context.Fotografis.Add(fotografie);
                    context.SaveChanges();

                    FotografiList.Add(fotografie);
                    ErrorMessage = string.Empty;
                }
                catch (Exception ex)
                {
                    ErrorMessage = "Eroare la adăugarea fotografiei: " + ex.Message;
                }
            }
            else
            {
                ErrorMessage = "Fotografia este nullă.";
            }
        }

        public void DeleteMethode(object obj)
        {
            Fotografi fotografie = obj as Fotografi;
            if (fotografie != null)
            {
                try
                {
                    context.Fotografis.Remove(fotografie);
                    context.SaveChanges();
                    FotografiList.Remove(fotografie);
                    ErrorMessage = string.Empty;
                }
                catch (Exception ex)
                {
                    ErrorMessage = "Eroare la ștergerea fotografiei: " + ex.Message;
                }
            }
            else
            {
                ErrorMessage = "Fotografia este nullă.";
            }
        }

        public ObservableCollection<Fotografi> GetAllFotografis()
        {
            return [.. context.Fotografis];
        }
    }
}
