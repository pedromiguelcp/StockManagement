using DataLibrary.BusinessLogic;
using StockManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StockManagement.Controllers
{
    public class IntegratorController : Controller
    {
        static int Id_Integrator_to_edit = new int();

        public ActionResult ViewIntegrators()
        {
            List<IntegratorModel> Integrators = new List<IntegratorModel>();

            var data = IntegratorProcessor.LoadIntegrators();

            foreach (var row in data)
            {
                Integrators.Add(new IntegratorModel
                {
                    Name_Integrator = row.Name_Integrator,
                    Phone_Integrator = row.Phone_Integrator,
                    Mail_Integrator = row.Mail_Integrator,
                    Address_Integrator = row.Address_Integrator,
                    Nif_Integrator = row.Nif_Integrator
                });
            }
            return View(Integrators.OrderBy(x => x.Name_Integrator));
        }


        public ActionResult CreateIntegrator()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateIntegrator(IntegratorModel model)
        {
            if (ModelState.IsValid)
            {
                if (IntegratorProcessor.CreateIntegrator(model.Name_Integrator, model.Phone_Integrator, model.Mail_Integrator, model.Address_Integrator, model.Nif_Integrator) == 0)
                {
                    ViewBag.Message = "Integrator not created";
                }
                else
                {
                    ViewBag.Message = "Integrator created";
                    return RedirectToAction("ViewIntegrators");
                }
            }
            return View();
        }


        public ActionResult EditIntegrator(string Name_Integrator)
        {
            DataLibrary.Models.IntegratorModel DLIntegrator = IntegratorProcessor.LoadIntegratorByName(Name_Integrator);

            IntegratorModel Integrator = new IntegratorModel();

            Id_Integrator_to_edit = DLIntegrator.Id_Integrator;
            Integrator.Name_Integrator = DLIntegrator.Name_Integrator;
            Integrator.Phone_Integrator = DLIntegrator.Phone_Integrator;
            Integrator.Mail_Integrator = DLIntegrator.Mail_Integrator;
            Integrator.Address_Integrator = DLIntegrator.Address_Integrator;
            Integrator.Nif_Integrator = DLIntegrator.Nif_Integrator;

            return View(Integrator);
        }

        [HttpPost]
        public ActionResult EditIntegrator(IntegratorModel model)
        {
            if (ModelState.IsValid)
            {
                if (IntegratorProcessor.EditIntegrator(Id_Integrator_to_edit, model.Name_Integrator, model.Phone_Integrator, model.Mail_Integrator, model.Address_Integrator, model.Nif_Integrator) == 0)
                {
                    ViewBag.Message = "Integrator not edited";
                }
                else
                {
                    ViewBag.Message = "Integrator edited";
                    return RedirectToAction("ViewIntegrators");
                }
            }
            return View();
        }


        public ActionResult DeleteIntegrator(string Name_Integrator)
        {
            DataLibrary.Models.IntegratorModel DLIntegrator = IntegratorProcessor.LoadIntegratorByName(Name_Integrator);//meter tudo a ir buscar pelo id, e separar em modulos os controllers

            IntegratorModel Integrator = new IntegratorModel();

            Integrator.Name_Integrator = DLIntegrator.Name_Integrator;
            Integrator.Phone_Integrator = DLIntegrator.Phone_Integrator;
            Integrator.Mail_Integrator = DLIntegrator.Mail_Integrator;
            Integrator.Address_Integrator = DLIntegrator.Address_Integrator;
            Integrator.Nif_Integrator = DLIntegrator.Nif_Integrator;

            return View(Integrator);
        }

        [HttpPost]
        public ActionResult DeleteIntegrator(IntegratorModel model)
        {
            IntegratorProcessor.DeleteIntegrator(model.Name_Integrator);
            return Redirect("ViewIntegrators");
        }

    }
}