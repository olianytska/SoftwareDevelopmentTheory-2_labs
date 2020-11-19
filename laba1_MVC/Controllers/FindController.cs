using laba1_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace laba1_MVC.Controllers
{
    public class FindController : Controller
    {
        // GET: Find
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(OperandsModel model)
        {
            string result = string.Empty;
            List<string> arrAllString = model.toArr.Split(' ').ToList();
            List<int> arrAll = new List<int>();
            List<int> arr2 = new List<int>();
            List<string> newResult = new List<string>();
            try
            {
                for(int i = 0; i < arrAllString.Count; i++)
                {
                    arrAll.Add(int.Parse(arrAllString[i]));
                }
                for (int i = 0; i < arrAll.Count; i++)
                {
                    if (arrAll[i] == model.x)
                    {
                        arr2.Add(i);
                    }
            
                  
                }
                for(int i = 0; i < arr2.Count; i++)
                {
                    newResult.Add($"Number {model.x} stays in position {arr2[i] + 1} ");
                }

                result = newResult.Aggregate((curr, next) => curr + ", " + next);
            }
            catch (Exception e)
            {
                result = "Error: " + e.Message.ToString();
            }
            ViewBag.Result = result;
            return View();
        }
    }
}