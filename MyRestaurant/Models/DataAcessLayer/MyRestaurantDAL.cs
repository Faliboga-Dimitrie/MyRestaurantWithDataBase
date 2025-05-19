using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRestaurant.Models.DataAcessLayer
{
    public class MyRestaurantDAL : MyRestaurantContext
    {
        #region CRUD Operations

        public async Task<int> AddAlergenAsync(string alergenName)
        {
            var nameParam = new SqlParameter("@NumeAlergen", alergenName);
            var idParam = new SqlParameter("@IDAlergen", SqlDbType.Int) { Direction = ParameterDirection.Output };
            await Database.ExecuteSqlRawAsync("EXEC AddAlergen @IDAlergen OUTPUT, @NumeAlergen", idParam, nameParam);
            return (int)idParam.Value;
        }

        public void UpdateAlergen(int alergenID, string alergenName)
        {
            var nameParam = new SqlParameter("@NumeAlergen", alergenName);
            var idParam = new SqlParameter("@IDAlergen", alergenID);
            Database.ExecuteSqlRawAsync("EXEC UpdateAlergeni @IDAlergen, @NumeAlergen", idParam, nameParam);
        }

        public void DeleteAlergen(int alergenID)
        {
            var idParam = new SqlParameter("@IDAlergen", alergenID);
            Database.ExecuteSqlRawAsync("EXEC DeleteAlergeni @IDAlergen", idParam);
        }

        public async Task<int> AddCategorieAsync(string categorieName)
        {
            var nameParam = new SqlParameter("@NumeCategorie", categorieName);
            var idParam = new SqlParameter("@IDCategorie", SqlDbType.Int) { Direction = ParameterDirection.Output };
            await Database.ExecuteSqlRawAsync("EXEC AddCategorie @IDCategorie OUTPUT, @NumeCategorie", idParam, nameParam);
            return (int)idParam.Value;
        }

        public void UpdateCategorie(int categorieID, string categorieName)
        {
            var nameParam = new SqlParameter("@NumeCategorie", categorieName);
            var idParam = new SqlParameter("@IDCategorie", categorieID);
            Database.ExecuteSqlRawAsync("EXEC UpdateCategorii @IDCategorie, @NumeCategorie", idParam, nameParam);
        }

        public void DeleteCategorie(int categorieID)
        {
            var idParam = new SqlParameter("@IDCategorie", categorieID);
            Database.ExecuteSqlRawAsync("EXEC DeleteCategorii @IDCategorie", idParam);
        }

        public async Task<int> AddPreparateAsync(string name, decimal price, int quantityPerPortion, int TotalQuantity, int categoryID)
        {
            var nameParam = new SqlParameter("@Denumire", name);
            var priceParam = new SqlParameter("@Pret", price);
            var quantityPerPortionParam = new SqlParameter("@CantitatePortie", quantityPerPortion);
            var totalQuantityParam = new SqlParameter("@CantitateTotala", TotalQuantity);
            var categoryParam = new SqlParameter("@IDCategorie", categoryID);
            var idParam = new SqlParameter("@IDPreparat", SqlDbType.Int) { Direction = ParameterDirection.Output };
            await Database.ExecuteSqlRawAsync("EXEC AddPreparate @Denumire, @Pret, @CantitatePortie, @CantitateTotala, @IDCategorie, @IDPreparat OUTPUT", nameParam, priceParam, quantityPerPortionParam, totalQuantityParam, categoryParam, idParam);
            return (int)idParam.Value;
        }

        public void UpdatePreparate(int preparatID, string name, decimal price, int quantityPerPortion, int totalQuantity, int categoryID)
        {
            var nameParam = new SqlParameter("@Denumire", name);
            var priceParam = new SqlParameter("@Pret", price);
            var quantityPerPortionParam = new SqlParameter("@CantitatePortie", quantityPerPortion);
            var totalQuantityParam = new SqlParameter("@CantitateTotala", totalQuantity);
            var categoryParam = new SqlParameter("@IDCategorie", categoryID);
            var idParam = new SqlParameter("@IDPreparat", preparatID);
            Database.ExecuteSqlRawAsync("EXEC UpdatePreparat  @IDPreparat, @Denumire, @IDCategorie, @Pret, @CantitatePortie, @CantitateTotala", idParam, nameParam, categoryParam, priceParam, quantityPerPortionParam, totalQuantityParam);
        }

        public void DeletePreparate(int preparatID)
        {
            var idParam = new SqlParameter("@IDPreparat", preparatID);
            Database.ExecuteSqlRawAsync("EXEC DeletePreparat @IDPreparat", idParam);
        }

        public async Task<int> AddMeniuAsync(string name, int categoryID)
        {
            var nameParam = new SqlParameter("@Denumire", name);
            var categoryParam = new SqlParameter("@IDCategorie", categoryID);
            var idParam = new SqlParameter("@IDMeniu", SqlDbType.Int) { Direction = ParameterDirection.Output };

            await Database.ExecuteSqlRawAsync("EXEC AddMeniu @Denumire, @IDCategorie, @IDMeniu OUTPUT", nameParam, categoryParam, idParam);

            return (int)idParam.Value;
        }


        public void UpdateMeniu(int categoryID, string name, int menuID)
        {
            var nameParam = new SqlParameter("@Denumire", name);
            var categoryParam = new SqlParameter("@IDCategorie", categoryID);
            var idParam = new SqlParameter("@IDMeniu", menuID);
            Database.ExecuteSqlRawAsync("EXEC UpdateMeniuri @IDCategorie, @Denumire, @IDMeniu", categoryParam, nameParam, idParam);
        }

        public void DeleteMeniu(int menuID)
        {
            var idParam = new SqlParameter("@IDMeniu", menuID);
            Database.ExecuteSqlRawAsync("EXEC DeleteMeniuri @IDMeniu", idParam);
        }

        #endregion

        public async Task<int> AddComandaAsync(Guid uniqueCode, int userID, DateTime orderDate, string status, decimal cost)
        {
            var codeParam = new SqlParameter("@CodUnic", uniqueCode);
            var userParam = new SqlParameter("@IDUtilizator", userID);
            var dateParam = new SqlParameter("@DataComanda", orderDate);
            var statusParam = new SqlParameter("@Stare", status);
            var costParam = new SqlParameter("@Cost", cost);
            var idParam = new SqlParameter("@IDComanda", SqlDbType.Int) { Direction = ParameterDirection.Output };
            await Database.ExecuteSqlRawAsync("EXEC AddComanda @CodUnic, @IDUtilizator, @DataComanda, @Stare, @Cost, @IDComanda OUTPUT", codeParam, userParam, dateParam, statusParam, costParam, idParam);
            return (int)idParam.Value;
        }

        public async Task AddMeniuForComandaAsync(int orderID, int menuID, int quantity)
        {
            var orderParam = new SqlParameter("@ComandaID", orderID);
            var menuParam = new SqlParameter("@MeniuID", menuID);
            var quantityParam = new SqlParameter("@Cantitate", quantity);
            await Database.ExecuteSqlRawAsync("EXEC AddComandaMeniu @ComandaID, @MeniuID, @Cantitate", orderParam, menuParam, quantityParam);
        }

        public void DeleteMeniuForComanda(int orderID, int menuID)
        {
            var orderParam = new SqlParameter("@IDComanda", orderID);
            var menuParam = new SqlParameter("@IDMeniu", menuID);
            Database.ExecuteSqlRawAsync("EXEC DeleteMeniuFromComanda @IDComanda, @IDMeniu", orderParam, menuParam);
        }

        public async Task AddPreparatForComandaAsync(int orderID, int preparatID, int quantity)
        {
            var orderParam = new SqlParameter("@ComandaID", orderID);
            var preparatParam = new SqlParameter("@PreparatID", preparatID);
            var quantityParam = new SqlParameter("@Cantitate", quantity);
            await Database.ExecuteSqlRawAsync("EXEC AddComandaPreparat @ComandaID, @PreparatID, @Cantitate", orderParam, preparatParam, quantityParam);
        }

        public void DeletePreparatForComanda(int orderID, int preparatID)
        {
            var orderParam = new SqlParameter("@IDComanda", orderID);
            var preparatParam = new SqlParameter("@IDPreparat", preparatID);
            Database.ExecuteSqlRawAsync("EXEC DeletePreparatFromComanda @IDComanda, @IDPreparat", orderParam, preparatParam);
        }

        public async Task<decimal> CalculeazaPretComandaAsync(int idComanda)
        {
            var idComandaParam = new SqlParameter("@IDComanda", idComanda);

            var result = await Database.SqlQueryRaw<decimal>(
                "EXEC CalculeazaPretComanda @IDComanda", idComandaParam).ToListAsync();

            return result.FirstOrDefault();
        }

        public void UpdateComandaStare(int orderID, string status)
        {
            var orderIDParam = new SqlParameter("@IDComanda", orderID);
            var statusParam = new SqlParameter("@Stare", status);
            Database.ExecuteSqlRawAsync("EXEC UpdateComandaStatus @IDComanda, @Stare", orderIDParam, statusParam);
        }

        public void UpdateComandaCost(int orderID, decimal cost)
        {
            var orderIDParam = new SqlParameter("@IDComanda", orderID);
            var costParam = new SqlParameter("@PretNou", cost);
            Database.ExecuteSqlRawAsync("EXEC UpdatePretComandaDirect @IDComanda, @PretNou", orderIDParam, costParam);
        }

        public void AddPreparatForMeniu(int menuID, int preparatID, int preparatQuantity)
        {
            var menuIDParam = new SqlParameter("@IDMeniu", menuID);
            var preparatIDParam = new SqlParameter("@IDPreparat", preparatID); // Fixed parameter name here
            var preparatQuantityParam = new SqlParameter("@CantitateInMeniu", preparatQuantity);

            Database.ExecuteSqlRawAsync(
                "EXEC AddMeniuPreparat @IDMeniu, @IDPreparat, @CantitateInMeniu",
                menuIDParam, preparatIDParam, preparatQuantityParam);
        }

        public void DeletePreparatForMeniu(int menuID, int preparatID)
        {
            var menuIDParam = new SqlParameter("@IDMeniu", menuID);
            var preparatIDParam = new SqlParameter("@IDPreparat", preparatID);
            Database.ExecuteSqlRawAsync("EXEC DeleteMeniuPreparat @IDMeniu, @IDPreparat", menuIDParam, preparatIDParam);
        }

        public void AddAlergenForPreparat(int preparatID, int alergenID)
        {
            var preparatIDParam = new SqlParameter("@IDPreparat", preparatID);
            var alergenIDParam = new SqlParameter("@IDAlergen", alergenID);
            Database.ExecuteSqlRawAsync("EXEC AddPreparatAlergen @IDPreparat, @IDAlergen", preparatIDParam, alergenIDParam);
        }

        public async Task<int> AddUtilizatorAsync(string firstName, string lastName, string email, string phone, string deliveryAddress, string password, string type)
        {
            var firstNameParam = new SqlParameter("@Nume", firstName);
            var lastNameParam = new SqlParameter("@Prenume", lastName);
            var emailParam = new SqlParameter("@Email", email);
            var phoneParam = new SqlParameter("@Telefon", phone);
            var deliveryAddressParam = new SqlParameter("@AdresaLivrare", deliveryAddress);
            var passwordParam = new SqlParameter("@Parola", password);
            var typeParam = new SqlParameter("@TipUtilizator", type);
            var idParam = new SqlParameter("@IDUtilizator", SqlDbType.Int) { Direction = ParameterDirection.Output };
            await Database.ExecuteSqlRawAsync("EXEC AddUtilizator @Nume, @Prenume, @Email, @Telefon, @AdresaLivrare, @Parola, @TipUtilizator, @IDUtilizator OUTPUT", firstNameParam, lastNameParam, emailParam, phoneParam, deliveryAddressParam, passwordParam, typeParam, idParam);
            return (int)idParam.Value;
        }
    }
}
