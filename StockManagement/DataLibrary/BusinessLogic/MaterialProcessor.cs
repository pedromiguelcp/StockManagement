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
    public static class MaterialProcessor
    {
        public static int CreateMaterial(string Name_Material, string Specs_Material, int Id_Section)
        {
            Specs_Material = Specs_Material ?? "Nada a registar";

            MaterialModel data = new MaterialModel
            {
                Name_Material = Name_Material,
                Specs_Material = Specs_Material,
                Id_Section = Id_Section
            };

            string sql = @"insert into dbo.Material (Name_Material, Specs_Material, Id_Section)
                        values (@Name_Material, @Specs_Material, @Id_Section);";

            return SqlDataAccess.SaveData(sql, data);
        }


        public static int EditMaterial(int Id_Material_to_edit, string new_Name_Material, string new_Specs_Material, int new_Id_Section)
        {
                new_Specs_Material = new_Specs_Material ?? "Nada a registar";

            MaterialModel data = new MaterialModel();

                data.Id_Material = Id_Material_to_edit;
                data.Name_Material = new_Name_Material;
                data.Specs_Material = new_Specs_Material;
                data.Id_Section = new_Id_Section;

                string sql = @"update dbo.Material set Name_Material = @Name_Material, Specs_Material = @Specs_Material, Id_Section = @Id_Section where Id_Material = @Id_Material;";

                return SqlDataAccess.SaveData(sql, data);
        }


        public static int DeleteMaterial(string Name_Material)
        {
            MaterialModel data = new MaterialModel
            {
                Name_Material = Name_Material
            };

            string sql = @"delete from dbo.Material where Name_Material = @Name_Material;";

            return SqlDataAccess.SaveData(sql, data);
        }


        public static int DeleteMaterialById(int Id_Material)
        {
            MaterialModel data = new MaterialModel
            {
                Id_Material = Id_Material
            };

            StockMaterialProcessor.DeleteMaterialfromStockMaterials(Id_Material);
            SupplyProcessor.DeleteMaterialfromSupplys(Id_Material);
            MachineMaterialProcessor.DeleteMaterialfromMachineMaterial(Id_Material);

            string sql = @"delete from dbo.Material where Id_Material = @Id_Material;";

            return SqlDataAccess.SaveData(sql, data);
        }


        public static List<MaterialModel> LoadMaterials()
        {
            string sql = @"select Id_Material, Name_Material ,Specs_Material, Id_Section
                            from dbo.Material;";

            return SqlDataAccess.LoadData<MaterialModel>(sql);
        }

        public static MaterialModel LoadMaterialById(int Id_Material)
        {
            MaterialModel data = new MaterialModel();
            data.Id_Material = Id_Material;

            string sql = @"select Id_Material, Name_Material ,Specs_Material, Id_Section
                            from dbo.Material where Id_Material = @Id_Material;";

            return SqlDataAccess.LoadOneData<MaterialModel>(sql, data);
        }

        public static MaterialModel LoadMaterialByName(string Name_Material)
        {
            MaterialModel data = new MaterialModel();
            data.Name_Material = Name_Material;

            string sql = @"select Id_Material, Name_Material ,Specs_Material, Id_Section
                            from dbo.Material where Name_Material = @Name_Material;";

            return SqlDataAccess.LoadOneData<MaterialModel>(sql, data);
        }

        public static List<MaterialModel> LoadMaterialsBySection(int Id_Section)
        {
            string sql = @"select Id_Material, Name_Material ,Specs_Material, Id_Section
                            from dbo.Material where Id_Section = " + Id_Section + ";";

            return SqlDataAccess.LoadData<MaterialModel>(sql);
        }
    }
}
