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
    public class ComenziBLL
    {
        private MyRestaurantDAL context = new MyRestaurantDAL();

        public ObservableCollection<Comenzi> ComenziList { get; set; }

        public string ErrorMessage { get; set; }

        public async Task AddMethode(object param)
        {
            Comenzi comanda = param as Comenzi;
            if(comanda != null)
            {
                if (comanda.Idutilizator < 0)
                {
                    ErrorMessage = "The user is invalid!";
                }
                else if (comanda.Cost < 0)
                {
                    ErrorMessage = "The cost came below 0!";
                }
                else
                {
                    comanda.Idcomanda = await context.AddComandaAsync(comanda.CodUnic, comanda.Idutilizator, comanda.DataComanda, comanda.Stare, comanda.Cost);
                    context.SaveChanges();
                    ComenziList.Add(comanda);
                    ErrorMessage = string.Empty; // Clear error message on success
                }
            }
            else
            {
                ErrorMessage = "param came as null!";
            }
        }

        public async Task CalculateCost(object param)
        {
            Comenzi comanda = param as Comenzi;
            if (comanda != null)
            {
                if (comanda.Idcomanda < 0)
                {
                    ErrorMessage = "The order is invalid!";
                }
                else
                {
                    try
                    {
                        comanda.Cost = await context.CalculeazaPretComandaAsync(comanda.Idcomanda);
                        context.SaveChanges();
                        ErrorMessage = string.Empty; // Clear error message on success
                    }
                    catch (Exception ex)
                    {
                        ErrorMessage = ex.Message;
                    }
                }
            }
            else
            {
                ErrorMessage = "param came as null!";
            }
        }

        public void UpdateCost(object param)
        {
            Comenzi comanda = param as Comenzi;
            if (comanda != null)
            {
                if (comanda.Idcomanda < 0)
                {
                    ErrorMessage = "The order is invalid!";
                }
                else if (comanda.Cost < 0)
                {
                    ErrorMessage = "The cost came below 0!";
                }
                else
                {
                    try
                    {
                        context.UpdateComandaCost(comanda.Idcomanda, comanda.Cost);
                        context.SaveChanges();
                        ErrorMessage = string.Empty; // Clear error message on success
                    }
                    catch (Exception ex)
                    {
                        ErrorMessage = ex.Message;
                    }
                }
            }
            else
            {
                ErrorMessage = "param came as null!";
            }
        }

        public void UpdateStatus(object param)
        {
            Comenzi comanda = param as Comenzi;
            if (comanda != null)
            {
                if (comanda.Idcomanda < 0)
                {
                    ErrorMessage = "The order is invalid!";
                }
                else
                {
                    try
                    {
                        context.UpdateComandaStare(comanda.Idcomanda, comanda.Stare);
                        context.SaveChanges();
                        ErrorMessage = string.Empty; // Clear error message on success
                    }
                    catch (Exception ex)
                    {
                        ErrorMessage = ex.Message;
                    }
                }
            }
            else
            {
                ErrorMessage = "param came as null!";
            }
        }

        public ObservableCollection<Comenzi> GetComenziList()
        {
            ComenziList = new ObservableCollection<Comenzi>(context.Comenzis.ToList());
            return ComenziList;
        }

        public ObservableCollection<Comenzi> GetComenziListAllData()
        {
            var comenziWithAllData = context.Comenzis
                .Include(c => c.IdutilizatorNavigation)           // Load related Utilizatori
                .Include(c => c.ComandaMenius)                     // Load related ComandaMeniu collection
                .ToList();

            ComenziList = new ObservableCollection<Comenzi>(comenziWithAllData);
            return ComenziList;
        }

    }
}
