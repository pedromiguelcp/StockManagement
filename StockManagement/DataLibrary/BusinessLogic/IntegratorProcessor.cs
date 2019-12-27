using DataLibrary.DataAccess;
using DataLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.BusinessLogic
{
    public static class IntegratorProcessor
    {
        public static int CreateIntegrator(string Name_Integrator, string Phone_Integrator, string Mail_Integrator, string Address_Integrator, string Nif_Integrator)
        {
            if (LoadIntegratorByName(Name_Integrator) == null)
            {
                Phone_Integrator = Phone_Integrator ?? "Nada a registar";
                Mail_Integrator = Mail_Integrator ?? "Nada a registar";
                Address_Integrator = Address_Integrator ?? "Nada a registar";
                Nif_Integrator = Nif_Integrator ?? "Nada a registar";

                IntegratorModel data = new IntegratorModel
                {
                    Id_Integrator = 0,
                    Name_Integrator = Name_Integrator,
                    Phone_Integrator = Phone_Integrator,
                    Mail_Integrator = Mail_Integrator,
                    Address_Integrator = Address_Integrator,
                    Nif_Integrator = Nif_Integrator
                };

                string sql = @"insert into dbo.Integrator (Name_Integrator, Phone_Integrator, Mail_Integrator, Address_Integrator, Nif_Integrator)
                            values (@Name_Integrator, @Phone_Integrator, @Mail_Integrator, @Address_Integrator, @Nif_Integrator);";

                return SqlDataAccess.SaveData(sql, data);
            }
            else
                return 0;
        }


        public static int EditIntegrator(int Id_Integrator_to_edit, string new_Name_Integrator, string new_Phone_Integrator, string new_Mail_Integrator, string new_Address_Integrator, string new_Nif_Integrator)
        {
            IntegratorModel data = LoadIntegratorByName(new_Name_Integrator);
            IntegratorModel auxdata = LoadIntegratorById(Id_Integrator_to_edit);

            if (data == null || auxdata.Name_Integrator == new_Name_Integrator)
            {
                new_Phone_Integrator = new_Phone_Integrator ?? "Nada a registar";
                new_Mail_Integrator = new_Mail_Integrator ?? "Nada a registar";
                new_Address_Integrator = new_Address_Integrator ?? "Nada a registar";
                new_Nif_Integrator = new_Nif_Integrator ?? "Nada a registar";

                data = new IntegratorModel();

                data.Id_Integrator = Id_Integrator_to_edit;
                data.Name_Integrator = new_Name_Integrator;
                data.Phone_Integrator = new_Phone_Integrator;
                data.Mail_Integrator = new_Mail_Integrator;
                data.Address_Integrator = new_Address_Integrator;
                data.Nif_Integrator = new_Nif_Integrator;

                string sql = @"update dbo.Integrator set Name_Integrator = @Name_Integrator, Phone_Integrator = @Phone_Integrator, Mail_Integrator = @Mail_Integrator, Address_Integrator = @Address_Integrator, Nif_Integrator = @Nif_Integrator where Id_Integrator = @Id_Integrator;";

                return SqlDataAccess.SaveData(sql, data);
            }
            else
                return 0;
        }

        
        public static int DeleteIntegrator(string Name_Integrator)
        {
            IntegratorModel data = new IntegratorModel();
            data = LoadIntegratorByName(Name_Integrator);

            //tenho de eliminar da tabela vendas!!!!
            string sql = @"delete from dbo.Integrator where Name_Integrator = @Name_Integrator;";

            return SqlDataAccess.SaveData(sql, data);
        }


        public static List<IntegratorModel> LoadIntegrators()
        {
            string sql = @"select Id_Integrator, Name_Integrator, Phone_Integrator, Mail_Integrator, Address_Integrator, Nif_Integrator 
                            from dbo.Integrator;";

            return SqlDataAccess.LoadData<IntegratorModel>(sql);
        }

        public static IntegratorModel LoadIntegratorById(int Id_Integrator)
        {
            IntegratorModel data = new IntegratorModel();
            data.Id_Integrator = Id_Integrator;

            string sql = @"select Id_Integrator, Name_Integrator, Phone_Integrator, Mail_Integrator, Address_Integrator, Nif_Integrator 
                            from dbo.Integrator where Id_Integrator = @Id_Integrator;";

            return SqlDataAccess.LoadOneData<IntegratorModel>(sql, data);
        }

        public static IntegratorModel LoadIntegratorByName(string Name_Integrator)
        {
            IntegratorModel data = new IntegratorModel();
            data.Name_Integrator = Name_Integrator;

            string sql = @"select Id_Integrator, Name_Integrator, Phone_Integrator, Mail_Integrator, Address_Integrator, Nif_Integrator 
                            from dbo.Integrator where Name_Integrator = @Name_Integrator;";

            return SqlDataAccess.LoadOneData<IntegratorModel>(sql, data);
        }
    }
}
