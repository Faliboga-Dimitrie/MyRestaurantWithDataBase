using MyRestaurant.Models.DataAcessLayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRestaurant.Models.BuisnessLogicLayer
{
    public class ComandaMeniuBLL
    {
        private MyRestaurantDAL context = new MyRestaurantDAL();

        public ObservableCollection<ComandaMeniu> ComandaMeniuList { get; set; }

        public string ErrorMessage { get; set; }

        public async Task AddMethodeAsync(object param)
        {
            ComandaMeniu comandaMeniu = param as ComandaMeniu;
            if (comandaMeniu != null)
            {
                if (comandaMeniu.Idcomanda == 0)
                {
                    ErrorMessage = "Id-ul comenzii nu poate fi 0!";
                }
                else if (comandaMeniu.Idmeniu == 0)
                {
                    ErrorMessage = "Id-ul meniului nu poate fi 0!";
                }
                else if (comandaMeniu.Cantitate <= 0)
                {
                    ErrorMessage = "Cantitatea trebuie sa fie mai mare decat 0!";
                }
                else
                {
                    try
                    {
                        await context.AddMeniuForComandaAsync(comandaMeniu.Idcomanda, comandaMeniu.Idmeniu, comandaMeniu.Cantitate);
                        context.SaveChanges();
                        ComandaMeniuList.Add(comandaMeniu);
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
                ErrorMessage = "Comanda meniului came as null";
            }
        }

        public void DeletheMethode(object param)
        {
            ComandaMeniu comandaMeniu = param as ComandaMeniu;
            if (comandaMeniu != null)
            {
                try
                {
                    context.DeleteMeniuForComanda(comandaMeniu.Idcomanda, comandaMeniu.Idmeniu);
                    context.SaveChanges();
                    ComandaMeniuList.Remove(comandaMeniu);
                    ErrorMessage = string.Empty;
                }
                catch (Exception ex)
                {
                    ErrorMessage = ex.Message;
                }
            }
            else
            {
                ErrorMessage = "Comanda meniului came as null";
            }
        }


        public ObservableCollection<ComandaMeniu> GetComandaMeniuList()
        {
            ComandaMeniuList = new ObservableCollection<ComandaMeniu>(context.ComandaMenius.ToList());
            return ComandaMeniuList;
        }
    }
}
