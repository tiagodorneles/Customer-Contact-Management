using System.Collections.Generic;

namespace WebApp.DAL
{
    public class City
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public List<CityRegion> Regions { get; set; }

    }
}