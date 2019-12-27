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
    public static class SectionProcessor
    {
        public static int CreateSection(string Name_Section, string Description_Section)
        {
            if (LoadSectionByName(Name_Section) == null)
            {
                Description_Section = Description_Section ?? "Nada a registar";

                SectionModel data = new SectionModel
                {
                    Name_Section = Name_Section,
                    Description_Section = Description_Section
                };

                string sql = @"insert into dbo.Section (Name_Section, Description_Section)
                            values (@Name_Section, @Description_Section);";

                return SqlDataAccess.SaveData(sql, data);
            }
            else
                return 0;
        }


        public static int EditSection(int Id_Section_to_edit, string new_Name_Section, string new_Description_Section)
        {
            SectionModel data = LoadSectionByName(new_Name_Section);
            SectionModel auxdata = LoadSectionById(Id_Section_to_edit);

            if (data == null || auxdata.Name_Section == new_Name_Section)
            {
                new_Description_Section = new_Description_Section ?? "Nada a registar";

                data = new SectionModel();

                data.Id_Section = Id_Section_to_edit;
                data.Name_Section = new_Name_Section;
                data.Description_Section = new_Description_Section;

                string sql = @"update dbo.Section set Name_Section = @Name_Section, Description_Section = @Description_Section where Id_Section = @Id_Section;";

                return SqlDataAccess.SaveData(sql, data);
            }
            else
                return 0;
        }


        public static int DeleteSection(string Name_Section)
        {
            SectionModel data = new SectionModel
            {
                Name_Section = Name_Section
            };

            StockMaterialProcessor.DeleteSectionfromStockMaterials(LoadSectionByName(Name_Section).Id_Section);
            SupplyProcessor.DeleteSectionfromSupplys(LoadSectionByName(Name_Section).Id_Section);

            string sql = @"delete from dbo.Section where Name_Section = @Name_Section;";

            return SqlDataAccess.SaveData(sql, data);
        }


        public static List<SectionModel> LoadSections()
        {
            string sql = @"select Id_Section, Name_Section, Description_Section 
                            from dbo.Section;";

            return SqlDataAccess.LoadData<SectionModel>(sql);
        }

        public static SectionModel LoadSectionById(int Id_Section)
        {
            SectionModel data = new SectionModel();
            data.Id_Section = Id_Section;

            string sql = @"select Id_Section, Name_Section ,Description_Section 
                            from dbo.Section where Id_Section = @Id_Section;";

            return SqlDataAccess.LoadOneData<SectionModel>(sql, data);
        }

        public static SectionModel LoadSectionByName(string Name_Section)
        {
            SectionModel data = new SectionModel();
            data.Name_Section = Name_Section;

            string sql = @"select Id_Section, Name_Section ,Description_Section 
                            from dbo.Section where Name_Section = @Name_Section;";

            return SqlDataAccess.LoadOneData<SectionModel>(sql, data);
        }
    }
}
