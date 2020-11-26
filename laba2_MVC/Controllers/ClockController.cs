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
    }
}