using DataLibrary.DataAccess;
using DataLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.BusinessLogic
{
    public static class MachineMaterialProcessor
    {
        public static int CreateMachineMaterial(int Id_Machine, int Id_Material, int Amount_Material, decimal Cost_Material, string Observations, int Id_Provider, int Request, bool Priority, int MissingMaterial)
        {//se ja existir mesmo idmaquina e id material tenho de atualizar o amount   Observations nao sei
            Observations = Observations ?? "Nada a registar";

            MachineMaterialModel data = new MachineMaterialModel
            {
                Id_MachineMaterial = 0,
                Id_Machine = Id_Machine,
                Id_Material = Id_Material,
                Amount_Material = Amount_Material,
                Cost_Material = Cost_Material,
                Observations = Observations,
                MissingMaterial = MissingMaterial,
                Id_Provider = Id_Provider,
                Request = Request,
                Priority = Priority
            };

            string sql;
            //List<int> ExistentId_MachineMaterial = ExistentMachineMaterial(Id_Machine, Id_Material);

                sql = @"insert into dbo.MachineMaterial (Id_Machine, Id_Material, Amount_Material, Cost_Material, Observations, MissingMaterial, Priority, Id_Provider, Request)
                            values (@Id_Machine, @Id_Material, @Amount_Material, @Cost_Material, @Observations, @MissingMaterial, @Priority, @Id_Provider, @Request);";

            return SqlDataAccess.SaveData(sql, data);
        }

        public static List<int> ExistentMachineMaterial(int Id_Machine, int Id_Material)
        {
            string sql = @"select Id_MachineMaterial 
                            from dbo.MachineMaterial where Id_Machine = " + Id_Machine + " and Id_Material = " + Id_Material + ";";

            return SqlDataAccess.LoadData<int>(sql);
        }

        public static int DeleteMachineMaterial(int Id_Machine, int Id_Material)
        {
            MachineMaterialModel data = new MachineMaterialModel
            {
                Id_Machine = Id_Machine,
                Id_Material = Id_Material
            };
            //se eliminar um projeto ou uma maquina tenho de tirar desta tabela tbm

            string sql = @"delete from dbo.MachineMaterial where Id_Machine = @Id_Machine and Id_Material = @Id_Material;";

            return SqlDataAccess.SaveData(sql, data);
        }

        public static int EditMachineMaterial(int Id_Machine, int Id_Material, int MissingMaterial)
        {
            MachineMaterialModel data = new MachineMaterialModel
            {
                Id_Machine = Id_Machine,
                Id_Material = Id_Material,
                MissingMaterial = MissingMaterial
            };
            //se eliminar um projeto ou uma maquina tenho de tirar desta tabela tbm

            string sql = @"update dbo.MachineMaterial set MissingMaterial = @MissingMaterial where Id_Machine = @Id_Machine and Id_Material = @Id_Material;";

            return SqlDataAccess.SaveData(sql, data);
        }

        public static int DeleteMachineMaterials(int Id_Machine)
        {
            MachineMaterialModel data = new MachineMaterialModel
            {
                Id_Machine = Id_Machine
            };
            //se eliminar um projeto ou uma maquina tenho de tirar desta tabela tbm

            string sql = @"delete from dbo.MachineMaterial where Id_Machine = @Id_Machine;";

            return SqlDataAccess.SaveData(sql, data);
        }

        public static List<MachineMaterialModel> LoadMachineMaterialsById(int Id_Machine)
        {
            string sql = @"select Id_MachineMaterial, Id_Machine, Id_Material, Amount_Material, Cost_Material, Observations, MissingMaterial, Priority, Id_Provider, Request
                            from dbo.MachineMaterial where Id_Machine = " + Id_Machine + ";";

            return SqlDataAccess.LoadData<MachineMaterialModel>(sql);
        }

        public static List<MachineMaterialModel> LoadMachineMaterials()
        {
            string sql = @"select Id_MachineMaterial, Id_Machine, Id_Material, Amount_Material, Cost_Material, Observations, MissingMaterial, Priority, Id_Provider, Request
                            from dbo.MachineMaterial;";

            return SqlDataAccess.LoadData<MachineMaterialModel>(sql);
        }

        public static int DeleteMachineMaterialsByProject(int Id_Project)
        {
            List<MachineModel> AllMachineProject = MachineProcessor.LoadMachinesByProject(Id_Project);
            foreach (var row in AllMachineProject)
                DeleteMachineMaterials(row.Id_Machine);
            return 0;
        }

        public static int DeleteMaterialfromMachineMaterial(int Id_Material)
        {
            MachineMaterialModel data = new MachineMaterialModel();
            data.Id_Material = Id_Material;

            string sql = @"delete from dbo.MachineMaterial where Id_Material = @Id_Material;";

            return SqlDataAccess.SaveData(sql, data);
        }

        public static int DeleteProviderfromMachineMaterial(int Id_Provider)
        {
            MachineMaterialModel data = new MachineMaterialModel();
            data.Id_Provider = Id_Provider;

            string sql = @"delete from dbo.MachineMaterial where Id_Provider = @Id_Provider;";

            return SqlDataAccess.SaveData(sql, data);
        }
    }
}
