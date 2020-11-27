using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using laba4_MVC.Models;

namespace laba4_MVC.Controllers
{
    public class OneBrand
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class OneType
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class WApiController : ApiController
    {
        private laba4_MVCEntities db = new laba4_MVCEntities();

        public ICollection<OneBrand> GetBrand()
        {
            var br = (from brand in db.Brands select brand).ToList();
            Collection<OneBrand> obr = new Collection<OneBrand>();
            foreach(Brand b in br)
            {
                obr.Add(new OneBrand { Id = b.IdBrand, Name = b.Name });
            }
            return obr;
        }

        public ICollection<OneType> GetType(int id)
        {
            var types = (from brand in db.Brands
                         join type in db.Types on brand.IdType equals type.IdType
                         where brand.IdBrand == id
                         select type).ToList();
            Collection<OneType> ot = new Collection<OneType>();
            foreach (Models.Type t in types)
            {
                ot.Add(new OneType { Id = t.IdType, Name = t.Name });
            }
            return ot;
        }
    }
}
