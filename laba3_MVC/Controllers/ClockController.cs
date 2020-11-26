using laba2_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace laba2_MVC.Controllers
{
    public class ClockController : Controller
    {
        private laba2_MVCEntities db = new laba2_MVCEntities();
        // GET: Clock
        public ActionResult Index()
        {
            var br = (from brand in db.Brand select brand).ToList();
            return View(br);
        }

        public ActionResult Details(int id)
        {
            var br = (from brand in db.Brand
                      join type in db.Type on brand.IdType equals type.IdType
                      where brand.IdBrand == id
                      select type).First(); 
            return View(br);
        }

        [HttpGet]
        public ActionResult Create()
        {
            Brand brand = new Brand();
            return View(brand);
        }

        [HttpPost]
        public ActionResult Create(Brand brand)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Brand.Add(brand);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch(Exception e)
            {
                ModelState.AddModelError(String.Empty, e);
            }
            return View(brand);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var brEdit = (from brand in db.Brand where brand.IdBrand == id select brand).First();
            return View(brEdit);
        }

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var brEdit = (from brand in db.Brand where brand.IdBrand == id select brand).First();
            try
            {
                UpdateModel(brEdit);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View(brEdit);
            }
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var brDelete = (from brand in db.Brand where brand.IdBrand == id select brand).First();
            return View(brDelete);
        }

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            var brDelete = (from brand in db.Brand where brand.IdBrand == id select brand).First();
            try
            {
                db.Brand.Remove(brDelete);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View(brDelete);
            }
        }
    }
}