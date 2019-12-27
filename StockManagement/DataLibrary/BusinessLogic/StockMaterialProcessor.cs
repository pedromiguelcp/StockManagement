using DataLibrary.DataAccess;
using DataLibrary.Models;
using DataLibrary.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.BusinessLogic
{
    public static class StockMaterialProcessor
    {
        public static int CreateStockMaterial(int Id_Material, int Id_Provider, int Id_Section, int Amount_Material, string Observations)
        {
            Observations = Observations ?? "Nada a registar";

            StockMaterialModel data = new StockMaterialModel
            {
                Id_StockMaterial = 0,
                Id_Material = Id_Material,
                Id_Provider = Id_Provider,
                Id_Section = Id_Section,
                Amount_Material = Amount_Material,
                Observations = Observations
            };

            string sql;
            List<int> ExistentId_StockMaterial = ExistentStockMaterial(Id_Material, Id_Provider, Id_Section);

            if (ExistentId_StockMaterial.Count == 0)
            {
                sql = @"insert into dbo.StockMaterial (Id_Material, Id_Provider, Id_Section, Amount_Material, Observations)
                            values (@Id_Material, @Id_Provider, @Id_Section, @Amount_Material, @Observations);";
            }
            else
            {
                data.Id_StockMaterial = ExistentId_StockMaterial[0];

                sql = @"update dbo.StockMaterial set Amount_Material = Amount_Material + @Amount_Material
                            where Id_StockMaterial = @Id_StockMaterial;";//nao estou a fazer nada com as observations
            }

            return SqlDataAccess.SaveData(sql, data);
        }


        public static int EditStockMaterial(int Id_StockMaterial_to_edit, int new_Id_Provider, int new_Id_Material, int new_Id_Section, int new_Amount_Material, string new_Observations)
        {
            new_Observations = new_Observations ?? "Nada a registar";

            StockMaterialModel data = new StockMaterialModel
            {
                Id_StockMaterial = Id_StockMaterial_to_edit,
                Id_Provider = new_Id_Provider,
                Id_Material = new_Id_Material,
                Id_Section = new_Id_Section,
                Amount_Material = new_Amount_Material,
                Observations = new_Observations
            };

            string sql = @"update dbo.StockMaterial set Id_Provider = @Id_Provider, Id_Material = @Id_Material, Id_Section = @Id_Section, Amount_Material = @Amount_Material, Observations = @Observations where Id_StockMaterial = @Id_StockMaterial;";

            return SqlDataAccess.SaveData(sql, data);
        }


        public static int DeleteProviderfromStockMaterials(int Id_Provider)
        {
            StockMaterialModel data = new StockMaterialModel();
            data.Id_Provider = Id_Provider;

            string sql = @"delete from dbo.StockMaterial where Id_Provider = @Id_Provider;";

            return SqlDataAccess.SaveData(sql, data);
        }

        public static int DeleteMaterialfromStockMaterials(int Id_Material)
        {
            StockMaterialModel data = new StockMaterialModel();
            data.Id_Material = Id_Material;

            string sql = @"delete from dbo.StockMaterial where Id_Material = @Id_Material;";

            return SqlDataAccess.SaveData(sql, data);
        }

        public static int DeleteSectionfromStockMaterials(int Id_Section)
        {
            StockMaterialModel data = new StockMaterialModel();
            data.Id_Section = Id_Section;

            string sql = @"delete from dbo.StockMaterial where Id_Section = @Id_Section;";

            return SqlDataAccess.SaveData(sql, data);
        }

        public static int DeleteStockMaterial(string Name_Provider, string Name_Material, string Name_Section)
        {
            StockMaterialModel data = new StockMaterialModel
            {
                Id_Provider = ProviderProcessor.LoadProviderByName(Name_Provider).Id_Provider,
                Id_Material = MaterialProcessor.LoadMaterialByName(Name_Material).Id_Material,
                Id_Section = SectionProcessor.LoadSectionByName(Name_Section).Id_Section
            };

            string sql = @"delete from dbo.StockMaterial where Id_Provider = @Id_Provider and Id_Material = @Id_Material and Id_Section = @Id_Section;";

            return SqlDataAccess.SaveData(sql, data);
        }


        public static List<StockMaterialModel> LoadStockMaterials(int Id_Section, int Id_Provider)
        {
            string sql = @"select Id_StockMaterial, Id_Material, Id_Provider, Id_Section, Amount_Material, Observations 
                            from dbo.StockMaterial where Id_Section = " + Id_Section + " and Id_Provider = " + Id_Provider + ";";

            return SqlDataAccess.LoadData<StockMaterialModel>(sql);
        }

        public static List<StockMaterialModel> LoadOrderStockMaterials()
        {
            List<SectionModel> AllSections = SectionProcessor.LoadSections();
            int auxsections = 0;

            List<ProviderModel> AllProviders = ProviderProcessor.LoadProviders();
            int auxproviders = 0;

            List<StockMaterialModel> OrderStockMaterials = new List<StockMaterialModel>();
            
            while((AllSections.Count() - auxsections) != 0)
            {
                while((AllProviders.Count() - auxproviders) != 0)
                {
                    OrderStockMaterials.AddRange(LoadStockMaterials(AllSections[auxsections].Id_Section, AllProviders[auxproviders].Id_Provider));
                    auxproviders++;
                }
                auxproviders = 0;
                auxsections++;
            }
            return OrderStockMaterials;
        }

        public static List<StockMaterialModel> LoadStockMaterialByNames(string Name_Provider, string Name_Material, string Name_Section)
        {
            int Id_Provider = ProviderProcessor.LoadProviderByName(Name_Provider).Id_Provider;
            //int Id_Material = MaterialProcessor.LoadMaterialProvider(Name_Material, Id_Provider).Id_Material;
            int Id_Section = SectionProcessor.LoadSectionByName(Name_Section).Id_Section;

            string sql = @"select Id_StockMaterial, Id_Material, Id_Provider, Id_Section, Amount_Material, Observations 
                            from dbo.StockMaterial where Id_Provider = " + Id_Provider +/* " and Id_Material = " + Id_Material +*/ " and Id_Section = " + Id_Section + ";";

            return SqlDataAccess.LoadData<StockMaterialModel>(sql);
        }

        public static List<int> ExistentStockMaterial(int Id_Material, int Id_Provider, int Id_Section)
        {
            string sql = @"select Id_StockMaterial 
                            from dbo.StockMaterial where Id_Material = " + Id_Material + " and Id_Provider = " + Id_Provider + " and Id_Section = " + Id_Section + ";";

            return SqlDataAccess.LoadData<int>(sql);
        }

        public static StockMaterialModel StockMaterialAmount(int Id_Material)
        {
            string sql = @"select Id_StockMaterial, Id_Material, Id_Provider, Id_Section, Amount_Material, Observations 
                            from dbo.StockMaterial where Id_Material = " + Id_Material + ";";

            List < StockMaterialModel > data = SqlDataAccess.LoadData<StockMaterialModel>(sql);

            if(data.Count()!=0)
                return data[0];
            StockMaterialModel model = new StockMaterialModel();
            model.Amount_Material = 0;
            return model;

        }

    }
}
