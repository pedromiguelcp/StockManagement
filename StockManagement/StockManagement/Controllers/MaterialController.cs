using DataLibrary.BusinessLogic;
using StockManagement.Models;
using StockManagement.Models.Material;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StockManagement.Controllers
{
    public class MaterialController : Controller
    {
        static bool Create_Flag = false;
        static string ProvideMaterial_to_edit;
        static string Material_to_edit;

        static MaterialModel MaterialModel = new MaterialModel();
        static List<ViewMaterialModel> ListMaterialModel = new List<ViewMaterialModel>();
        static List<ViewMaterialModel> TotalListMaterialModel = new List<ViewMaterialModel>();
        static List<ViewMaterialModel> Materials = new List<ViewMaterialModel>();
        public ActionResult CreateMaterial()
        {
            Create_Flag = true;

            ListMaterialProviderModel model = new ListMaterialProviderModel();
            model.MaterialModel = MaterialModel;
            model.ListMaterialModel = ListMaterialModel;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateMaterial(ListMaterialProviderModel model)
        {
            if (ModelState.IsValid)
            {
                if(ListMaterialModel.Count() == 0)
                {
                    ViewBag.Message = "Material not created";
                    model.MaterialModel = MaterialModel;
                    model.ListMaterialModel = ListMaterialModel;
                    return View(model);
                }
                if (MaterialProcessor.CreateMaterial(model.MaterialModel.Name_Material, model.MaterialModel.Specs_Material, model.MaterialModel.Id_Section) == 0)
                {
                    ViewBag.Message = "Material not created";//se falhar falha logo no primeiro create pq os providers nao estarao repetidos quando chegar aqui
                    model.MaterialModel = MaterialModel;
                    model.ListMaterialModel = ListMaterialModel;
                    return View(model);
                }
                foreach (var row in ListMaterialModel)
                {
                    if (ProvideMaterialsProcessor.CreateProvideMaterial(ProviderProcessor.LoadProviderByName(row.Name_Provider).Id_Provider, MaterialProcessor.LoadMaterialByName(model.MaterialModel.Name_Material).Id_Material, row.Unit_Cost, row.ExpirationDate, row.QuotationPath) == 0)
                    {
                        ViewBag.Message = "Material not created";//???
                        model.MaterialModel = MaterialModel;
                        model.ListMaterialModel = ListMaterialModel;
                        return View(model);
                    }

                }
                ViewBag.Message = "Material created";
                return RedirectToAction("ViewMaterials");
            }
            return View();
        }

        public ActionResult ViewMaterials()
        {
            CleanListMaterialProviderModel();
            Create_Flag = false;

            var data = MaterialProcessor.LoadMaterials();

            Materials.Clear();
            foreach (var row in data)
            {
                Materials.Add(new ViewMaterialModel
                 {
                     Name_Material = row.Name_Material,
                     Specs_Material = row.Specs_Material,
                     Name_Section = SectionProcessor.LoadSectionById(row.Id_Section).Name_Section
                 });
            }
            return View(Materials);
        }

        public ActionResult DeleteMaterial(string Name_Material)
        {
            DataLibrary.Models.MaterialModel DLMaterial = MaterialProcessor.LoadMaterialByName(Name_Material);

            MaterialModel Material = new MaterialModel();

            Material.Name_Material = DLMaterial.Name_Material;
            Material.Specs_Material = DLMaterial.Specs_Material;

            return View(Material);
        }

        [HttpPost]
        public ActionResult DeleteMaterial(MaterialModel model)
        {
            ProvideMaterialsProcessor.DeleteProvideMaterialByMaterial(MaterialProcessor.LoadMaterialByName(model.Name_Material).Id_Material);
            StockMaterialProcessor.DeleteMaterialfromStockMaterials(MaterialProcessor.LoadMaterialByName(model.Name_Material).Id_Material);
            SupplyProcessor.DeleteMaterialfromSupplys(MaterialProcessor.LoadMaterialByName(model.Name_Material).Id_Material);
            MachineMaterialProcessor.DeleteMaterialfromMachineMaterial(MaterialProcessor.LoadMaterialByName(model.Name_Material).Id_Material);
            MaterialProcessor.DeleteMaterial(model.Name_Material);

            return Redirect("~/Material/ViewMaterials");
        }

        public ActionResult EditMaterial(string Name_Material)
        {
            if (Name_Material != null)
            {
                //List<DataLibrary.Models.MaterialModel> DLMaterial = MaterialProcessor.LoadMaterialsByName(Name_Material);
                DataLibrary.Models.MaterialModel DLMaterial = MaterialProcessor.LoadMaterialByName(Name_Material);
                List<DataLibrary.Models.ProvideMaterialsModel> DLProvideMaterials = ProvideMaterialsProcessor.LoadProvideMaterialsByMaterial(DLMaterial.Id_Material);

                MaterialModel.Name_Material = Name_Material;
                MaterialModel.Specs_Material = DLMaterial.Specs_Material;
                MaterialModel.Id_Section = DLMaterial.Id_Section;
                TotalListMaterialModel.Clear();

                foreach (var row in DLProvideMaterials)
                {
                    ListMaterialModel.Add(new ViewMaterialModel
                    {
                        Id_ProvideMaterial = row.Id_ProvideMaterials,
                        Id_Material = row.Id_Material,
                        Name_Provider = ProviderProcessor.LoadProviderById(row.Id_Provider).Name_Provider,
                        Unit_Cost = row.Unit_Cost,
                        ExpirationDate = row.ExpirationDate,
                        QuotationPath = row.QuotationPath
                    });

                    TotalListMaterialModel.Add(new ViewMaterialModel
                    {
                        Id_ProvideMaterial = row.Id_ProvideMaterials,
                        Id_Material = row.Id_Material,
                        Name_Provider = ProviderProcessor.LoadProviderById(row.Id_Provider).Name_Provider,
                        Unit_Cost = row.Unit_Cost,
                        ExpirationDate = row.ExpirationDate,
                        QuotationPath = row.QuotationPath,
                    });
                }
                Material_to_edit = Name_Material;
            }
            ListMaterialProviderModel model = new ListMaterialProviderModel();
            model.MaterialModel = MaterialModel;
            model.ListMaterialModel = ListMaterialModel;
            return View(model);
        }

        [HttpPost]
        public ActionResult EditMaterial(ListMaterialProviderModel model)
        {
            if (ModelState.IsValid)
            {
                if (ListMaterialModel.Count() == 0)
                {
                    ViewBag.Message = "Material not edited";
                    model.MaterialModel = MaterialModel;
                    model.ListMaterialModel = ListMaterialModel;
                    return View(model);
                }

                if (MaterialProcessor.EditMaterial(MaterialProcessor.LoadMaterialByName(Material_to_edit).Id_Material, model.MaterialModel.Name_Material, model.MaterialModel.Specs_Material, model.MaterialModel.Id_Section) == 0)
                {
                    ViewBag.Message = "Material not edited";
                    model.MaterialModel = MaterialModel;
                    model.ListMaterialModel = ListMaterialModel;
                    return View(model);
                }

                foreach (var row in TotalListMaterialModel)
                    if (existentprovidematerial(row.Id_ProvideMaterial) == 1)
                        ProvideMaterialsProcessor.DeleteProvideMaterial(ProviderProcessor.LoadProviderByName(row.Name_Provider).Id_Provider, row.Id_Material);

                foreach (var row in ListMaterialModel)
                    if(row.Id_ProvideMaterial != 0)
                        ProvideMaterialsProcessor.EditProvideMaterial(row.Id_ProvideMaterial, ProviderProcessor.LoadProviderByName(row.Name_Provider).Id_Provider, row.Id_Material, row.Unit_Cost, row.ExpirationDate, row.QuotationPath);

                foreach (var row in ListMaterialModel)
                {
                    if(row.Id_ProvideMaterial == 0)
                    {
                        if (ProvideMaterialsProcessor.CreateProvideMaterial(ProviderProcessor.LoadProviderByName(row.Name_Provider).Id_Provider, MaterialProcessor.LoadMaterialByName(model.MaterialModel.Name_Material).Id_Material, row.Unit_Cost, row.ExpirationDate, row.QuotationPath) == 0)
                        {
                            ViewBag.Message = "Material not edited";
                            model.MaterialModel = MaterialModel;
                            model.ListMaterialModel = ListMaterialModel;
                            return View(model);
                        }
                    }
                }

                return RedirectToAction("ViewMaterials");
            }
            return View();
        }


        public void CleanListMaterialProviderModel()
        {
            MaterialModel.Id_Provider = 0;
            MaterialModel.Id_Section = 0;
            MaterialModel.Name_Material = "";
            MaterialModel.Specs_Material = "";
            MaterialModel.Unit_Cost = 0;
            ListMaterialModel.Clear();
        }

        public void SaveInfoMaterial(MaterialModel model)
         {
             MaterialModel.Name_Material = model.Name_Material;
             MaterialModel.Specs_Material = model.Specs_Material;
             MaterialModel.Id_Section = model.Id_Section;
         }

        public ActionResult DiscardIncludeProvider()
        {
            if (Create_Flag)
                return RedirectToAction("CreateMaterial");
            return RedirectToAction("EditMaterial");
        }



        [HttpPost]
        public ActionResult IncludeProvidersAux(ListMaterialProviderModel model)
        {
            SaveInfoMaterial(model.MaterialModel);//guardar a info dos campos

            return RedirectToAction("IncludeProvider");
        }

        public ActionResult IncludeProvider()
        {
            MaterialModel.Id_Provider = 0;
            MaterialModel.Unit_Cost = 0;
            MaterialModel.ExpirationDate = DateTime.Now;

            return View(MaterialModel);
        }

        [HttpPost]
        public ActionResult IncludeProvider(MaterialModel model)
        {
            if (ModelState.IsValid)
            {
                if(model.Unit_Cost == 0)
                {
                    ViewBag.Message = "Provider not registered";
                    return View(MaterialModel);
                }
                    
                foreach (var row in ListMaterialModel)
                    if(row.Name_Provider == ProviderProcessor.LoadProviderById(model.Id_Provider).Name_Provider)
                    {
                        ViewBag.Message = "Provider not registered";
                        return View(MaterialModel);
                    }



                if (model.QuotationFile != null && model.QuotationFile.ContentLength > 0)
                    try
                    {
                        /******************************************************************************************/
                        string FileName = Path.GetFileNameWithoutExtension(model.QuotationFile.FileName);

                        string FileExtension = Path.GetExtension(model.QuotationFile.FileName);//To Get File Extension 

                        FileName = DateTime.Now.ToString("yyyyMMdd") + "-" + FileName.Trim() + FileExtension;//Add Current Date To Attached File Name 

                        string UploadPath = ConfigurationManager.AppSettings["UserQuotatioPath"].ToString();//Get Upload path from Web.Config file AppSettings.

                        model.QuotationPath = UploadPath + FileName;//Its Create complete path to store in server.

                        model.QuotationFile.SaveAs(model.QuotationPath);//To copy and save file into server.

                        /******************************************************************************************/

                    }
                    catch (Exception ex)
                    {
                        ViewBag.Message = "ERROR:" + ex.Message.ToString();
                    }
                else
                {
                    ViewBag.Message = "You have not specified a file.";
                }
                //nao tenho provider, esta em listas, só qd criar ou editar
                //QuotationProcessor.CreateQuotation(6, model.Id_Material, model.ExpirationDate, model.QuotationPath);//meter em lista!!!

                ListMaterialModel.Add(new ViewMaterialModel
                {
                    Id_ProvideMaterial = 0,
                    Id_Material = 0,
                    Name_Provider = ProviderProcessor.LoadProviderById(model.Id_Provider).Name_Provider,
                    Unit_Cost = model.Unit_Cost,
                    QuotationPath = model.QuotationPath,//ja tenho o ficheiro guardado na pasta, tenho de ver como se elimina 
                    ExpirationDate = model.ExpirationDate
                });

                ViewBag.Message = "Provider registered";

                if(Create_Flag)
                    return RedirectToAction("CreateMaterial");
                return RedirectToAction("EditMaterial");
            }
            return View(MaterialModel);
        }


        public ActionResult DeleteMaterialProvider(string Name_Provider)
        {
            ListMaterialModel.RemoveAll(x => x.Name_Provider == Name_Provider);
            if(Create_Flag)
                return RedirectToAction("CreateMaterial");
            return RedirectToAction("EditMaterial");
        }


        public ActionResult EditMaterialProvider(string Name_Provider)
        {
            foreach(var row in ListMaterialModel)
            {
                if(row.Name_Provider == Name_Provider)
                {
                    MaterialModel.ExpirationDate = row.ExpirationDate;
                    MaterialModel.Id_Material = row.Id_Material;
                    MaterialModel.Specs_Material = row.Specs_Material;
                    MaterialModel.Id_Provider = ProviderProcessor.LoadProviderByName(row.Name_Provider).Id_Provider;
                    MaterialModel.QuotationPath = row.QuotationPath;
                    MaterialModel.Unit_Cost = row.Unit_Cost;
                }
            }
            ProvideMaterial_to_edit = Name_Provider;
            return View(MaterialModel);
        }

        [HttpPost]
        public ActionResult EditMaterialProvider(MaterialModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Unit_Cost == 0)
                {
                    ViewBag.Message = "Provider not registered";
                    return View(MaterialModel);
                }

                foreach (var row in ListMaterialModel)
                    if ((row.Name_Provider == ProviderProcessor.LoadProviderById(model.Id_Provider).Name_Provider) & (row.Name_Provider != ProvideMaterial_to_edit))
                    {
                        ViewBag.Message = "Provider not registered";
                        return View(MaterialModel);
                    }



                if (model.QuotationFile != null && model.QuotationFile.ContentLength > 0)
                    try
                    {
                        /******************************************************************************************/
                        string FileName = Path.GetFileNameWithoutExtension(model.QuotationFile.FileName);

                        string FileExtension = Path.GetExtension(model.QuotationFile.FileName);//To Get File Extension 

                        FileName = DateTime.Now.ToString("yyyyMMdd") + "-" + FileName.Trim() + FileExtension;//Add Current Date To Attached File Name 

                        string UploadPath = ConfigurationManager.AppSettings["UserQuotatioPath"].ToString();//Get Upload path from Web.Config file AppSettings.

                        model.QuotationPath = UploadPath + FileName;//Its Create complete path to store in server.

                        model.QuotationFile.SaveAs(model.QuotationPath);//To copy and save file into server.

                        /******************************************************************************************/

                    }
                    catch (Exception ex)
                    {
                        ViewBag.Message = "ERROR:" + ex.Message.ToString();
                    }
                else
                {
                    ViewBag.Message = "You have not specified a file.";
                }
                //nao tenho provider, esta em listas, só qd criar ou editar
                //QuotationProcessor.CreateQuotation(6, model.Id_Material, model.ExpirationDate, model.QuotationPath);//meter em lista!!!
                foreach(var row in ListMaterialModel)
                    if(row.Name_Provider == ProvideMaterial_to_edit)
                    {
                        ListMaterialModel.RemoveAll(x => x.Name_Provider == row.Name_Provider);
                        break;
                    }

                ListMaterialModel.Add(new ViewMaterialModel
                {
                    Id_Material = 0,
                    Name_Provider = ProviderProcessor.LoadProviderById(model.Id_Provider).Name_Provider,
                    Unit_Cost = model.Unit_Cost,
                    QuotationPath = model.QuotationPath,//ja tenho o ficheiro guardado na pasta, tenho de ver como se elimina 
                    ExpirationDate = model.ExpirationDate
                });

                ViewBag.Message = "Provider registered";

                return RedirectToAction("EditMaterial");
            }
            return View(MaterialModel);
        }



        public int existentprovidematerial(int id_providematerial)
        {
            foreach (var row in ListMaterialModel)
            {
                if (row.Id_ProvideMaterial == id_providematerial)
                {
                    return 0;
                }
            }
            return 1;
        }

        public FilePathResult SeeQuotationFile(string QuotationPath)
        {
            return File(QuotationPath, "application/pdf");
        }

        [HttpPost]
        public ActionResult _TableMaterialsaux(string id)
        {
            Materials.Clear();
            var data = MaterialProcessor.LoadMaterials();
            foreach (var row in data)
            {
                Materials.Add(new ViewMaterialModel
                {
                    Name_Material = row.Name_Material,
                    Specs_Material = row.Specs_Material,
                    Name_Section = SectionProcessor.LoadSectionById(row.Id_Section).Name_Section
                });
            }

            if (id == "")
            {
                Materials = Materials.OrderBy(x => x.Name_Material).ToList();
            }
            else
            {
                Materials = Materials.Where(x => x.Name_Material
                                                                .IndexOf(id, StringComparison.OrdinalIgnoreCase) != -1).ToList();
            }

            return RedirectToAction("_TableMaterials");
        }

        public ActionResult _TableMaterials(List<ViewMaterialModel> model)
        {
            return PartialView(Materials);
        }

    }
}