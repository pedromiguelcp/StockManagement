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
    public static class ProviderProcessor
    {
        public static int CreateProvider(string Name_Provider, string Phone_Provider, string Mail_Provider, string Address_Provider, string Nif_Provider, int Language_Provider)
        {
            if (LoadProviderByName(Name_Provider) == null)
            {
                Phone_Provider = Phone_Provider ?? "Nada a registar";
                Mail_Provider = Mail_Provider ?? "Nada a registar";
                Address_Provider = Address_Provider ?? "Nada a registar";
                Nif_Provider = Nif_Provider ?? "Nada a registar";

                ProviderModel data = new ProviderModel
                {
                    Id_Provider = 0,
                    Name_Provider = Name_Provider,
                    Phone_Provider = Phone_Provider,
                    Mail_Provider = Mail_Provider,
                    Address_Provider = Address_Provider,
                    Nif_Provider = Nif_Provider,
                    Language_Provider = (Language)Language_Provider
                };

                string sql = @"insert into dbo.Provider (Name_Provider, Phone_Provider, Mail_Provider, Address_Provider, Nif_Provider, Language_Provider)
                            values (@Name_Provider, @Phone_Provider, @Mail_Provider, @Address_Provider, @Nif_Provider, @Language_Provider);";

                return SqlDataAccess.SaveData(sql, data);
            }
            else
                return 0;
        }


        public static int EditProvider(int Id_Provider_to_edit, string new_Name_Provider, string new_Phone_Provider, string new_Mail_Provider, string new_Address_Provider, string new_Nif_Provider, int new_Language_Provider)
        {
            ProviderModel data = LoadProviderByName(new_Name_Provider);
            ProviderModel auxdata = LoadProviderById(Id_Provider_to_edit);

            if (data == null || auxdata.Name_Provider == new_Name_Provider)
            {
                new_Phone_Provider = new_Phone_Provider ?? "Nada a registar";
                new_Mail_Provider = new_Mail_Provider ?? "Nada a registar";
                new_Address_Provider = new_Address_Provider ?? "Nada a registar";
                new_Nif_Provider = new_Nif_Provider ?? "Nada a registar";

                data = new ProviderModel();

                data.Id_Provider = Id_Provider_to_edit;
                data.Name_Provider = new_Name_Provider;
                data.Phone_Provider = new_Phone_Provider;
                data.Mail_Provider = new_Mail_Provider;
                data.Address_Provider = new_Address_Provider;
                data.Nif_Provider = new_Nif_Provider;
                data.Language_Provider = (Language)new_Language_Provider;

                string sql = @"update dbo.Provider set Name_Provider = @Name_Provider, Phone_Provider = @Phone_Provider, Mail_Provider = @Mail_Provider, Address_Provider = @Address_Provider, Nif_Provider = @Nif_Provider, Language_Provider = @Language_Provider where Id_Provider = @Id_Provider;";

                return SqlDataAccess.SaveData(sql, data);
            }
            else
                return 0;
        }


        public static int DeleteProvider(string Name_Provider)
        {
            ProviderModel data = new ProviderModel();
            data = LoadProviderByName(Name_Provider);

            string sql = @"delete from dbo.Provider where Name_Provider = @Name_Provider;";

            return SqlDataAccess.SaveData(sql, data);
        }


        public static List<ProviderModel> LoadProviders()
        {
            string sql = @"select Id_Provider, Name_Provider, Phone_Provider, Mail_Provider, Address_Provider, Nif_Provider, Language_Provider
                            from dbo.Provider;";

            return SqlDataAccess.LoadData<ProviderModel>(sql);
        }

        public static ProviderModel LoadProviderById(int Id_Provider)
        {
            ProviderModel data = new ProviderModel();
            data.Id_Provider = Id_Provider;

            string sql = @"select Id_Provider, Name_Provider, Phone_Provider, Mail_Provider, Address_Provider, Nif_Provider, Language_Provider 
                            from dbo.Provider where Id_Provider = @Id_Provider;";

            return SqlDataAccess.LoadOneData<ProviderModel>(sql, data);
        }

        public static ProviderModel LoadProviderByName(string Name_Provider)
        {
            ProviderModel data = new ProviderModel();
            data.Name_Provider = Name_Provider;

            string sql = @"select Id_Provider, Name_Provider, Phone_Provider, Mail_Provider, Address_Provider, Nif_Provider, Language_Provider
                            from dbo.Provider where Name_Provider = @Name_Provider;";

            return SqlDataAccess.LoadOneData<ProviderModel>(sql, data);
        }
    }
}
