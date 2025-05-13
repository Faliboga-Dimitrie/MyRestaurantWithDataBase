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
        public int AddAlergen(string alergenName)
        {
            var nameParam = new SqlParameter("@NumeAlergen", alergenName);
            var idParam = new SqlParameter("@IDAlergen", SqlDbType.Int) { Direction = ParameterDirection.Output };
            Database.ExecuteSqlRawAsync("EXEC AddAlergen @NumeAlergen, @IDAlergen OUTPUT", nameParam, idParam);
            return (int)idParam.Value;
        }

        public void UpdateAlergen(int alergenID, string alergenName)
        {
            var nameParam = new SqlParameter("@NumeAlergen", alergenName);
            var idParam = new SqlParameter("@IDAlergen", alergenID);
            Database.ExecuteSqlRawAsync("EXEC UpdateAlergeni @NumeAlergen, @IDAlergen", nameParam, idParam);
        }

        public void DeleteAlergen(int alergenID)
        {
            var idParam = new SqlParameter("@IDAlergen", alergenID);
            Database.ExecuteSqlRawAsync("EXEC DeleteAlergeni @IDAlergen", idParam);
        }

        public int AddCategorie(string categorieName)
        {
            var nameParam = new SqlParameter("@NumeCategorie", categorieName);
            var idParam = new SqlParameter("@IDCategorie", SqlDbType.Int) { Direction = ParameterDirection.Output };
            Database.ExecuteSqlRawAsync("EXEC AddCategorie @NumeCategorie, @IDCategorie OUTPUT", nameParam, idParam);
            return (int)idParam.Value;
        }

        public void UpdateCategorie(int categorieID, string categorieName)
        {
            var nameParam = new SqlParameter("@NumeCategorie", categorieName);
            var idParam = new SqlParameter("@IDCategorie", categorieID);
            Database.ExecuteSqlRawAsync("EXEC UpdateCategorii @NumeCategorie, @IDCategorie", nameParam, idParam);
        }

        public void DeleteCategorie(int categorieID)
        {
            var idParam = new SqlParameter("@IDCategorie", categorieID);
            Database.ExecuteSqlRawAsync("EXEC DeleteCategorii @IDCategorie", idParam);
        }

        public int AddPreparate(string name, decimal price, int quantityPerPortion, int TotalQuantity, int categoryID)
        {
            var nameParam = new SqlParameter("@Denumire", name);
            var priceParam = new SqlParameter("@Pret", price);
            var quantityPerPortionParam = new SqlParameter("@CantitatePortie", quantityPerPortion);
            var totalQuantityParam = new SqlParameter("@CantitateTotala", TotalQuantity);
            var categoryParam = new SqlParameter("@IDCategorie", categoryID);
            var idParam = new SqlParameter("@IDPreparat", SqlDbType.Int) { Direction = ParameterDirection.Output };
            Database.ExecuteSqlRawAsync("EXEC AddPreparate @Denumire, @Pret, @CantitatePortie, @CantitateTotala, @IDCategorie, @IDPreparat OUTPUT", nameParam, priceParam, quantityPerPortionParam, totalQuantityParam, categoryParam, idParam);
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
            Database.ExecuteSqlRawAsync("EXEC UpdatePreparat @Denumire, @Pret, @CantitatePortie, @CantitateTotala, @IDCategorie, @IDPreparat", nameParam, priceParam, quantityPerPortionParam, totalQuantityParam, categoryParam, idParam);
        }

        public void DeletePreparate(int preparatID)
        {
            var idParam = new SqlParameter("@IDPreparat", preparatID);
            Database.ExecuteSqlRawAsync("EXEC DeletePreparat @IDPreparat", idParam);
        }

        public int AddMeniu(string name, int categoryID)
        {
            var nameParam = new SqlParameter("@Denumire", name);
            var categoryParam = new SqlParameter("@IDCategorie", categoryID);
            var idParam = new SqlParameter("@IDMeniu", SqlDbType.Int) { Direction = ParameterDirection.Output };
            Database.ExecuteSqlRawAsync("EXEC AddMeniu @Denumire, @IDCategorie, @IDMeniu OUTPUT", nameParam, categoryParam, idParam);
            return (int)idParam.Value;
        }

        public void UpdateMeniu(int categoryID, string name, int menuID)
        {
            var nameParam = new SqlParameter("@Denumire", name);
            var categoryParam = new SqlParameter("@IDCategorie", categoryID);
            var idParam = new SqlParameter("@IDMeniu", menuID);
            Database.ExecuteSqlRawAsync("EXEC UpdateMeniuri @Denumire, @IDCategorie, @IDMeniu", nameParam, categoryParam, idParam);
        }

        public void DeleteMeniu(int menuID)
        {
            var idParam = new SqlParameter("@IDMeniu", menuID);
            Database.ExecuteSqlRawAsync("EXEC DeleteMeniuri @IDMeniu", idParam);
        }

        #endregion
        public int AddComanda(Guid uniqueCode, int userID, DateTime orderDate, string status, decimal cost)
        {
            var codeParam = new SqlParameter("@CodUnic", uniqueCode);
            var userParam = new SqlParameter("@IDUtilizator", userID);
            var dateParam = new SqlParameter("@DataComanda", orderDate);
            var statusParam = new SqlParameter("@Stare", status);
            var costParam = new SqlParameter("@Cost", cost);
            var idParam = new SqlParameter("@IDComanda", SqlDbType.Int) { Direction = ParameterDirection.Output };
            Database.ExecuteSqlRawAsync("EXEC AddComanda @CodUnic, @IDUtilizator, @DataComanda, @Stare, @Cost, @IDComanda OUTPUT", codeParam, userParam, dateParam, statusParam, costParam, idParam);
            return (int)idParam.Value;
        }

        public void AddMeniuForComanda(int orderID, int menuID, int quantity)
        {
            var orderParam = new SqlParameter("@ComandaID", orderID);
            var menuParam = new SqlParameter("@MeniuID", menuID);
            var quantityParam = new SqlParameter("@Cantitate", quantity);
            Database.ExecuteSqlRawAsync("EXEC AddComandaMeniu @ComandaID, @MeniuID, @Cantitate", orderParam, menuParam, quantityParam);
        }

        public void AddPreparatForComanda(int orderID, int preparatID, int quantity)
        {
            var orderParam = new SqlParameter("@ComandaID", orderID);
            var preparatParam = new SqlParameter("@PreparatID", preparatID);
            var quantityParam = new SqlParameter("@Cantitate", quantity);
            Database.ExecuteSqlRawAsync("EXEC AddComandaPreparat @ComandaID, @PreparatID, @Cantitate", orderParam, preparatParam, quantityParam);
        }

        public void AddPreparatForMeniu(int menuID, int preparatID, int preparatQuantity)
        {
            var menuIDParam = new SqlParameter("@IDMeniu", menuID);
            var preparatIDParam = new SqlParameter("@IDPreparat", preparatID);
            var preparatQuantityParam = new SqlParameter("@CantitateInMeniu", preparatQuantity);
            Database.ExecuteSqlRawAsync("EXEC AddMeniuPreparat @IDMeniu, @PIDPreparat, @CantitateInMeniu", menuIDParam, preparatIDParam, preparatQuantityParam);
        }

        public void AddAlergenForPreparat(int preparatID, int alergenID)
        {
            var preparatIDParam = new SqlParameter("@IDPreparat", preparatID);
            var alergenIDParam = new SqlParameter("@IDAlergen", alergenID);
            Database.ExecuteSqlRawAsync("EXEC AddPreparatAlergen @IDPreparat, @IDAlergen", preparatIDParam, alergenIDParam);
        }

        public int AddUtilizator(string firstName, string lastName, string email, string phone, string deliveryAddress, string password, string type)
        {
            var firstNameParam = new SqlParameter("@Nume", firstName);
            var lastNameParam = new SqlParameter("@Prenume", lastName);
            var emailParam = new SqlParameter("@Email", email);
            var phoneParam = new SqlParameter("@Telefon", phone);
            var deliveryAddressParam = new SqlParameter("@AdresaLivrare", deliveryAddress);
            var passwordParam = new SqlParameter("@Parola", password);
            var typeParam = new SqlParameter("@TipUtilizator", type);
            var idParam = new SqlParameter("@IDUtilizator", SqlDbType.Int) { Direction = ParameterDirection.Output };
            Database.ExecuteSqlRawAsync("EXEC AddUtilizator @Nume, @Prenume, @Email, @Telefon, @AdresaLivrare, @Parola, @TipUtilizator, @IDUtilizator OUTPUT", firstNameParam, lastNameParam, emailParam, phoneParam, deliveryAddressParam, passwordParam, typeParam, idParam);
            return (int)idParam.Value;
        }
    }
}
