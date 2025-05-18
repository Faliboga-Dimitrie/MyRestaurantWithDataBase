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
    public class MeniuPreparatBLL
    {
        private MyRestaurantDAL context = new MyRestaurantDAL();
        public ObservableCollection<MeniuPreparat> MeniuPreparatList { get; set; }
        public string ErrorMessage { get; set; }

        public void AddMethode(object param)
        {
            MeniuPreparat meniuPreparat = param as MeniuPreparat;
            if (meniuPreparat != null)
            {
                if (meniuPreparat.Idmeniu == 0)
                {
                    ErrorMessage = "Id-ul meniului nu poate fi 0!";
                }
                else if (meniuPreparat.Idpreparat == 0)
                {
                    ErrorMessage = "Id-ul preparatului nu poate fi 0!";
                }
                else if (meniuPreparat.CantitateInMeniu <= 0)
                {
                    ErrorMessage = "Cantitatea in meniu trebuie sa fie mai mare decat 0!";
                }
                else
                {
                    try
                    {
                        context.AddPreparatForMeniu(meniuPreparat.Idmeniu, meniuPreparat.Idpreparat, meniuPreparat.CantitateInMeniu);
                        context.SaveChanges();
                        MeniuPreparatList.Add(meniuPreparat);
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
                ErrorMessage = "Meniul preparat came as null";
            }
        }

        public void DeleteMethode(object param)
        {
            MeniuPreparat meniuPreparat = param as MeniuPreparat;
            if (meniuPreparat != null)
            {
                try
                {
                    context.DeletePreparatForMeniu(meniuPreparat.Idmeniu, meniuPreparat.Idpreparat);
                    context.SaveChanges();
                    var meniuPreparatToRemove = MeniuPreparatList.FirstOrDefault(mp => mp.Idmeniu == meniuPreparat.Idmeniu && mp.Idpreparat == meniuPreparat.Idpreparat);
                    MeniuPreparatList.Remove(meniuPreparatToRemove);
                    ErrorMessage = string.Empty;
                }
                catch (Exception ex)
                {
                    ErrorMessage = ex.Message;
                }
            }
            else
            {
                ErrorMessage = "Meniul preparat came as null";
            }
        }

        public ObservableCollection<MeniuPreparat> GetAllPreparates()
        {
            try
            {
                var meniuPreparatList = context.MeniuPreparats
                    .Include(mp => mp.IdmeniuNavigation)
                    .Include(mp => mp.IdpreparatNavigation)
                    .ToList();
                MeniuPreparatList = new ObservableCollection<MeniuPreparat>(meniuPreparatList);
                return MeniuPreparatList;
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return null;
            }
        }
    }
}
