using DataLibrary.BusinessLogic;
using StockManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace StockManagement.Controllers
{
    public class ProjectController : Controller
    {
        static int Id_Project_to_edit = new int();
        static string Name_ProjectMachine_to_edit;
        static string Name_MachineMaterials_to_edit;
        static decimal CostMaterialAux;
        static decimal CostMachineAux;
        static bool Create_Flag = false;
        static bool EditonCreate_Flag = false;
        static bool CreateonEdit_Flag = false;

        static ProjectMachineModel ProjectMachine_Model = new ProjectMachineModel();
        static ProjectModel Project_Model = new ProjectModel();
        static List<ViewMachineModel> ViewMachines_Model = new List<ViewMachineModel>();
        static List<DataLibrary.Models.MachineModel> Machines_Model = new List<DataLibrary.Models.MachineModel>();

        static ProjectMachineMaterialModel ProjectMachineMaterial_Model = new ProjectMachineMaterialModel();
        static MachineModel Machine_Model = new MachineModel();
        static List<ViewMachineMaterialModel> ViewMachineMaterial_Model = new List<ViewMachineMaterialModel>();

        static List<ViewMachineMaterialModel> Total_ViewMachineMaterial_Model = new List<ViewMachineMaterialModel>();



        #region Projects
        public ActionResult ViewProjects()
        {
            Project_Model.Name_Project = "";
            Project_Model.Description_Project = "";
            Project_Model.TotalCost_Project = 0;
            Machines_Model.Clear();
            ViewMachines_Model.Clear();
            ProjectMachine_Model.ProjectModel = Project_Model;
            ProjectMachine_Model.ViewMachineModel = ViewMachines_Model;

            CleanMachineMaterialView();
            Total_ViewMachineMaterial_Model.Clear();
            ProjectMachineMaterial_Model.MachineModel = Machine_Model;
            ProjectMachineMaterial_Model.ViewMachineMaterialModel = ViewMachineMaterial_Model;

            EditonCreate_Flag = false;
            CreateonEdit_Flag = false;
            Create_Flag = false;

            List<ProjectModel> Projects = new List<ProjectModel>();

            var data = ProjectProcessor.LoadProjects();

            foreach (var row in data)
                Projects.Add(new ProjectModel
                {
                    Name_Project = row.Name_Project,
                    Description_Project = row.Description_Project,
                    TotalCost_Project = row.TotalCost_Project
                });

            return View(Projects.OrderBy(x => x.Name_Project));
        }


        public ActionResult CreateProject()
        {
            ProjectMachine_Model.ProjectModel = Project_Model;
            ProjectMachine_Model.ViewMachineModel = ViewMachines_Model;
            Create_Flag = true;

            return View(ProjectMachine_Model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateProject(ProjectMachineModel model)
        {
            if (ModelState.IsValid)
            {
                model.ProjectModel.TotalCost_Project = Project_Model.TotalCost_Project;
                if (ProjectProcessor.CreateProject(model.ProjectModel.Name_Project, model.ProjectModel.Description_Project, model.ProjectModel.TotalCost_Project) == 0)
                    ViewBag.Message = "Project not created";
                else
                {
                    foreach(var row in ViewMachines_Model)
                    {
                        MachineProcessor.CreateMachine(row.Name_Machine, row.Description_Machine, row.TotalCost_Machine, ProjectProcessor.LoadProjectByName(model.ProjectModel.Name_Project).Id_Project);
                    }

                    foreach (var row in Total_ViewMachineMaterial_Model)
                    {
                        MachineMaterialProcessor.CreateMachineMaterial(MachineProcessor.LoadMachineByName(row.Name_Machine).Id_Machine, MaterialProcessor.LoadMaterialByName(row.Name_Material).Id_Material, row.Amount_Material, row.Unit_Cost, row.Observations, row.Id_Provider, row.Request, row.Priority, row.MissingMaterial);
                    }

                    ViewBag.Message = "Project created";
                    return RedirectToAction("ViewProjects");
                }
            }
            return View(ProjectMachine_Model);
        }


        public ActionResult EditProject(string Name_Project)
        {
            CreateonEdit_Flag = false;
            if (Name_Project != null)
            {
                DataLibrary.Models.ProjectModel DLProject = ProjectProcessor.LoadProjectByName(Name_Project);
                Id_Project_to_edit = DLProject.Id_Project;

                Project_Model.Name_Project = Name_Project;
                Project_Model.Description_Project = DLProject.Description_Project;
                Project_Model.TotalCost_Project = DLProject.TotalCost_Project;

                Total_ViewMachineMaterial_Model.Clear();

                List<DataLibrary.Models.MachineModel> data = MachineProcessor.LoadMachinesByProject(Id_Project_to_edit);

                foreach (var row in data)
                {
                    ViewMachines_Model.Add(new ViewMachineModel
                    {
                        Name_Machine = row.Name_Machine,
                        Description_Machine = row.Description_Machine,
                        TotalCost_Machine = row.TotalCost_Machine,
                        Name_Project = ProjectProcessor.LoadProjectById(row.Id_Project).Name_Project
                    });

                    Machines_Model.Add(new DataLibrary.Models.MachineModel
                    {
                        Id_Machine = row.Id_Machine,
                        Name_Machine = row.Name_Machine,
                        Description_Machine = row.Description_Machine,
                        TotalCost_Machine = row.TotalCost_Machine,
                        Id_Project = Id_Project_to_edit
                    });

                    List<DataLibrary.Models.MachineMaterialModel> aux = MachineMaterialProcessor.LoadMachineMaterialsById(row.Id_Machine);
                    foreach (var row1 in aux)
                        Total_ViewMachineMaterial_Model.Add(new ViewMachineMaterialModel
                        {
                            Name_Machine = row.Name_Machine,
                            Name_Material = MaterialProcessor.LoadMaterialById(row1.Id_Material).Name_Material,
                            Amount_Material = row1.Amount_Material,
                            Cost_Material = row1.Cost_Material,
                            Observations = row1.Observations,
                            MissingMaterial = row1.MissingMaterial,
                            Id_Provider = row1.Id_Provider,
                            Request = row1.Request,
                            Priority = row1.Priority,
                            Unit_Cost = row1.Cost_Material
                        });
                }
            }
            Create_Flag = false;
            ProjectMachine_Model.ProjectModel = Project_Model;
            ProjectMachine_Model.ViewMachineModel = ViewMachines_Model;

            return View(ProjectMachine_Model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProject(ProjectMachineModel model)
        {
            if (ModelState.IsValid)
            {
                model.ProjectModel.TotalCost_Project = Project_Model.TotalCost_Project;

                if (ProjectProcessor.EditProject(Id_Project_to_edit, model.ProjectModel.Name_Project, model.ProjectModel.Description_Project, model.ProjectModel.TotalCost_Project) == 0)
                    ViewBag.Message = "Project not edited";
                else
                {
                    List<DataLibrary.Models.MachineModel> Allmachines = MachineProcessor.LoadMachinesByProject(Id_Project_to_edit);

                    foreach (var row in Allmachines)//eliminar maquinas que só estao na BD
                        if (Searchmachine(row.Id_Machine) == 0)
                        {
                            MachineProcessor.DeleteMachine(row.Name_Machine);
                            MachineMaterialProcessor.DeleteMachineMaterials(row.Id_Machine);//eliminar todos os materiais associados a essa maquina
                        }

                    foreach (var row in Machines_Model)//update as maquinas e criar as novas maquinas
                        if (MachineProcessor.LoadMachineById(row.Id_Machine) != null)
                            MachineProcessor.EditMachine(row.Id_Machine, row.Name_Machine, row.Description_Machine, row.TotalCost_Machine, Id_Project_to_edit);
                        else
                            MachineProcessor.CreateMachine(row.Name_Machine, row.Description_Machine, row.TotalCost_Machine, Id_Project_to_edit);

                    MachineMaterialProcessor.DeleteMachineMaterialsByProject(Id_Project_to_edit);//limpo tudo e insiro de novo, ou mando uma lista e faz tudo no processor

                    foreach (var row in Total_ViewMachineMaterial_Model)//meter a fazer isto tudo no processor
                        MachineMaterialProcessor.CreateMachineMaterial(MachineProcessor.LoadMachineByName(row.Name_Machine).Id_Machine, MaterialProcessor.LoadMaterialByName(row.Name_Material).Id_Material, row.Amount_Material, row.Unit_Cost, row.Observations, row.Id_Provider, row.Request, row.Priority, row.MissingMaterial);

                    ViewBag.Message = "Project edited";
                    return RedirectToAction("ViewProjects");
                }
            }
            return View(ProjectMachine_Model);//tenho de garantir que tenho neste model a lista de maquinas atualizada
        }


        public ActionResult DeleteProject(string Name_Project)
        {
            DataLibrary.Models.ProjectModel DLProject = ProjectProcessor.LoadProjectByName(Name_Project);

            ProjectModel Project = new ProjectModel();

            Project.Name_Project = Name_Project;
            Project.Description_Project = DLProject.Description_Project;
            Project.TotalCost_Project = DLProject.TotalCost_Project;

            return View(Project);
        }

        [HttpPost]
        public ActionResult DeleteProject(ProjectModel model)
        {
            ProjectProcessor.DeleteProject(model.Name_Project);
            return Redirect("~/Project/ViewProjects");
        }


        public void SaveInfoProjectMachine(ProjectMachineModel model)
        {
            Project_Model.Name_Project = model.ProjectModel.Name_Project;
            Project_Model.Description_Project = model.ProjectModel.Description_Project;
            //Project_Model.TotalCost_Project = model.ProjectModel.TotalCost_Project;
            Project_Model.TotalCost_Project = Project_Model.TotalCost_Project;

            ProjectMachine_Model.ProjectModel = Project_Model;
            ProjectMachine_Model.ViewMachineModel = ViewMachines_Model;

            Machine_Model.Name_Machine = "";
            Machine_Model.Description_Machine = "";
            Machine_Model.TotalCost_Machine = 0;
        }
        #endregion



        #region Machines
        [HttpPost]
        public ActionResult Create_IncludeMachinesAux(ProjectMachineModel model)
        {
            SaveInfoProjectMachine(model);//guardar a info dos campos
            EditonCreate_Flag = false;
            ViewMachineMaterial_Model.Clear();

            return RedirectToAction("IncludeMachines");
        }

        [HttpPost]
        public ActionResult Edit_IncludeMachinesAux(ProjectMachineModel model)
        {
            SaveInfoProjectMachine(model);
            CreateonEdit_Flag = true;

            return RedirectToAction("IncludeMachines");
        }

        public ActionResult IncludeMachines()
        {
            ProjectMachineMaterial_Model.MachineModel = Machine_Model;
            ProjectMachineMaterial_Model.ViewMachineMaterialModel = ViewMachineMaterial_Model;

            return View(ProjectMachineMaterial_Model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult IncludeMachines(ProjectMachineMaterialModel model)
        {
            if (ModelState.IsValid)
                if(IncludeMachinesAux(model.MachineModel) == "Machine included")
                {
                    foreach(var row in ViewMachineMaterial_Model)
                        Total_ViewMachineMaterial_Model.Add(new ViewMachineMaterialModel
                        {
                            Name_Machine = model.MachineModel.Name_Machine,
                            Name_Material = row.Name_Material,
                            Amount_Material = row.Amount_Material,
                            Cost_Material = row.Unit_Cost * row.Amount_Material,
                            Unit_Cost = row.Unit_Cost,
                            Observations = row.Observations,
                            MissingMaterial = row.MissingMaterial,
                            Id_Provider = row.Id_Provider,
                            Request = row.Request,
                            Priority = row.Priority
                        });

                    ViewMachineMaterial_Model.Clear();
                    ViewBag.Message = "Machine included";

                    Project_Model.TotalCost_Project += Machine_Model.TotalCost_Machine;

                    if (Create_Flag)
                    {
                        EditonCreate_Flag = true;
                        return RedirectToAction("CreateProject");
                    }
                    else
                        return RedirectToAction("EditProject");
                }
                else
                    ViewBag.Message = "Machine not included";

            //Machine_Model.Name_Machine = "";
            //Machine_Model.Description_Machine = "";
            //Machine_Model.TotalCost_Machine = 0;

            ProjectMachineMaterial_Model.MachineModel = Machine_Model;
            ProjectMachineMaterial_Model.ViewMachineMaterialModel = ViewMachineMaterial_Model;
            return View(ProjectMachineMaterial_Model);
        }

        public string IncludeMachinesAux(MachineModel model)
        {
            if (MachineProcessor.LoadMachineByName(model.Name_Machine) != null)
                return "Machine not included";
            else
            {
                foreach(var row in ViewMachines_Model)//excepto se eu mantiver o nome da maquina que escolhi editar
                    if(row.Name_Machine == model.Name_Machine)
                        return "Machine not included";

                model.Description_Machine = model.Description_Machine ?? "Nada a registar";
                model.TotalCost_Machine = Machine_Model.TotalCost_Machine;

                ViewMachines_Model.Add(new ViewMachineModel
                {
                    Name_Machine = model.Name_Machine,
                    Description_Machine = model.Description_Machine,
                    TotalCost_Machine = model.TotalCost_Machine,
                    Name_Project = ""
                });

                Machines_Model.Add(new DataLibrary.Models.MachineModel
                {
                    Name_Machine = model.Name_Machine,
                    Description_Machine = model.Description_Machine,
                    TotalCost_Machine = model.TotalCost_Machine,
                    Id_Project = 0
                });

                return "Machine included";
            }
        }


        public ActionResult EditProjectMachine(string Name_Machine)
        {
            if (Name_Machine != null)
            {
                Name_ProjectMachine_to_edit = Name_Machine;

                foreach (var row in Machines_Model)
                    if (row.Name_Machine == Name_Machine)
                    {
                        Machine_Model.Name_Machine = row.Name_Machine;
                        Machine_Model.Description_Machine = row.Description_Machine;
                        Machine_Model.TotalCost_Machine = row.TotalCost_Machine;

                        CostMachineAux = row.TotalCost_Machine;

                        ProjectMachineMaterial_Model.MachineModel = Machine_Model;
                        ProjectMachineMaterial_Model.ViewMachineMaterialModel = ViewMachineMaterial_Model;

                        ViewMachineMaterial_Model.Clear();

                        foreach (var rowaux in Total_ViewMachineMaterial_Model)
                            if (rowaux.Name_Machine == Name_Machine)
                            {
                                if (rowaux.Amount_Material == 0)
                                    rowaux.Amount_Material = 1;

                                ViewMachineMaterial_Model.Add(new ViewMachineMaterialModel
                                {
                                    Name_Machine = rowaux.Name_Machine,
                                    Name_Material = rowaux.Name_Material,
                                    Amount_Material = rowaux.Amount_Material,
                                    Unit_Cost = rowaux.Unit_Cost,
                                    Cost_Material = rowaux.Unit_Cost * rowaux.Amount_Material,
                                    Observations = rowaux.Observations,
                                    MissingMaterial = rowaux.MissingMaterial,
                                    Id_Provider = rowaux.Id_Provider,
                                    Request = rowaux.Request,
                                    Priority = rowaux.Priority
                                });
                            }
                        ViewMachineMaterial_Model = ViewMachineMaterial_Model.OrderBy(x => x.Name_Machine).ToList();
                        return View(ProjectMachineMaterial_Model);
                    }
            }
            ProjectMachineMaterial_Model.MachineModel = Machine_Model;
            ProjectMachineMaterial_Model.ViewMachineMaterialModel = ViewMachineMaterial_Model;
            return View(ProjectMachineMaterial_Model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProjectMachine(ProjectMachineMaterialModel model)
        {
            //ProjectMachineMaterial_Model.MachineModel = Machine_Model;
            if (ModelState.IsValid)
            {
                foreach (var row in ViewMachines_Model)//ver se ja existe na lista de maquinas que estou a editar/criar
                    if ((row.Name_Machine == model.MachineModel.Name_Machine) & (row.Name_Machine != Name_ProjectMachine_to_edit))
                    {
                        ViewBag.Message = "Machine not edited";
                        ProjectMachineMaterial_Model.MachineModel = Machine_Model;
                        ProjectMachineMaterial_Model.ViewMachineMaterialModel = ViewMachineMaterial_Model;
                        return View(ProjectMachineMaterial_Model);
                    }

                List<DataLibrary.Models.MachineModel> AllMachines = MachineProcessor.LoadMachines();
                foreach (var row in AllMachines)//ver se ja existe na BD
                    if ((row.Name_Machine == model.MachineModel.Name_Machine) & (row.Id_Project != Id_Project_to_edit))
                    {
                        ViewBag.Message = "Machine not edited";
                        ProjectMachineMaterial_Model.MachineModel = Machine_Model;
                        ProjectMachineMaterial_Model.ViewMachineMaterialModel = ViewMachineMaterial_Model;
                        return View(ProjectMachineMaterial_Model);
                    }

                foreach (var row in ViewMachines_Model)//editar as caracteristicas da maquina
                    if (row.Name_Machine == Name_ProjectMachine_to_edit)
                    {
                        row.Name_Machine = model.MachineModel.Name_Machine;
                        row.Description_Machine = model.MachineModel.Description_Machine;
                        row.TotalCost_Machine = model.MachineModel.TotalCost_Machine;
                    }

                foreach (var row in Machines_Model)//editar as caracteristicas da maquina
                    if (row.Name_Machine == Name_ProjectMachine_to_edit)
                    {
                        row.Name_Machine = model.MachineModel.Name_Machine;
                        row.Description_Machine = model.MachineModel.Description_Machine;
                        row.TotalCost_Machine = model.MachineModel.TotalCost_Machine;
                    }


                Total_ViewMachineMaterial_Model.RemoveAll(a => a.Name_Machine == Name_ProjectMachine_to_edit);//se eu mudar o nome da maquina??
                foreach (var row in ViewMachineMaterial_Model)
                {
                    if (row.Name_Machine == Name_ProjectMachine_to_edit)
                        row.Name_Machine = model.MachineModel.Name_Machine;
                    Total_ViewMachineMaterial_Model.Add(new ViewMachineMaterialModel
                    {
                        Name_Machine = row.Name_Machine,
                        Name_Material = row.Name_Material,
                        Amount_Material = row.Amount_Material,
                        Cost_Material = row.Unit_Cost * row.Amount_Material,
                        Unit_Cost = row.Unit_Cost,
                        Observations = row.Observations,
                        MissingMaterial = row.MissingMaterial,
                        Id_Provider = row.Id_Provider,
                        Request = row.Request,
                        Priority = row.Priority
                    });
                }//passar o que esta em viewmachines para total viewmachines ou seja dar update

                Project_Model.TotalCost_Project -= CostMachineAux;
                Project_Model.TotalCost_Project += model.MachineModel.TotalCost_Machine;


                ProjectMachine_Model.ProjectModel = Project_Model;
                ProjectMachine_Model.ViewMachineModel = ViewMachines_Model;
                ViewBag.Message = "Machine edited";
                if (!Create_Flag)
                    return RedirectToAction("EditProject");
                else
                    return RedirectToAction("CreateProject");
            }
            return View();
        }


        public ActionResult DeleteProjectMachine(string Name_Machine)
        {
            if (Create_Flag)
                DeleteProjectMachineAux(Name_Machine);
            else
            {
                ViewMachineModel Machine = new ViewMachineModel();

                foreach (var row in ViewMachines_Model)
                    if (row.Name_Machine == Name_Machine)
                    {
                        Machine.Name_Machine = Name_Machine;
                        Machine.Description_Machine = row.Description_Machine;
                        Machine.TotalCost_Machine = row.TotalCost_Machine;
                        Machine.Name_Project = row.Name_Project;
                        return View(Machine);
                    }
            }
            return Redirect("~/Project/CreateProject");
        }

        [HttpPost]
        public ActionResult DeleteProjectMachine(ViewMachineModel model)
        {
            DeleteProjectMachineAux(model.Name_Machine);//eliminar mesmo da base de dados é so quando eu fizer save
            return Redirect("~/Project/EditProject");
        }

        public void DeleteProjectMachineAux(string Name_Machine)
        {
            foreach(var row in ViewMachines_Model)
            {
                if (row.Name_Machine == Name_Machine)
                    Project_Model.TotalCost_Project -= row.TotalCost_Machine;
            }
            ViewMachines_Model.RemoveAll(a => a.Name_Machine == Name_Machine);
            Machines_Model.RemoveAll(a => a.Name_Machine == Name_Machine);
            ViewMachineMaterial_Model.RemoveAll(a => a.Name_Machine == Name_Machine);
            Total_ViewMachineMaterial_Model.RemoveAll(a => a.Name_Machine == Name_Machine);
        }


        public int Searchmachine(int idmachine)
        {
            foreach (var row in Machines_Model)
                if (row.Id_Machine == idmachine)
                    return 1;

            return 0;
        }
        #endregion



        #region Machine Materials

        public ActionResult EditMachineMaterial(string Name_Material)
        {
            Name_MachineMaterials_to_edit = Name_Material;

            MachineMaterialModel MachineMaterial = new MachineMaterialModel();

            foreach (var row in ViewMachineMaterial_Model)//tenho de carregar para esta lista a lista de materiais sempre que vou editar um projeto
                if (row.Name_Material == Name_Material)
                {
                    MachineMaterial.Id_Material = MaterialProcessor.LoadMaterialByName(row.Name_Material).Id_Material;
                    MachineMaterial.Amount_Material = row.Amount_Material;
                    MachineMaterial.UnitCost_Material = row.Unit_Cost;
                    MachineMaterial.Cost_Material = row.Cost_Material;
                    CostMaterialAux = row.Cost_Material;
                    MachineMaterial.Observations = row.Observations;
                    MachineMaterial.MissingMaterial = row.MissingMaterial;
                    MachineMaterial.Id_Provider = row.Id_Provider;
                    MachineMaterial.Request = row.Request;
                    MachineMaterial.Priority = row.Priority;

                    return View(MachineMaterial);
                }

            return View(MachineMaterial);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditMachineMaterial(MachineMaterialModel model)
        {
            if (ModelState.IsValid)
            {
                model.Observations = model.Observations ?? "Nada a registar";
                //string modelNameMaterial = MaterialProcessor.LoadMaterialById(model.Id_Material).Name_Material;
                string modelNameMaterial = Name_MachineMaterials_to_edit;

                foreach (var row in ViewMachineMaterial_Model)//ver se ja existe na lista de maquinas que estou a editar/criar, se houver nao deixa editar
                    if ((row.Name_Material == modelNameMaterial) & (modelNameMaterial != Name_MachineMaterials_to_edit))
                    {
                        ViewBag.Message = "MachineMaterial not edited";
                        return View();
                    }

                foreach (var row in ViewMachineMaterial_Model)
                    if (row.Name_Material == Name_MachineMaterials_to_edit)
                    {
                        row.Name_Material = modelNameMaterial;
                        row.Amount_Material = model.Amount_Material;
                        row.Unit_Cost = model.UnitCost_Material;
                        row.Cost_Material = model.UnitCost_Material * model.Amount_Material;
                        row.Observations = model.Observations;
                        row.MissingMaterial = model.MissingMaterial;///////////
                        row.Id_Provider = model.Id_Provider;
                        row.Request = model.Request;
                        row.Priority = model.Priority;

                        Machine_Model.TotalCost_Machine -= CostMaterialAux;
                        Machine_Model.TotalCost_Machine += model.UnitCost_Material * model.Amount_Material;
                    }

                ProjectMachineMaterial_Model.MachineModel = Machine_Model;
                ProjectMachineMaterial_Model.ViewMachineMaterialModel = ViewMachineMaterial_Model;
                ViewBag.Message = "MachineMaterial edited";

                if (!Create_Flag)
                    if (!CreateonEdit_Flag)
                        return RedirectToAction("EditProjectMachine");
                    else
                        return RedirectToAction("IncludeMachines");
                else
                    if (EditonCreate_Flag)
                        return RedirectToAction("EditProjectMachine");
                    else
                        return RedirectToAction("IncludeMachines");
            }
            MachineMaterialModel MachineMaterial = new MachineMaterialModel();

            foreach (var row in ViewMachineMaterial_Model)//tenho de carregar para esta lista a lista de materiais sempre que vou editar um projeto
                if (row.Name_Material == Name_MachineMaterials_to_edit)
                {
                    MachineMaterial.Id_Material = MaterialProcessor.LoadMaterialByName(row.Name_Material).Id_Material;
                    MachineMaterial.Amount_Material = row.Amount_Material;
                    MachineMaterial.Cost_Material = row.Cost_Material;
                    CostMaterialAux = row.Cost_Material;
                    MachineMaterial.Observations = row.Observations;
                    MachineMaterial.MissingMaterial = row.MissingMaterial;
                    MachineMaterial.Id_Provider = row.Id_Provider;
                    MachineMaterial.Request = row.Request;
                    MachineMaterial.Priority = row.Priority;

                    break;
                }
             return View(MachineMaterial);
        }


        public ActionResult DeleteMachineMaterial(string Name_Material)
        {
            foreach(var row in ViewMachineMaterial_Model)
            {
                if (row.Name_Material == Name_Material)
                    Machine_Model.TotalCost_Machine -= row.Cost_Material;
            }
            
            ViewMachineMaterial_Model.RemoveAll(a => a.Name_Material == Name_Material);
            if (Create_Flag)
                if (!EditonCreate_Flag)
                    return Redirect("~/Project/IncludeMachines");
            return Redirect("~/Project/EditProjectMachine");
        }//comfirmaçao de eliminaçao?


        public void SaveInfoMachine(ProjectMachineMaterialModel model)
        {
            Machine_Model.Name_Machine = model.MachineModel.Name_Machine;
            Machine_Model.Description_Machine = model.MachineModel.Description_Machine;
            //Machine_Model.TotalCost_Machine = model.MachineModel.TotalCost_Machine;

            if (!EditonCreate_Flag & Create_Flag)
                foreach (var row in Total_ViewMachineMaterial_Model)
                    if (row.Name_Machine == model.MachineModel.Name_Machine)
                        ViewMachineMaterial_Model.Add(new ViewMachineMaterialModel
                        {
                            Name_Machine = row.Name_Machine,
                            Name_Material = row.Name_Material,
                            Amount_Material = row.Amount_Material,
                            Cost_Material = row.Cost_Material,
                            Observations = row.Observations,
                            MissingMaterial = row.MissingMaterial,
                            Id_Provider = row.Id_Provider,
                            Request = row.Request,
                            Priority = row.Priority
                        });
            
            ProjectMachineMaterial_Model.MachineModel = Machine_Model;
            ProjectMachineMaterial_Model.ViewMachineMaterialModel = ViewMachineMaterial_Model;
        }


        public ActionResult DiscardCreateMachine()
        {
            CleanMachineMaterialView();
            if(Create_Flag)
                return RedirectToAction("CreateProject");
            return RedirectToAction("EditProject");
        }

        public ActionResult DiscardEditMachine()
        {
            CleanMachineMaterialView();

            if (!Create_Flag)
                return RedirectToAction("EditProject");
            else
                return RedirectToAction("CreateProject");
        }

        public void CleanMachineMaterialView()
        {
            Machine_Model.Name_Machine = "";
            Machine_Model.Id_Project = 0;
            Machine_Model.Description_Machine = "";
            Machine_Model.TotalCost_Machine = 0;
            ViewMachineMaterial_Model.Clear();

            ProjectMachineMaterial_Model.MachineModel = Machine_Model;
            ProjectMachineMaterial_Model.ViewMachineMaterialModel = ViewMachineMaterial_Model;
        }


        public ActionResult AskRequest(int Id_Provider)
        {
            DataLibrary.Models.ProviderModel Provider = ProviderProcessor.LoadProviderById(Id_Provider);

            Outlook.Application oApp = new Outlook.Application();
            Outlook._MailItem oMailItem = (Outlook._MailItem)oApp.CreateItem(Outlook.OlItemType.olMailItem);

            oMailItem.To = Provider.Mail_Provider;
            oMailItem.CC = "pedro.diogo@preh.pt";
            if ((int)Provider.Language_Provider == 1)
            {
                oMailItem.Subject = "Pedido de Cotação de material";

                oMailItem.Body = "Serve o seguinte email para pedir cotação dos seguintes materiais abaixo alistados.";

                oMailItem.Body += "\nProjeto: " + ProjectProcessor.LoadProjectById(Id_Project_to_edit).Name_Project;
                oMailItem.Body += "\nMáquina: " + Name_ProjectMachine_to_edit;
                oMailItem.Body += "\nMateriais:\n";

                foreach (var row in ViewMachineMaterial_Model)
                {
                    if (row.Id_Provider == Id_Provider)
                    {
                        oMailItem.Body += "Nome: " + row.Name_Material + "        Quantidade: " + row.MissingMaterial;
                    }
                }

                oMailItem.Body += "\n\nCumprimentos,";
            }
            else
            {
                oMailItem.Subject = "Material Quote Request";

                oMailItem.Body = "The following email is to request quote from the following materials listed below.";

                oMailItem.Body += "\nProject: " + ProjectProcessor.LoadProjectById(Id_Project_to_edit).Name_Project;
                oMailItem.Body += "\nMachine: " + Name_ProjectMachine_to_edit;
                oMailItem.Body += "\nMaterials:\n";

                foreach (var row in ViewMachineMaterial_Model)
                {
                    if (row.Id_Provider == Id_Provider)
                    {
                        oMailItem.Body += "Name: " + row.Name_Material + "        Amount: " + row.MissingMaterial;
                    }
                }

                oMailItem.Body += "\n\nBest regards,";
            }

           /* foreach (var row in Quotation)
                oMailItem.Attachments.Add(row.QuotationPath);*/

            oMailItem.Display(true);
            return RedirectToAction("EditProjectMachine");
        }
        #endregion



        #region Changes
        static List<MachineMaterialModelPartial> IncludeMachineMaterials = new List<MachineMaterialModelPartial>();

        [HttpPost]
        public ActionResult IncludeMachinesMaterialsAux(ProjectMachineMaterialModel model)
        {
            SaveInfoMachine(model);
            IncludeMachineMaterials.Clear();
            return RedirectToAction("IncludeMachineMaterial");
        }

        public ActionResult IncludeMachineMaterial()
        {
            ListMachineMaterialModelPartial model = new ListMachineMaterialModelPartial();
            model.MachineMaterialModelPartialDetails = IncludeMachineMaterials;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult IncludeMachineMaterial(ListMachineMaterialModelPartial model)
        {//nao tenho acesso ao nome nem specs
                string new_namematerial;

                for (int i = 0; i < IncludeMachineMaterials.Count(); i++)
                {
                    if(model.MachineMaterialModelPartialDetails[i].Check == true)
                    {
                        new_namematerial = IncludeMachineMaterials[i].Name_Material;
                        model.MachineMaterialModelPartialDetails[i].Observations = model.MachineMaterialModelPartialDetails[i].Observations ?? "Nada a registar";
                        
                        if(model.MachineMaterialModelPartialDetails[i].Id_Provider == 0)
                        {
                            ViewBag.Message = "MachineMaterial incorrect";
                            ListMachineMaterialModelPartial model1 = new ListMachineMaterialModelPartial();
                            IncludeMachineMaterials.Clear();
                            model1.MachineMaterialModelPartialDetails = IncludeMachineMaterials;
                            return View(model1);
                        }

                        foreach (var row in ViewMachineMaterial_Model)
                            if (row.Name_Material == new_namematerial)
                            {
                                ViewBag.Message = "MachineMaterial not included";
                                ListMachineMaterialModelPartial model1 = new ListMachineMaterialModelPartial();
                                IncludeMachineMaterials.Clear();
                                model1.MachineMaterialModelPartialDetails = IncludeMachineMaterials;
                                return View(model1);
                            }

                        ViewMachineMaterial_Model.Add(new ViewMachineMaterialModel
                        {
                            Name_Machine = Name_ProjectMachine_to_edit,
                            Name_Material = new_namematerial,
                            Amount_Material = model.MachineMaterialModelPartialDetails[i].Amount_Material,
                            Unit_Cost = model.MachineMaterialModelPartialDetails[i].CostUnit_Material,
                            Cost_Material = model.MachineMaterialModelPartialDetails[i].CostUnit_Material * model.MachineMaterialModelPartialDetails[i].Amount_Material,
                            Observations = model.MachineMaterialModelPartialDetails[i].Observations,
                            MissingMaterial = model.MachineMaterialModelPartialDetails[i].Amount_Material,
                            Id_Provider = model.MachineMaterialModelPartialDetails[i].Id_Provider,
                            Request = model.MachineMaterialModelPartialDetails[i].Request,
                            Priority = model.MachineMaterialModelPartialDetails[i].Priority
                        });

                        Machine_Model.TotalCost_Machine += (model.MachineMaterialModelPartialDetails[i].CostUnit_Material) * (model.MachineMaterialModelPartialDetails[i].Amount_Material);
                    }
                }

                if (Create_Flag)
                    if (EditonCreate_Flag)
                        return RedirectToAction("EditProjectMachine");
                    else
                        return RedirectToAction("IncludeMachines");
                else
                    if (!CreateonEdit_Flag)
                    return RedirectToAction("EditProjectMachine");
                else
                    return RedirectToAction("IncludeMachines");
        }

        public ActionResult _TableMachineMaterial(ListMachineMaterialModelPartial model)
        {
            ListMachineMaterialModelPartial modelaux = new ListMachineMaterialModelPartial();
            modelaux.MachineMaterialModelPartialDetails = IncludeMachineMaterials;
            return PartialView(modelaux);
            
           // return RedirectToAction("IncludeMachineMaterial");
        }

        [HttpPost]
        public ActionResult _TableMachineMaterialaux(int id)
        {
            IncludeMachineMaterials.Clear();
            List<DataLibrary.Models.MaterialModel> DLMaterial = MaterialProcessor.LoadMaterialsBySection(id);
            foreach(var row in DLMaterial)
            {
                IncludeMachineMaterials.Add(new MachineMaterialModelPartial
                {
                    Name_Material = row.Name_Material,
                    Specs_Material = row.Specs_Material,
                    Check = false,
                    Amount_Material = 0,
                    Cost_Material = 0,
                    Observations = "Nada a registar",
                    Id_Provider = 0,
                    Request = 0,
                    Priority = false
                });
                IncludeMachineMaterials = IncludeMachineMaterials.GroupBy(x => new { x.Name_Material })
                                                                 .Select(x => x.First())
                                                                 .OrderBy(x => x.Name_Material)
                                                                 .ToList();
            }
            
            return RedirectToAction("_TableMachineMaterial");
        }

        public ActionResult BacktoMachine()
        {
            if(Create_Flag)
                return RedirectToAction("IncludeMachines");
            return RedirectToAction("EditProjectMachine");
        }


        [HttpGet]
        public JsonResult ReturnUnitCost(string Name_Material, int Id_Provider)
        {
            DataLibrary.Models.ProvideMaterialsModel ProvideMaterial = ProvideMaterialsProcessor.LoadProvideMaterial(Id_Provider, MaterialProcessor.LoadMaterialByName(Name_Material).Id_Material);
            if (Request.IsAjaxRequest())
            {
                return new JsonResult
                {
                    Data = ProvideMaterial.Unit_Cost,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            else
            {
                return new JsonResult
                {
                    Data = null,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }

        [HttpGet]
        public JsonResult ReturnUnitCostByid(int Id_Material, int Id_Provider)
        {
            DataLibrary.Models.ProvideMaterialsModel ProvideMaterial = ProvideMaterialsProcessor.LoadProvideMaterial(Id_Provider, Id_Material);

            if (Request.IsAjaxRequest())
            {
                return new JsonResult
                {
                    Data = ProvideMaterial.Unit_Cost,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            else
            {
                return new JsonResult
                {
                    Data = null,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }


        public FilePathResult SeeQuotationFile(int Id_Provider, int Id_Material)
        {
            return File(ProvideMaterialsProcessor.LoadProvideMaterial(Id_Provider, Id_Material).QuotationPath, "application/pdf");
        }

        #endregion
    }
}