using DataLibrary.DataAccess;
using DataLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.BusinessLogic
{
    public static class MachineProcessor
    {
        public static int CreateMachine(string Name_Machine, string Description_Machine, decimal TotalCost_Machine, int Id_Project)
        {
            if (LoadMachineByName(Name_Machine) == null)
            {
                Description_Machine = Description_Machine ?? "Nada a registar";

                MachineModel data = new MachineModel
                {
                    Name_Machine = Name_Machine,
                    Description_Machine = Description_Machine,
                    TotalCost_Machine = TotalCost_Machine,
                    Id_Project = Id_Project
                };

                string sql = @"insert into dbo.Machine (Name_Machine, Description_Machine, TotalCost_Machine, Id_Project)
                            values (@Name_Machine, @Description_Machine, @TotalCost_Machine, @Id_Project);";

                return SqlDataAccess.SaveData(sql, data);
            }
            else
                return 0;
        }


        public static int EditMachine(int Id_Machine_to_edit, string new_Name_Machine, string new_Description_Machine, decimal new_TotalCost_Machine, int new_Id_Project)
        {
            MachineModel data = LoadMachineByName(new_Name_Machine);
            MachineModel auxdata = LoadMachineById(Id_Machine_to_edit);

            if (data == null || auxdata.Name_Machine == new_Name_Machine)
            {
                new_Description_Machine = new_Description_Machine ?? "Nada a registar";

                data = new MachineModel();

                data.Id_Machine = Id_Machine_to_edit;
                data.Name_Machine = new_Name_Machine;
                data.Description_Machine = new_Description_Machine;
                data.TotalCost_Machine = new_TotalCost_Machine;
                data.Id_Project = new_Id_Project;

                string sql = @"update dbo.Machine set Name_Machine = @Name_Machine, Description_Machine = @Description_Machine, TotalCost_Machine = @TotalCost_Machine, Id_Project = @Id_Project where Id_Machine = @Id_Machine;";

                return SqlDataAccess.SaveData(sql, data);
            }
            else
                return 0;
        }


        public static int DeleteMachine(string Name_Machine)
        {
            MachineModel data = new MachineModel();
            data = LoadMachineByName(Name_Machine);

            string sql = @"delete from dbo.Machine where Name_Machine = @Name_Machine;";

            return SqlDataAccess.SaveData(sql, data);
        }

        public static int DeleteProjectfromMachines(int Id_Project)
        {
            MachineModel data = new MachineModel();
            data.Id_Project = Id_Project;

            string sql = @"delete from dbo.Machine where Id_Project = @Id_Project;";

            return SqlDataAccess.SaveData(sql, data);
        }


        public static List<MachineModel> LoadMachines()
        {
            string sql = @"select Id_Machine, Name_Machine, Description_Machine, TotalCost_Machine, Id_Project 
                            from dbo.Machine;";

            return SqlDataAccess.LoadData<MachineModel>(sql);
        }

        public static List<MachineModel> LoadMachinesByProject(int Id_Project)
        {
            string sql = @"select Id_Machine, Name_Machine, Description_Machine, TotalCost_Machine, Id_Project 
                            from dbo.Machine where Id_Project = " + Id_Project + ";";

            return SqlDataAccess.LoadData<MachineModel>(sql);
        }

        public static MachineModel LoadMachineByName(string Name_Machine)
        {
            MachineModel data = new MachineModel();
            data.Name_Machine = Name_Machine;

            string sql = @"select Id_Machine, Name_Machine, Description_Machine, TotalCost_Machine, Id_Project 
                            from dbo.Machine where Name_Machine = @Name_Machine;";

            return SqlDataAccess.LoadOneData<MachineModel>(sql, data);
        }

        public static MachineModel LoadMachineById(int Id_Machine)
        {
            MachineModel data = new MachineModel();
            data.Id_Machine = Id_Machine;

            string sql = @"select Id_Machine, Name_Machine, Description_Machine, TotalCost_Machine, Id_Project 
                            from dbo.Machine where Id_Machine = @Id_Machine;";

            return SqlDataAccess.LoadOneData<MachineModel>(sql, data);
        }

    }
}
