using DataLibrary.BusinessLogic;
using StockManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StockManagement.Controllers
{
    public class SupplyController : Controller
    {
        static List<ViewSupplyModel> ViewSupplyModel = new List<ViewSupplyModel>();
        static List<ViewSupplyModel> TotalViewSupplyModel = new List<ViewSupplyModel>();
        static SupplyModel SupplyModel = new SupplyModel();
        static bool Details = false;
        static int Request_Details;
        static List<ViewSupplyModel> Supplys = new List<ViewSupplyModel>();

        public ActionResult ViewSupplys()
        {
            var data = SupplyProcessor.LoadSupplys();
            Details = false;
            CleanModel();

            foreach (var row in data)
            {
                if(Supplys.Where(x => x.Request == row.Request).ToList().Count() == 0)
                    Supplys.Add(new ViewSupplyModel
                    {
                        Name_Provider = ProviderProcessor.LoadProviderById(row.Id_Provider).Name_Provider,
                        Name_Material = MaterialProcessor.LoadMaterialById(row.Id_Material).Name_Material,
                        Name_Section = SectionProcessor.LoadSectionById(row.Id_Section).Name_Section,
                        Amount_Material = row.Amount_Material,
                        TotalPrice_Supply = row.TotalPrice_Supply,
                        Date_Supply = row.Date_Supply,
                        Request = row.Request,
                        SupplyOrder = row.SupplyOrder
                    });
            }
            return View(Supplys.OrderByDescending(x => x.Request));
        }


        public ActionResult CreateSupply()
        {
            SupplyMaterialModel model = new SupplyMaterialModel();
            model.ViewSupplyModel = TotalViewSupplyModel;
            model.SupplyModel = SupplyModel;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateSupply(SupplyMaterialModel model)
        {
            if (ModelState.IsValid)
            {
                if ((SupplyProcessor.LoadSupplys().Where(x => x.Request == model.SupplyModel.Request).Count()) == 0 & TotalViewSupplyModel.Count() != 0)
                {
                    foreach (var row in TotalViewSupplyModel)
                    {
                        SupplyProcessor.CreateSupply(model.SupplyModel.Id_Provider, MaterialProcessor.LoadMaterialByName(row.Name_Material).Id_Material, SectionProcessor.LoadSectionByName(row.Name_Section).Id_Section, row.Amount_Material, row.TotalPrice_Material, model.SupplyModel.TotalPrice_Supply, model.SupplyModel.Date_Supply, model.SupplyModel.Request, model.SupplyModel.SupplyOrder);
                        StockMaterialProcessor.CreateStockMaterial(MaterialProcessor.LoadMaterialByName(row.Name_Material).Id_Material, SupplyModel.Id_Provider, SectionProcessor.LoadSectionByName(row.Name_Section).Id_Section, row.Amount_Material, "Nada a registar");
                    }
                    ViewBag.Message = "Supply created";
                    return RedirectToAction("ViewSupplys");
                }
                else
                {
                    ViewBag.Message = "Supply not created";
                    SupplyMaterialModel model1 = new SupplyMaterialModel();
                    model1.ViewSupplyModel = TotalViewSupplyModel;
                    model1.SupplyModel = SupplyModel;
                    return View(model1);
                }
            }
            return View();
        }


        public ActionResult DeleteSupply(int Request, string Name_Provider, decimal TotalPrice_Supply, DateTime Date_Supply)
        {
            ViewSupplyModel Supply = new ViewSupplyModel();

            Supply.Request = Request;
            Supply.Name_Provider = Name_Provider;
            Supply.TotalPrice_Supply = TotalPrice_Supply;
            Supply.Date_Supply = Date_Supply;

            return View(Supply);
        }


        [HttpPost]
        public ActionResult DeleteSupply(ViewSupplyModel model)
        {
            SupplyProcessor.DeleteSupplyByRequest(model.Request);

            return Redirect("~/Supply/ViewSupplys");
        }



        public void CleanModel()
        {
            ViewSupplyModel.Clear();
            TotalViewSupplyModel.Clear();
            Supplys.Clear();
            SupplyModel.Id_Material = 0;
            SupplyModel.Id_Provider = 0;
            SupplyModel.Id_Section = 0;
            SupplyModel.Request = 0;
            SupplyModel.SupplyOrder = 0;
            SupplyModel.TotalPrice_Supply = 0;
            SupplyModel.Amount_Material = 0;
            SupplyModel.Date_Supply = DateTime.Now;
        }


        public ActionResult DetailsSupply(int? Request)
        {
            Details = true;
            if (Request != null)
            {
                Request_Details = (int)Request;
                var data = SupplyProcessor.LoadSupplys();

                foreach (var row in data)
                {
                    TotalViewSupplyModel.Add(new ViewSupplyModel
                    {
                        Name_Provider = ProviderProcessor.LoadProviderById(row.Id_Provider).Name_Provider,
                        Name_Material = MaterialProcessor.LoadMaterialById(row.Id_Material).Name_Material,
                        Name_Section = SectionProcessor.LoadSectionById(row.Id_Section).Name_Section,
                        Amount_Material = row.Amount_Material,
                        TotalPrice_Material = row.TotalPrice_Material,
                        TotalPrice_Supply = row.TotalPrice_Supply,
                        Date_Supply = row.Date_Supply,
                        Request = row.Request,
                        SupplyOrder = row.SupplyOrder
                    });
                }
                TotalViewSupplyModel = TotalViewSupplyModel.Where(x => x.Request == Request).ToList();
                SupplyModel.Request = (int)Request;
                SupplyModel.Id_Provider = ProviderProcessor.LoadProviderByName(TotalViewSupplyModel[0].Name_Provider).Id_Provider;
                SupplyModel.TotalPrice_Supply = TotalViewSupplyModel[0].TotalPrice_Supply;
                SupplyModel.Date_Supply = TotalViewSupplyModel[0].Date_Supply;
                SupplyModel.SupplyOrder = TotalViewSupplyModel[0].SupplyOrder;
            }
            SupplyMaterialModel model = new SupplyMaterialModel();
            model.ViewSupplyModel = TotalViewSupplyModel;
            model.SupplyModel = SupplyModel;

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DetailsSupply(SupplyMaterialModel model)
        {
            if (ModelState.IsValid)
            {
                if (((SupplyProcessor.LoadSupplys().Where(x => x.Request == model.SupplyModel.Request).Count() == 0) || (Request_Details == model.SupplyModel.Request)) & TotalViewSupplyModel.Count() != 0)
                {
                    SupplyProcessor.DeleteSupplyByRequest(Request_Details);

                    foreach (var row in TotalViewSupplyModel)
                    {
                        SupplyProcessor.CreateSupply(model.SupplyModel.Id_Provider, MaterialProcessor.LoadMaterialByName(row.Name_Material).Id_Material, SectionProcessor.LoadSectionByName(row.Name_Section).Id_Section, row.Amount_Material, row.TotalPrice_Material, model.SupplyModel.TotalPrice_Supply, model.SupplyModel.Date_Supply, model.SupplyModel.Request, model.SupplyModel.SupplyOrder);
                        //optei por nao mexer no stock aqui
                    }
                    ViewBag.Message = "Supply edited";
                    return RedirectToAction("ViewSupplys");
                }
                else
                {
                    ViewBag.Message = "Supply not edited";
                    SupplyMaterialModel model1 = new SupplyMaterialModel();
                    model1.ViewSupplyModel = TotalViewSupplyModel;
                    model1.SupplyModel = SupplyModel;
                    return View(model1);
                }
            }
            return View();
        }

        [HttpPost]
        public ActionResult _TableSupplyMaterialaux(int id)
        {
            ViewSupplyModel.Clear();
            List<DataLibrary.Models.MaterialModel> DLMaterial = MaterialProcessor.LoadMaterialsBySection(id);


            foreach (var row in DLMaterial)
            {
                List<DataLibrary.Models.ProvideMaterialsModel> DLProvideMaterial = ProvideMaterialsProcessor.LoadProvideMaterialsByMaterial(row.Id_Material);
                foreach (var row1 in DLProvideMaterial)
                {
                    if (row1.Id_Provider == SupplyModel.Id_Provider)
                        ViewSupplyModel.Add(new ViewSupplyModel
                        {
                            Name_Material = row.Name_Material,
                            Specs_Material = row.Specs_Material,
                            Name_Section = SectionProcessor.LoadSectionById(id).Name_Section,
                            Check = false,
                            Amount_Material = 0,
                            TotalPrice_Supply = 0,
                            TotalPrice_Material = row1.Unit_Cost
                        });
                }
            }

            return RedirectToAction("_TableSupplyMaterial");
        }


        public ActionResult _TableSupplyMaterial(ListSupplyMaterialModelPartial model)
        {
            ListSupplyMaterialModelPartial modelaux = new ListSupplyMaterialModelPartial();
            modelaux.ViewSupply_Model = ViewSupplyModel;
            return PartialView(modelaux);
        }




        public ActionResult IncludeMaterialsaux(SupplyMaterialModel model)
        {
            ViewSupplyModel.Clear();

            SupplyModel.Id_Provider = model.SupplyModel.Id_Provider;
            SupplyModel.Request = model.SupplyModel.Request;
            SupplyModel.TotalPrice_Supply = model.SupplyModel.TotalPrice_Supply;
            SupplyModel.Date_Supply = model.SupplyModel.Date_Supply;
            SupplyModel.SupplyOrder = model.SupplyModel.SupplyOrder;

            return RedirectToAction("IncludeMaterials");
        }

        public ActionResult IncludeMaterials()
        {
            ListSupplyMaterialModelPartial model1 = new ListSupplyMaterialModelPartial();
            model1.ViewSupply_Model = ViewSupplyModel;
            return View(model1);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult IncludeMaterials(ListSupplyMaterialModelPartial model)
        {//nao tenho acesso ao nome nem specs
            string new_namematerial;

            for (int i = 0; i < ViewSupplyModel.Count(); i++)
            {
                if (model.ViewSupply_Model[i].Check == true)
                {
                    new_namematerial = ViewSupplyModel[i].Name_Material;

                    foreach (var row in TotalViewSupplyModel)
                        if (row.Name_Material == new_namematerial)
                        {
                            ViewBag.Message = "Material not included";
                            ListSupplyMaterialModelPartial model1 = new ListSupplyMaterialModelPartial();
                            ViewSupplyModel.Clear();
                            model1.ViewSupply_Model = ViewSupplyModel;
                            return View(model1);
                        }

                    TotalViewSupplyModel.Add(new ViewSupplyModel
                    {
                        Name_Section = SectionProcessor.LoadSectionById(MaterialProcessor.LoadMaterialByName(new_namematerial).Id_Section).Name_Section,
                        Name_Material = new_namematerial,
                        Amount_Material = model.ViewSupply_Model[i].Amount_Material,
                        TotalPrice_Material = model.ViewSupply_Model[i].TotalPrice_Material * model.ViewSupply_Model[i].Amount_Material
                    });
                    SupplyModel.TotalPrice_Supply += model.ViewSupply_Model[i].TotalPrice_Material * model.ViewSupply_Model[i].Amount_Material;
                }
            }

            foreach(var row in TotalViewSupplyModel)
            {
                row.TotalPrice_Supply = SupplyModel.TotalPrice_Supply;
            }

            if (Details)
                return RedirectToAction("DetailsSupply");

            return RedirectToAction("CreateSupply");
        }


        public ActionResult DeleteMaterial(string Name_Material)
        {
            foreach(var row in TotalViewSupplyModel)
            {
                if (row.Name_Material == Name_Material)
                    SupplyModel.TotalPrice_Supply -= row.TotalPrice_Material;
            }
            TotalViewSupplyModel.RemoveAll(x => x.Name_Material == Name_Material);
            if(!Details)
                return RedirectToAction("CreateSupply");
            return RedirectToAction("DetailsSupply");
        }


        public ActionResult Back()
        {
            if(!Details)
                return RedirectToAction("CreateSupply");
            return RedirectToAction("DetailsSupply");
        }






        [HttpPost]
        public ActionResult _TableSupplysaux(int? id, string id1)
        {
            //ir buscar o supply com este request
            if (id == null)
            {
                Supplys.Clear();
                var data = SupplyProcessor.LoadSupplys();

                foreach (var row in data)
                {
                    if (Supplys.Where(x => x.Request == row.Request).ToList().Count() == 0)
                        Supplys.Add(new ViewSupplyModel
                        {
                            Name_Provider = ProviderProcessor.LoadProviderById(row.Id_Provider).Name_Provider,
                            Name_Material = MaterialProcessor.LoadMaterialById(row.Id_Material).Name_Material,
                            Name_Section = SectionProcessor.LoadSectionById(row.Id_Section).Name_Section,
                            Amount_Material = row.Amount_Material,
                            TotalPrice_Supply = row.TotalPrice_Supply,
                            Date_Supply = row.Date_Supply,
                            Request = row.Request,
                            SupplyOrder = row.SupplyOrder
                        });
                }
                Supplys = Supplys.OrderByDescending(x => x.Request).ToList();
            }
            else
            {
                var data = SupplyProcessor.LoadSupplys();
                Supplys.Clear();

                foreach (var row in data)
                {
                        Supplys.Add(new ViewSupplyModel
                        {
                            Name_Provider = ProviderProcessor.LoadProviderById(row.Id_Provider).Name_Provider,
                            Name_Material = MaterialProcessor.LoadMaterialById(row.Id_Material).Name_Material,
                            Name_Section = SectionProcessor.LoadSectionById(row.Id_Section).Name_Section,
                            Amount_Material = row.Amount_Material,
                            TotalPrice_Supply = row.TotalPrice_Supply,
                            Date_Supply = row.Date_Supply,
                            Request = row.Request,
                            SupplyOrder = row.SupplyOrder
                        });
                }
                if (id1 == "Exactly")
                    Supplys = Supplys.Where(x => x.Request == id).ToList();
                else
                    Supplys = Supplys.Where(x => x.Request.ToString().Contains(id.ToString())).ToList();
            }
            return RedirectToAction("_TableSupplys");
        }

        public ActionResult _TableSupplys(List<ViewSupplyModel> model)
        {
            return PartialView(Supplys);
        }


    }
}