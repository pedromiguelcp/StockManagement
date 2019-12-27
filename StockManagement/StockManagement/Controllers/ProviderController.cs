using DataLibrary.BusinessLogic;
using StockManagement.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StockManagement.Controllers
{
    public class ProviderController : Controller
    {
        static int Id_Provider_to_edit = new int();
        static int Id_Provider_Quotations = new int();
        static List<ProviderModel> Providers = new List<ProviderModel>();

        public ActionResult ViewProviders()
        {
            Providers.Clear();

            var data = ProviderProcessor.LoadProviders();

            foreach (var row in data)
            {
                Providers.Add(new ProviderModel
                {
                    Name_Provider = row.Name_Provider,
                    Phone_Provider = row.Phone_Provider,
                    Mail_Provider = row.Mail_Provider,
                    Address_Provider = row.Address_Provider,
                    Nif_Provider = row.Nif_Provider
                });
            }
            return View(Providers.OrderBy(x => x.Name_Provider));
        }


        public ActionResult CreateProvider()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateProvider(ProviderModel model)
        {
            if (ModelState.IsValid)
            {
                if (ProviderProcessor.CreateProvider(model.Name_Provider, model.Phone_Provider, model.Mail_Provider, model.Address_Provider, model.Nif_Provider, (int)model.Language_Provider) == 0)
                {
                    ViewBag.Message = "Provider not created";
                }
                else
                {
                    ViewBag.Message = "Provider created";
                    return RedirectToAction("ViewProviders");
                }
            }
            return View();
        }


        public ActionResult EditProvider(string Name_Provider)
        {
            DataLibrary.Models.ProviderModel DLProvider = ProviderProcessor.LoadProviderByName(Name_Provider);

            ProviderModel Provider = new ProviderModel();

            Id_Provider_to_edit = DLProvider.Id_Provider;
            Provider.Name_Provider = DLProvider.Name_Provider;
            Provider.Phone_Provider = DLProvider.Phone_Provider;
            Provider.Address_Provider = DLProvider.Address_Provider;
            if (DLProvider.Mail_Provider != "Nada a registar")
                Provider.Mail_Provider = DLProvider.Mail_Provider;
            else
                Provider.Mail_Provider = "";
            Provider.Nif_Provider = DLProvider.Nif_Provider;
            Provider.Language_Provider = DLProvider.Language_Provider;

            return View(Provider);
        }

        [HttpPost]
        public ActionResult EditProvider(ProviderModel model)
        {
            if (ModelState.IsValid)
            {
                if (ProviderProcessor.EditProvider(Id_Provider_to_edit, model.Name_Provider, model.Phone_Provider, model.Mail_Provider, model.Address_Provider, model.Nif_Provider, (int)model.Language_Provider) == 0)
                {
                    ViewBag.Message = "Provider not edited";
                }
                else
                {
                    ViewBag.Message = "Provider edited";
                    return RedirectToAction("ViewProviders");
                }
            }
            return View();
        }


        public ActionResult DeleteProvider(string Name_Provider)
        {
            DataLibrary.Models.ProviderModel DLProvider = ProviderProcessor.LoadProviderByName(Name_Provider);

            ProviderModel Provider = new ProviderModel();

            Provider.Name_Provider = DLProvider.Name_Provider;
            Provider.Phone_Provider = DLProvider.Phone_Provider;
            Provider.Mail_Provider = DLProvider.Mail_Provider;
            Provider.Address_Provider = DLProvider.Address_Provider;
            Provider.Nif_Provider = DLProvider.Nif_Provider;

            return View(Provider);
        }

        [HttpPost]
        public ActionResult DeleteProvider(ProviderModel model)
        {
            ProvideMaterialsProcessor.DeleteProviderMaterialByProvider(ProviderProcessor.LoadProviderByName(model.Name_Provider).Id_Provider);
            StockMaterialProcessor.DeleteProviderfromStockMaterials(ProviderProcessor.LoadProviderByName(model.Name_Provider).Id_Provider);
            SupplyProcessor.DeleteProviderfromSupplys(ProviderProcessor.LoadProviderByName(model.Name_Provider).Id_Provider);
            MachineMaterialProcessor.DeleteProviderfromMachineMaterial(ProviderProcessor.LoadProviderByName(model.Name_Provider).Id_Provider);
            ProviderProcessor.DeleteProvider(model.Name_Provider);

            return Redirect("~/Provider/ViewProviders");
        }
        /*************************************************************************/

        public ActionResult ViewQuotationsProvider(string Name_Provider)
        {
            List<DataLibrary.Models.ProvideMaterialsModel> DLProvideMaterials = ProvideMaterialsProcessor.LoadProviderMaterialsByProviders(ProviderProcessor.LoadProviderByName(Name_Provider).Id_Provider);

            List<ViewMaterialModel> ListProviderMaterialModel = new List<ViewMaterialModel>();

            foreach(var row in DLProvideMaterials)
                ListProviderMaterialModel.Add(new ViewMaterialModel
                {
                    Id_ProvideMaterial = row.Id_ProvideMaterials,
                    Id_Material = row.Id_Material,
                    Name_Section = SectionProcessor.LoadSectionById(MaterialProcessor.LoadMaterialById(row.Id_Material).Id_Section).Name_Section,
                    Name_Material = MaterialProcessor.LoadMaterialById(row.Id_Material).Name_Material,
                    Name_Provider = ProviderProcessor.LoadProviderById(row.Id_Provider).Name_Provider,
                    Unit_Cost = row.Unit_Cost,
                    ExpirationDate = row.ExpirationDate,
                    QuotationPath = row.QuotationPath
                });

            return View(ListProviderMaterialModel);
        }

        public ActionResult DeleteQuotation(int Id_Quotation)
        {
            List<DataLibrary.Models.QuotationModel> DLQuotations = QuotationProcessor.LoadQuotationByProvider(Id_Provider_Quotations);

            ViewQuotationModel Quotation = new ViewQuotationModel();

            foreach (var row in DLQuotations)
            {
                if (row.Id_Quotation == Id_Quotation)
                {
                    Quotation.Id_Quotation = row.Id_Quotation;
                    Quotation.Name_Provider = ProviderProcessor.LoadProviderById(Id_Provider_Quotations).Name_Provider;
                    Quotation.ExpirationDate = row.ExpirationDate;
                }
            }

            return View(Quotation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteQuotation(ViewQuotationModel model)
        {
            QuotationProcessor.DeleteQuotation(model.Id_Quotation);
            return RedirectToAction("ViewQuotationsProvider", model.Name_Provider);
        }

        public ActionResult EditQuotation(int Id_Quotation)
        {
            List<DataLibrary.Models.QuotationModel> DLQuotations = QuotationProcessor.LoadQuotationByProvider(Id_Provider_Quotations);

            ViewQuotationModel Quotation = new ViewQuotationModel();

            foreach (var row in DLQuotations)
            {
                if (row.Id_Quotation == Id_Quotation)
                {
                    Quotation.Id_Quotation = row.Id_Quotation;
                    Quotation.Name_Provider = ProviderProcessor.LoadProviderById(Id_Provider_Quotations).Name_Provider;
                    Quotation.ExpirationDate = row.ExpirationDate;
                }
            }

            return View(Quotation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditQuotation(ViewQuotationModel model)
        {
            QuotationProcessor.DeleteQuotation(model.Id_Quotation);
            return RedirectToAction("ViewQuotationsProvider");
        }

        public ActionResult CreateQuotation()
        {
            ViewQuotationModel model = new ViewQuotationModel();
            model.Name_Provider = ProviderProcessor.LoadProviderById(Id_Provider_Quotations).Name_Provider;
            model.ExpirationDate = DateTime.Now;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateQuotation(ViewQuotationModel model)
        {
            if (ModelState.IsValid)
            {
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
                        //QuotationProcessor.CreateQuotation(Id_Provider_Quotations, model.ExpirationDate, model.QuotationPath);
                        return RedirectToAction("ViewQuotationsProvider");
            }
            return View();
        }

        public FilePathResult SeeQuotationFile(string QuotationPath)
        {
            return File(QuotationPath, "application/pdf");
        }


        [HttpPost]
        public ActionResult _TableProvidersaux(string id)
        {
            Providers.Clear();
            var data = ProviderProcessor.LoadProviders();

            foreach (var row in data)
            {
                Providers.Add(new ProviderModel
                {
                    Name_Provider = row.Name_Provider,
                    Phone_Provider = row.Phone_Provider,
                    Mail_Provider = row.Mail_Provider,
                    Address_Provider = row.Address_Provider,
                    Nif_Provider = row.Nif_Provider
                });
            }

            if (id == "")
            {
                Providers = Providers.OrderBy(x => x.Name_Provider).ToList();
            }
            else
            {
                Providers = Providers.Where(x => x.Name_Provider
                                                                .IndexOf(id, StringComparison.OrdinalIgnoreCase) != -1).ToList();
            }

            return RedirectToAction("_TableProviders");
        }

        public ActionResult _TableProviders(List<ProviderModel> model)
        {
            return PartialView(Providers);
        }
    }
}