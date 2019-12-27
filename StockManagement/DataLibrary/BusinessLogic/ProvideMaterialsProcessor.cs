using DataLibrary.DataAccess;
using DataLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.BusinessLogic
{
    public static class ProvideMaterialsProcessor
    {
        public static int CreateProvideMaterial(int Id_Provider, int Id_Material, decimal Unit_Cost, DateTime ExpirationDate, string QuotationPath)
        {
            ProvideMaterialsModel ProvideMaterial = LoadProvideMaterial(Id_Provider, Id_Material);

            if (ProvideMaterial != null)
              return 0;

            ProvideMaterialsModel data = new ProvideMaterialsModel
            {
                Id_Provider = Id_Provider,
                Id_Material = Id_Material,
                Unit_Cost = Unit_Cost,
                ExpirationDate = ExpirationDate,
                QuotationPath = QuotationPath
            };

            string sql = @"insert into dbo.ProvideMaterials (Id_Provider, Id_Material, Unit_Cost, ExpirationDate, QuotationPath)
                        values (@Id_Provider, @Id_Material, @Unit_Cost, @ExpirationDate, @QuotationPath);";

            return SqlDataAccess.SaveData(sql, data);
        }

        public static ProvideMaterialsModel LoadProvideMaterial(int Id_Provider, int Id_Material)
        {
            ProvideMaterialsModel data = new ProvideMaterialsModel();
            data.Id_Provider = Id_Provider;
            data.Id_Material = Id_Material;

            string sql = @"select Id_Provider, Id_Material, Unit_Cost, ExpirationDate, QuotationPath
                            from dbo.ProvideMaterials where Id_Provider = @Id_Provider and Id_Material = @Id_Material;";

            return SqlDataAccess.LoadOneData<ProvideMaterialsModel>(sql, data);
        }


        public static int EditProvideMaterial(int ProvideMaterial_to_edit, int new_Id_Provider, int new_Id_Material, decimal new_Unit_Cost, DateTime new_ExpirationDate, string new_QuotationPath)
        {
            ProvideMaterialsModel ProvideMaterial = LoadProvideMaterial(new_Id_Provider, new_Id_Material);

            if (ProvideMaterial != null)
                if(ProvideMaterial.Id_ProvideMaterials != ProvideMaterial_to_edit)
                  return 0;


            ProvideMaterial = new ProvideMaterialsModel();

            ProvideMaterial.Id_ProvideMaterials = ProvideMaterial_to_edit;
            ProvideMaterial.Id_Provider = new_Id_Provider;
            ProvideMaterial.Id_Material = new_Id_Material;
            ProvideMaterial.Unit_Cost = new_Unit_Cost;
            ProvideMaterial.ExpirationDate = new_ExpirationDate;
            ProvideMaterial.QuotationPath = new_QuotationPath;

            string sql = @"update dbo.ProvideMaterials set Id_Provider = @Id_Provider, Id_Material = @Id_Material, Unit_Cost = @Unit_Cost, ExpirationDate = @ExpirationDate, QuotationPath = @QuotationPath where Id_ProvideMaterials = @Id_ProvideMaterials;";

            return SqlDataAccess.SaveData(sql, ProvideMaterial);
        }


        public static int DeleteProvideMaterial(int Id_Provider, int Id_Material)
        {
            ProvideMaterialsModel data = new ProvideMaterialsModel
            {
                Id_Provider = Id_Provider,
                Id_Material = Id_Material
            };

            //talvez eliminar de vaarios sitios

            string sql = @"delete from dbo.ProvideMaterials where Id_Provider = @Id_Provider and Id_Material = @Id_Material;";

            return SqlDataAccess.SaveData(sql, data);
        }
        public static int DeleteProvideMaterialByMaterial(int Id_Material)
        {
            ProvideMaterialsModel data = new ProvideMaterialsModel
            {
                Id_Material = Id_Material
            };

            //talvez eliminar de vaarios sitios

            string sql = @"delete from dbo.ProvideMaterials where Id_Material = @Id_Material;";

            return SqlDataAccess.SaveData(sql, data);
        }
        public static int DeleteProviderMaterialByProvider(int Id_Provider)
        {
            ProvideMaterialsModel data = new ProvideMaterialsModel
            {
                Id_Provider = Id_Provider
            };

            //talvez eliminar de vaarios sitios

            string sql = @"delete from dbo.ProvideMaterials where Id_Provider = @Id_Provider;";

            return SqlDataAccess.SaveData(sql, data);
        }


        public static List<ProvideMaterialsModel> LoadProvideMaterials()
        {
            string sql = @"select Id_ProvideMaterials, Id_Provider, Id_Material, Unit_Cost, ExpirationDate, QuotationPath
                            from dbo.ProvideMaterials;";

            return SqlDataAccess.LoadData<ProvideMaterialsModel>(sql);
        }

        public static List<ProvideMaterialsModel> LoadProvideMaterialsByMaterial(int Id_Material)
        {
            string sql = @"select Id_ProvideMaterials, Id_Provider, Id_Material, Unit_Cost, ExpirationDate, QuotationPath
                            from dbo.ProvideMaterials where Id_Material = " + Id_Material + ";";

            return SqlDataAccess.LoadData<ProvideMaterialsModel>(sql);
        }
        public static List<ProvideMaterialsModel> LoadProviderMaterialsByProviders(int Id_Provider)
        {
            string sql = @"select Id_ProvideMaterials, Id_Provider, Id_Material, Unit_Cost, ExpirationDate, QuotationPath
                            from dbo.ProvideMaterials where Id_Provider = " + Id_Provider + ";";

            return SqlDataAccess.LoadData<ProvideMaterialsModel>(sql);
        }
        public static List<ProviderModel> LoadProvidersByMaterial(int Id_Material)
        {
            string sql = @"select Id_ProvideMaterials, Id_Provider, Id_Material, Unit_Cost, ExpirationDate, QuotationPath
                            from dbo.ProvideMaterials where Id_Material = " + Id_Material + ";";

            List<ProvideMaterialsModel> ProvideMaterials = SqlDataAccess.LoadData<ProvideMaterialsModel>(sql);

            List<ProviderModel> ProviderModels = new List<ProviderModel>();

            foreach (var row in ProvideMaterials)
                ProviderModels.Add(new ProviderModel
                {
                    Id_Provider = row.Id_Provider,
                    Name_Provider = ProviderProcessor.LoadProviderById(row.Id_Provider).Name_Provider
                });

            return ProviderModels;
        }
    }
}
