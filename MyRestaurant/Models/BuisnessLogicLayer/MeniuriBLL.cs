using Microsoft.EntityFrameworkCore;
using MyRestaurant.Models;
using MyRestaurant.Models.DataAcessLayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRestaurant.Models.BuisnessLogicLayer
{
    public class MeniuriBLL
    {
        private MyRestaurantDAL context = new MyRestaurantDAL();
        public ObservableCollection<Meniuri> MeniuriList { get; set; }
        public string ErrorMessage { get; set; }

        public void AddMethode(object param)
        {
            Meniuri meniu = param as Meniuri;
            if (meniu != null)
            {
                if (string.IsNullOrEmpty(meniu.Denumire))
                {
                    ErrorMessage = "Numele nu poate fi gol!";
                }
                else if (context.Meniuris.Any(m => m.Denumire == meniu.Denumire))
                {
                    ErrorMessage = "Un meniu cu acest nume exista deja!";
                }
                else
                {
                    try
                    {
                        meniu.Idmeniu = context.AddMeniu(meniu.Denumire, meniu.Idcategorie);
                        context.SaveChanges();
                        MeniuriList.Add(meniu);
                        ErrorMessage = string.Empty;
                    }
                    catch (Exception ex)
                    {
                        ErrorMessage = ex.Message;
                    }
                }
            }
            else
            {
                ErrorMessage = "Meniul came as null";
            }
        }

        public async Task AddMethodeAsync(object param)
        {
            Meniuri meniu = param as Meniuri;
            if (meniu != null)
            {
                if (string.IsNullOrEmpty(meniu.Denumire))
                {
                    ErrorMessage = "Numele nu poate fi gol!";
                }
                else if (context.Meniuris.Any(m => m.Denumire == meniu.Denumire))
                {
                    ErrorMessage = "Un meniu cu acest nume exista deja!";
                }
                else
                {
                    try
                    {
                        meniu.Idmeniu = await context.AddMeniuAsync(meniu.Denumire, meniu.Idcategorie);
                        context.SaveChanges();
                        MeniuriList.Add(meniu);
                        ErrorMessage = string.Empty;
                    }
                    catch (Exception ex)
                    {
                        ErrorMessage = ex.Message;
                    }
                }
            }
            else
            {
                ErrorMessage = "Meniul came as null";
            }
        }


        public void UpdateMethode(object param)
        {
            Meniuri meniu = param as Meniuri;
            if (meniu != null)
            {
                if (string.IsNullOrEmpty(meniu.Denumire))
                {
                    ErrorMessage = "Numele nu poate fi gol!";
                }
                else if (context.Meniuris.Any(m => m.Denumire == meniu.Denumire && m.Idmeniu != meniu.Idmeniu))
                {
                    ErrorMessage = "Un meniu cu acest nume exista deja!";
                }
                else
                {
                    try
                    {
                        context.UpdateMeniu(meniu.Idcategorie,meniu.Denumire,meniu.Idmeniu);
                        context.SaveChanges();
                        ErrorMessage = string.Empty;
                    }
                    catch (Exception ex)
                    {
                        ErrorMessage = ex.Message;
                    }
                }
            }
            else
            {
                ErrorMessage = "Meniul came as null";
            }
        }

        public void DeleteMethode(object param)
        {
            Meniuri meniu = param as Meniuri;
            if (meniu != null)
            {
                try
                {
                    context.DeleteMeniu(meniu.Idmeniu);
                    context.SaveChanges();
                    MeniuriList.Remove(meniu);
                    ErrorMessage = string.Empty;
                }
                catch (Exception ex)
                {
                    ErrorMessage = ex.Message;
                }
            }
            else
            {
                ErrorMessage = "Meniul came as null";
            }
        }

        public ObservableCollection<Meniuri> GetMeniuriList()
        {
            return [.. context.Meniuris];
        }

        public ObservableCollection<Meniuri> GetMeniuriListFullData()
        {
            var meniuri = context.Meniuris
                .Include(m => m.MeniuPreparats)
                    .ThenInclude(mp => mp.IdpreparatNavigation)
                        .ThenInclude(p => p.Idalergens)
                .Include(m => m.IdcategorieNavigation)
                .ToList();

            return new ObservableCollection<Meniuri>(meniuri);
        }
    }
}