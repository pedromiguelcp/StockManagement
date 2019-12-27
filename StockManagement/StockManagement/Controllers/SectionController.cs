using DataLibrary.BusinessLogic;
using StockManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StockManagement.Controllers
{
    public class SectionController : Controller
    {
        static int Id_Section_to_edit = new int();

        public ActionResult ViewSections()
        {
            var data = SectionProcessor.LoadSections();

            List<SectionModel> Sections = new List<SectionModel>();

            foreach (var row in data)
            {
                Sections.Add(new SectionModel
                {
                    Name_Section = row.Name_Section,
                    Description_Section = row.Description_Section
                });
            }
            return View(Sections.OrderBy(x => x.Name_Section));
        }

        public ActionResult CreateSection()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateSection(SectionModel model)
        {
            if (ModelState.IsValid)
            {
                if (SectionProcessor.CreateSection(model.Name_Section, model.Description_Section) == 0)
                {
                    ViewBag.Message = "Section not created";
                }
                else
                {
                    ViewBag.Message = "Section created";
                    return RedirectToAction("ViewSections");
                }
            }
            return View();
        }


        public ActionResult EditSection(string Name_Section)
        {
            DataLibrary.Models.SectionModel DLSection = SectionProcessor.LoadSectionByName(Name_Section);
            Id_Section_to_edit = DLSection.Id_Section;

            SectionModel Section = new SectionModel();
            Section.Name_Section = Name_Section;
            Section.Description_Section = DLSection.Description_Section;

            return View(Section);
        }

        [HttpPost]
        public ActionResult EditSection(SectionModel model)
        {
            if (ModelState.IsValid)
            {
                if (SectionProcessor.EditSection(Id_Section_to_edit, model.Name_Section, model.Description_Section) == 0)
                {
                    ViewBag.Message = "Section not edited";
                }
                else
                {
                    ViewBag.Message = "Section edited";
                    return RedirectToAction("ViewSections");
                }
            }
            return View();
        }


        public ActionResult DeleteSection(string Name_Section)
        {
            DataLibrary.Models.SectionModel DLSection = SectionProcessor.LoadSectionByName(Name_Section);

            SectionModel Section = new SectionModel();

            Section.Name_Section = Name_Section;
            Section.Description_Section = DLSection.Description_Section;

            return View(Section);
        }

        [HttpPost]
        public ActionResult DeleteSection(SectionModel model)
        {
            SectionProcessor.DeleteSection(model.Name_Section);
            return Redirect("~/Section/ViewSections");
        }
    }
}