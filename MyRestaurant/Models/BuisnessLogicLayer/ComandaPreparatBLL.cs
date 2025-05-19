using MyRestaurant.Models.DataAcessLayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRestaurant.Models.BuisnessLogicLayer
{
    public class ComandaPreparatBLL
    {
        private MyRestaurantDAL context = new MyRestaurantDAL();

        public ObservableCollection<ComandaPreparat> ComandaPreparatList { get; set; }

        public string ErrorMessage { get; set; }

        public async Task AddMethodeAsync(object param)
        {
            ComandaPreparat comandaPreparat = param as ComandaPreparat;
            if (comandaPreparat != null)
            {
                if (comandaPreparat.Idcomanda == 0)
                {
                    ErrorMessage = "Id-ul comenzii nu poate fi 0!";
                }
                else if (comandaPreparat.Idpreparat == 0)
                {
                    ErrorMessage = "Id-ul preparatului nu poate fi 0!";
                }
                else if (comandaPreparat.Cantitate <= 0)
                {
                    ErrorMessage = "Cantitatea trebuie sa fie mai mare decat 0!";
                }
                else
                {
                    try
                    {
                        await context.AddPreparatForComandaAsync(comandaPreparat.Idcomanda, comandaPreparat.Idpreparat, comandaPreparat.Cantitate);
                        context.SaveChanges();
                        ComandaPreparatList.Add(comandaPreparat);
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
                ErrorMessage = "Comanda preparatului came as null";
            }
        }

        public void DeleteMethode(object param)
        {
            ComandaPreparat comandaPreparat = param as ComandaPreparat;
            if (comandaPreparat != null)
            {
                try
                {
                    context.DeletePreparatForComanda(comandaPreparat.Idcomanda, comandaPreparat.Idpreparat);
                    context.SaveChanges();
                    ComandaPreparatList.Remove(comandaPreparat);
                    ErrorMessage = string.Empty;
                }
                catch (Exception ex)
                {
                    ErrorMessage = ex.Message;
                }
            }
            else
            {
                ErrorMessage = "Comanda preparatului came as null";
            }
        }

        public ObservableCollection<ComandaPreparat> GetComandaPreparatList()
        {
            ComandaPreparatList = new ObservableCollection<ComandaPreparat>(context.ComandaPreparats.ToList());
            return ComandaPreparatList;
        }
    }
}
