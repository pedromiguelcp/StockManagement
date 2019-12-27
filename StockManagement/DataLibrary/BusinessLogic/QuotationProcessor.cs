using DataLibrary.DataAccess;
using DataLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.BusinessLogic
{
    public static class QuotationProcessor
    {
        public static List<QuotationModel> LoadQuotationByProvider(int Id_Provider)
        {
            string sql = @"select Id_Quotation, Id_Provider, Id_Material, ExpirationDate, QuotationPath 
                            from dbo.Quotation where Id_Provider = " + Id_Provider + ";";

            return SqlDataAccess.LoadData<QuotationModel>(sql);
        }

        public static int CreateQuotation(int Id_Provider, int Id_Material, DateTime ExpirationDate, string QuotationPath)
        {
            QuotationModel data = new QuotationModel
            {
                Id_Provider = Id_Provider,
                Id_Material = Id_Material,
                ExpirationDate = ExpirationDate,
                QuotationPath = QuotationPath
            };

            string sql = @"insert into dbo.Quotation (Id_Provider, Id_Material, ExpirationDate, QuotationPath)
                            values (@Id_Provider, @Id_Material, @ExpirationDate, @QuotationPath);";

            return SqlDataAccess.SaveData(sql, data);
        }

        public static int DeleteQuotation(int Id_Quotation)
        {
            QuotationModel data = new QuotationModel();
            data.Id_Quotation = Id_Quotation;

            string sql = @"delete from dbo.Quotation where Id_Quotation = @Id_Quotation;";

            return SqlDataAccess.SaveData(sql, data);
        }
    }
}
