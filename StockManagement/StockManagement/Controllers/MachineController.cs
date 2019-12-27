using DataLibrary.BusinessLogic;
using StockManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StockManagement.Controllers
{
    public class MachineController : Controller
    {
        static int Id_Machine_to_edit = new int();
        static string Name_Machine_info = "";

        static ProjectStockMachineMaterialModel model = new ProjectStockMachineMaterialModel();
        static MachineModel Machine_Model = new MachineModel();
        static List<ViewStockMachineMaterialModel> ViewStockMachineMaterial_Model = new List<ViewStockMachineMaterialModel>();
        static List<ViewStockMachineMaterialModel> AllViewStockMachineMaterial_Model = new List<ViewStockMachineMaterialModel>();
        static bool Alert_flag = false;
        static bool Free_Assign = false;

        static List<ViewMachineModel> Machines = new List<ViewMachineModel>();

        public ActionResult ViewMachines()
        {
            ViewMachinesaux();
            return View(Machines.OrderBy(x => x.Name_Machine));
        }

        public void ViewMachinesaux()
        {
            Machine_Model.Name_Machine = "";
            Machine_Model.Description_Machine = "";
            Machine_Model.TotalCost_Machine = 0;
            ViewStockMachineMaterial_Model.Clear();
            AllViewStockMachineMaterial_Model.Clear();
            Name_Machine_info = "";
            Alert_flag = false;
            Free_Assign = false;

            Machines.Clear();
            var data = MachineProcessor.LoadMachines();

            foreach (var row in data)
            {
                Machines.Add(new ViewMachineModel
                {
                    Name_Machine = row.Name_Machine,
                    Description_Machine = row.Description_Machine,
                    TotalCost_Machine = row.TotalCost_Machine,
                    Name_Project = ProjectProcessor.LoadProjectById(row.Id_Project).Name_Project
                });
            }
        }

        public ActionResult _TableMachine(ViewStockMaterialModel model)
        {
            return PartialView(Machines.OrderBy(x => x.Name_Machine));
        }

        [HttpPost]
        public ActionResult _TableMachineaux(int id)
        {
            ViewMachinesaux();
            Machines = Machines.Where(x => x.Name_Project == ProjectProcessor.LoadProjectById(id).Name_Project).ToList();
            return RedirectToAction("_TableMachine");
        }

        public ActionResult CreateMachine()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateMachine(MachineModel model)
        {
            if (ModelState.IsValid)
            {
                if (MachineProcessor.CreateMachine(model.Name_Machine, model.Description_Machine, model.TotalCost_Machine, model.Id_Project) == 0)
                {
                    ViewBag.Message = "Machine not created";
                }
                else
                {
                    ViewBag.Message = "Machine created";
                    return RedirectToAction("ViewMachines");
                }
            }
            return View();
        }


        public ActionResult EditMachine(string Name_Machine)
        {
            DataLibrary.Models.MachineModel DLMachine = MachineProcessor.LoadMachineByName(Name_Machine);

            MachineModel Machine = new MachineModel();

            Id_Machine_to_edit = DLMachine.Id_Machine;
            Machine.Name_Machine = DLMachine.Name_Machine;
            Machine.Description_Machine = DLMachine.Description_Machine;
            Machine.TotalCost_Machine = DLMachine.TotalCost_Machine;
            Machine.Id_Project = DLMachine.Id_Project;

            return View(Machine);
        }

        [HttpPost]
        public ActionResult EditMachine(MachineModel model)
        {
            if (ModelState.IsValid)
            {
                if (MachineProcessor.EditMachine(Id_Machine_to_edit, model.Name_Machine, model.Description_Machine, model.TotalCost_Machine, model.Id_Project) == 0)
                {
                    ViewBag.Message = "Machine not edited";
                }
                else
                {
                    ViewBag.Message = "Machine edited";
                    return RedirectToAction("ViewMachines");
                }
            }
            return View();
        }



        public ActionResult DeleteMachine(string Name_Machine, string Description_Machine, decimal TotalCost_Machine, string Name_Project)
        {
            ViewMachineModel Machine = new ViewMachineModel();

            Machine.Name_Machine = Name_Machine;
            Machine.Description_Machine = Description_Machine;
            Machine.TotalCost_Machine = TotalCost_Machine;
            Machine.Name_Project = Name_Project;

            return View(Machine);
        }

        [HttpPost]
        public ActionResult DeleteMachine(ViewMachineModel model)
        {
            MachineMaterialProcessor.DeleteMachineMaterials(MachineProcessor.LoadMachineByName(model.Name_Machine).Id_Machine);//remover os materiais desta maquina
            MachineProcessor.DeleteMachine(model.Name_Machine);//remover a maquina do projeto ao qual esta associada->ja faz isto aqui

            return Redirect("~/Machine/ViewMachines");
        }


        public ActionResult InfoMachine(string Name_Machine)
        {
            if (Name_Machine != null)
            {
                Name_Machine_info = Name_Machine;
                DataLibrary.Models.MachineModel DLMachine = MachineProcessor.LoadMachineByName(Name_Machine);
                List<DataLibrary.Models.MachineMaterialModel> data = MachineMaterialProcessor.LoadMachineMaterialsById(DLMachine.Id_Machine);
                List<DataLibrary.Models.MachineMaterialModel> data1 = MachineMaterialProcessor.LoadMachineMaterials();


                Machine_Model.Name_Machine = Name_Machine;
                Machine_Model.Description_Machine = DLMachine.Description_Machine;
                Machine_Model.TotalCost_Machine = DLMachine.TotalCost_Machine;
                Machine_Model.Id_Project = DLMachine.Id_Project;

                foreach (var row in data1)
                    AllViewStockMachineMaterial_Model.Add(new ViewStockMachineMaterialModel
                    {
                        Name_Machine = MachineProcessor.LoadMachineById(row.Id_Machine).Name_Machine,
                        Name_Material = MaterialProcessor.LoadMaterialById(row.Id_Material).Name_Material,
                        Amount_Material = row.Amount_Material,
                        Cost_Material = row.Cost_Material,
                        Observations = row.Observations,
                        MissingMaterial = row.MissingMaterial,
                        AmountinStock = StockMaterialProcessor.StockMaterialAmount(row.Id_Material).Amount_Material,
                        Request = row.Request,
                        Priority = row.Priority,
                        Name_Provider = ProviderProcessor.LoadProviderById(row.Id_Provider).Name_Provider,
                        StocktoMachine = 0
                    });

                foreach (var row in AllViewStockMachineMaterial_Model)
                {
                    if ((row.MissingMaterial > 0) & (row.AmountinStock > 0))
                    {
                        if (row.MissingMaterial <= row.AmountinStock)
                            row.StocktoMachine = row.MissingMaterial;
                        else
                            row.StocktoMachine = row.AmountinStock;
                    }
                }

                ViewStockMachineMaterial_Model = AllViewStockMachineMaterial_Model.Where(x => x.Name_Machine == DLMachine.Name_Machine).ToList();


                model.MachineModel = Machine_Model;
                model.ViewStockMachineMaterialModel = ViewStockMachineMaterial_Model;

                return View(model);
            }
            Name_Machine = Name_Machine_info;
            model.MachineModel = Machine_Model;
            model.ViewStockMachineMaterialModel = ViewStockMachineMaterial_Model;

            Free_Assign = false;
            if (Alert_flag)
            {
                Alert_flag = false;
                Free_Assign = true;
                ViewBag.Message = "Incorrect Assign";
            }
            return View(model);

        }


        public ActionResult InfoMachineaux(string Name_Machine, string Name_Material)
        {
            foreach (var row in ViewStockMachineMaterial_Model)
            {
                if((row.Name_Machine == Name_Machine) & (row.Name_Material == Name_Material))
                    if ((row.MissingMaterial > 0) & (row.AmountinStock > 0))
                    {
                        if (row.Priority == false)
                        {
                            foreach (var row1 in AllViewStockMachineMaterial_Model)
                            {
                                if ((row1.Name_Material == Name_Material) & (row1.Priority == true) & (Free_Assign == false) & (row1.MissingMaterial != 0))
                                {
                                    Alert_flag = true;
                                    return RedirectToAction("InfoMachine");
                                    //mandar aviso de que existe prioridade desse material para outra maquina
                                }
                            }
                        }

                        if (row.MissingMaterial <= row.AmountinStock)
                        {
                            row.StocktoMachine -= row.MissingMaterial;
                            row.AmountinStock -= row.MissingMaterial;
                            row.MissingMaterial = 0;
                        }
                        else
                        {
                            row.MissingMaterial -= row.StocktoMachine;
                            row.AmountinStock -= row.StocktoMachine;
                            row.StocktoMachine = 0;
                        }

                        DataLibrary.Models.StockMaterialModel Stock_Material = StockMaterialProcessor.StockMaterialAmount(MaterialProcessor.LoadMaterialByName(Name_Material).Id_Material);
                        //em principio um material só esta associado a 1 fornecedor, senao tenho de ajustar isto
                        
                        StockMaterialProcessor.EditStockMaterial(Stock_Material.Id_StockMaterial, Stock_Material.Id_Provider, Stock_Material.Id_Material, MaterialProcessor.LoadMaterialByName(Name_Material).Id_Section, row.AmountinStock, Stock_Material.Observations);

                        MachineMaterialProcessor.EditMachineMaterial(MachineProcessor.LoadMachineByName(row.Name_Machine).Id_Machine, MaterialProcessor.LoadMaterialByName(row.Name_Material).Id_Material, row.MissingMaterial);            
                    }
            }
            return RedirectToAction("InfoMachine");
        }
    }
}