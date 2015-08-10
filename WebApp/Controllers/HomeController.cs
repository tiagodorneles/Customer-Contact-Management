using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WebApp.DAL;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        protected ApplicationDbContext Db = new ApplicationDbContext();
        public string GetRegionsInCity(string id)
        {
            SelectList regions;
            regions = string.IsNullOrEmpty(id) ? new SelectList(Db.CityRegions, "ID", "Name") : new SelectList(Db.CityRegions.Where(c => c.CityID.ToString().Equals(id)), "ID", "Name");
            return new JavaScriptSerializer().Serialize(regions);
        }
    }
}