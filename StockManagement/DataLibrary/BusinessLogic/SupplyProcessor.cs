using DataLibrary.DataAccess;
using DataLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.BusinessLogic
{
    public static class SupplyProcessor
    {
        public static int CreateSupply(int Id_Provider, int Id_Material, int Id_Section, int Amount_Material, decimal TotalPrice_Material, decimal TotalPrice_Supply, DateTime Date_Supply, int Request, int SupplyOrder)
        {
            SupplyModel data = new SupplyModel
            {
                Id_Provider = Id_Provider,
                Id_Material = Id_Material,
                Id_Section = Id_Section,
                Amount_Material = Amount_Material,
                TotalPrice_Material = TotalPrice_Material,
                TotalPrice_Supply = TotalPrice_Supply,
                Date_Supply = Date_Supply,
                Request = Request,
                SupplyOrder = SupplyOrder
            };

            string sql = @"insert into dbo.Supply (Id_Provider, Id_Material, Id_Section, Amount_Material, TotalPrice_Supply, Date_Supply, Request, TotalPrice_Material, SupplyOrder)
                            values (@Id_Provider, @Id_Material, @Id_Section, @Amount_Material, @TotalPrice_Supply, @Date_Supply, @Request, @TotalPrice_Material, @SupplyOrder);";

            return SqlDataAccess.SaveData(sql, data);
        }

        public static int DeleteProviderfromSupplys(int Id_Provider)
        {
            StockMaterialModel data = new StockMaterialModel();
            data.Id_Provider = Id_Provider;

            string sql = @"delete from dbo.Supply where Id_Provider = @Id_Provider;";

            return SqlDataAccess.SaveData(sql, data);
        }

        public static int DeleteMaterialfromSupplys(int Id_Material)
        {
            StockMaterialModel data = new StockMaterialModel();
            data.Id_Material = Id_Material;

            string sql = @"delete from dbo.Supply where Id_Material = @Id_Material;";

            return SqlDataAccess.SaveData(sql, data);
        }

        public static int DeleteSectionfromSupplys(int Id_Section)
        {
            StockMaterialModel data = new StockMaterialModel();
            data.Id_Section = Id_Section;

            string sql = @"delete from dbo.Supply where Id_Section = @Id_Section;";

            return SqlDataAccess.SaveData(sql, data);
        }

        public static int DeleteSupplyByRequest(int Request)
        {
            SupplyModel data = new SupplyModel();
            data.Request = Request;

            string sql = @"delete from dbo.Supply where Request = @Request;";

            return SqlDataAccess.SaveData(sql, data);
        }

        public static List<SupplyModel> LoadSupplys()
        {
            string sql = @"select Id_Provider, Id_Material, Id_Section, Amount_Material, TotalPrice_Supply, Date_Supply, Request, TotalPrice_Material, SupplyOrder
                        from dbo.Supply;";

            return SqlDataAccess.LoadData<SupplyModel>(sql);
        }
    }
}
