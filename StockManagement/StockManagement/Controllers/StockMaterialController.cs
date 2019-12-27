using DataLibrary.BusinessLogic;
using StockManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StockManagement.Controllers
{
    public class StockMaterialController : Controller
    {
        static int Id_StockMaterial_to_edit = new int();
        static List<ViewStockMaterialModel> StockMaterials = new List<ViewStockMaterialModel>();

        public ActionResult ViewStockMaterial()
        {
            ViewStockMaterialaux();
            return View(StockMaterials);
        }

        public void ViewStockMaterialaux()
        {
            var data = StockMaterialProcessor.LoadOrderStockMaterials();
            StockMaterials.Clear();
            foreach (var row in data)
            {
                StockMaterials.Add(new ViewStockMaterialModel
                {
                    Name_Material = MaterialProcessor.LoadMaterialById(row.Id_Material).Name_Material,
                    Name_Provider = ProviderProcessor.LoadProviderById(row.Id_Provider).Name_Provider,
                    Name_Section = SectionProcessor.LoadSectionById(row.Id_Section).Name_Section,
                    Amount_Material = row.Amount_Material,
                    Observations = row.Observations,
                });
            }
        }

        public ActionResult _TableStockMaterial(ViewStockMaterialModel model)
        {
            return PartialView(StockMaterials);
        }

        [HttpPost]
        public ActionResult _TableStockMaterialaux(int id)
        {
            ViewStockMaterialaux();
            StockMaterials = StockMaterials.Where(x => x.Name_Section == SectionProcessor.LoadSectionById(id).Name_Section).ToList();
            return RedirectToAction("_TableStockMaterial");
        }


        public ActionResult CreateStockMaterial()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateStockMaterial(StockMaterialModel model)
        {
            if (ModelState.IsValid)
            {
               // DataLibrary.Models.ProvideMaterialsModel model1 = ProvideMaterialsProcessor.LoadProvideMaterial(model.Id_Material, model.Id_Provider);
                StockMaterialProcessor.CreateStockMaterial(model.Id_Material, model.Id_Provider, model.Id_Section, model.Amount_Material, model.Observations);
                return RedirectToAction("ViewStockMaterial");
            }
            return View();
        }


        public ActionResult EditStockMaterial(string Name_Provider, string Name_Material, string Name_Section)
        {
            DataLibrary.Models.StockMaterialModel DLStockMaterial = StockMaterialProcessor.LoadStockMaterialByNames(Name_Provider, Name_Material, Name_Section)[0];
            Id_StockMaterial_to_edit = DLStockMaterial.Id_StockMaterial;


            StockMaterialModel StockMaterial = new StockMaterialModel();

            StockMaterial.Id_Section = DLStockMaterial.Id_Section;
            StockMaterial.Id_Provider = DLStockMaterial.Id_Provider;
            StockMaterial.Id_Material = DLStockMaterial.Id_Material;
            StockMaterial.Amount_Material = DLStockMaterial.Amount_Material;
            StockMaterial.Observations = DLStockMaterial.Observations;

            return View(StockMaterial);
        }

        [HttpPost]
        public ActionResult EditStockMaterial(StockMaterialModel model)
        {
            //DataLibrary.Models.ProvideMaterialsModel model1 = ProvideMaterialsProcessor.LoadProvideMaterial(model.Id_Material, model.Id_Provider);
            if (StockMaterialProcessor.ExistentStockMaterial(model.Id_Material, model.Id_Provider, model.Id_Section) == null)
            {
                StockMaterialProcessor.EditStockMaterial(Id_StockMaterial_to_edit, model.Id_Provider, model.Id_Material, model.Id_Section, model.Amount_Material, model.Observations);
                return Redirect("~/StockMaterial/ViewStockMaterial");
            }
            ViewBag.Message = "StockMaterial not edited";
            return View(model);
        }


        public ActionResult DeleteStockMaterial(string Name_Provider, string Name_Material, string Name_Section)
        {
            DataLibrary.Models.StockMaterialModel DLStockMaterial = StockMaterialProcessor.LoadStockMaterialByNames(Name_Provider, Name_Material, Name_Section)[0];

            ViewStockMaterialModel StockMaterial = new ViewStockMaterialModel();

            StockMaterial.Name_Provider = Name_Provider;
            StockMaterial.Name_Material = Name_Material;
            StockMaterial.Name_Section = Name_Section;
            StockMaterial.Amount_Material = DLStockMaterial.Amount_Material;
            StockMaterial.Observations = DLStockMaterial.Observations;

            return View(StockMaterial);
        }

        [HttpPost]
        public ActionResult DeleteStockMaterial(ViewStockMaterialModel model)
        {
            StockMaterialProcessor.DeleteStockMaterial(model.Name_Provider, model.Name_Material, model.Name_Section);
            return Redirect("~/StockMaterial/ViewStockMaterial");
        }

        [HttpGet]
        public JsonResult ReturnSectionMaterials( int Id_Section)
        {
            if (Request.IsAjaxRequest())
            {
                return new JsonResult
                {
                    Data = MaterialProcessor.LoadMaterials().Where(x => x.Id_Section == Id_Section)
                                                            .GroupBy(x => new { x.Name_Material })
                                                            .Select(x => x.First())
                                                            .OrderBy(x => x.Name_Material)
                                                            .ToList(),//cuidado com o id do material que vai -> só pertence a um dos providers

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
        public JsonResult ReturnMaterialProviders(int Id_Material)
        {//tenho de retornar os possiveis providers
            List<DataLibrary.Models.ProvideMaterialsModel> ProvideMaterials = ProvideMaterialsProcessor.LoadProvideMaterialsByMaterial(Id_Material);
            List<DataLibrary.Models.ProviderModel> DLProvider = new List<DataLibrary.Models.ProviderModel>();
            foreach(var row in ProvideMaterials)
            {
                DLProvider.Add(new DataLibrary.Models.ProviderModel
                {
                    Id_Provider = row.Id_Provider,
                    Name_Provider = ProviderProcessor.LoadProviderById(row.Id_Provider).Name_Provider
                });
            }
            if (Request.IsAjaxRequest())
            {
                return new JsonResult
                {
                    Data = DLProvider,
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

    }
}