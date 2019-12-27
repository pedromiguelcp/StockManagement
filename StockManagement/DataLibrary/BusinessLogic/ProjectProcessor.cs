using DataLibrary.DataAccess;
using DataLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.BusinessLogic
{
    public static class ProjectProcessor
    {
        public static int CreateProject(string Name_Project, string Description_Project, decimal TotalCost_Project)
        {
            if (LoadProjectByName(Name_Project) == null)
            {
                Description_Project = Description_Project ?? "Nada a registar";

                ProjectModel data = new ProjectModel
                {
                    Id_Project = 0,
                    Name_Project = Name_Project,
                    Description_Project = Description_Project,
                    TotalCost_Project = TotalCost_Project
                };

                string sql = @"insert into dbo.Project (Name_Project, Description_Project, TotalCost_Project)
                            values (@Name_Project, @Description_Project, @TotalCost_Project);";

                return SqlDataAccess.SaveData(sql, data);
            }
            else
                return 0;
        }


        public static int EditProject(int Id_Project_to_edit, string new_Name_Project, string new_Description_Project, decimal new_TotalCost_Project)
        {
            ProjectModel data = LoadProjectByName(new_Name_Project);
            ProjectModel auxdata = LoadProjectById(Id_Project_to_edit);

            if (data == null || auxdata.Name_Project == new_Name_Project)
            {
                new_Description_Project = new_Description_Project ?? "Nada a registar";

                data = new ProjectModel();

                data.Id_Project = Id_Project_to_edit;
                data.Name_Project = new_Name_Project;
                data.Description_Project = new_Description_Project;
                data.TotalCost_Project = new_TotalCost_Project;

                string sql = @"update dbo.Project set Name_Project = @Name_Project, Description_Project = @Description_Project, TotalCost_Project = @TotalCost_Project
                             where Id_Project = @Id_Project;";

                return SqlDataAccess.SaveData(sql, data);
            }
            else
                return 0;
        }


        public static int DeleteProject(string Name_Project)
        {
            ProjectModel data = new ProjectModel();
            data = LoadProjectByName(Name_Project);

            MachineProcessor.DeleteProjectfromMachines(LoadProjectByName(Name_Project).Id_Project);

            string sql = @"delete from dbo.Project where Name_Project = @Name_Project;";

            return SqlDataAccess.SaveData(sql, data);
        }


        public static List<ProjectModel> LoadProjects()
        {
            string sql = @"select Id_Project, Name_Project, Description_Project, TotalCost_Project 
                            from dbo.Project;";

            return SqlDataAccess.LoadData<ProjectModel>(sql);
        }

        public static ProjectModel LoadProjectById(int Id_Project)
        {
            ProjectModel data = new ProjectModel();
            data.Id_Project = Id_Project;

            string sql = @"select Id_Project, Name_Project, Description_Project, TotalCost_Project 
                            from dbo.Project where Id_Project = @Id_Project;";

            return SqlDataAccess.LoadOneData<ProjectModel>(sql, data);
        }

        public static ProjectModel LoadProjectByName(string Name_Project)
        {
            ProjectModel data = new ProjectModel();
            data.Name_Project = Name_Project;

            string sql = @"select Id_Project, Name_Project, Description_Project, TotalCost_Project 
                            from dbo.Project where Name_Project = @Name_Project;";

            return SqlDataAccess.LoadOneData<ProjectModel>(sql, data);
        }

    }
}
