using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.DAL
{
    public class CityRegion
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public int CityID { get; set; }

        public virtual City City { get; set; }
    }
}