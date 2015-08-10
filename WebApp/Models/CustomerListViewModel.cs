using System.Collections.Generic;
using WebApp.DAL;

namespace WebApp.Models
{
    public class CustomerListViewModel
    {
        public List<Customer> Customers { get; set; }
        public FilterCustomerViewModel Filters { get; set; }
    }

    public class FilterCustomerViewModel
    {
        public string CustomerClassificationID { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public string CityID { get; set; }
        public string CityRegionID { get; set; }
        public string LastPurchaseInitialDate { get; set; }
        public string LastPurchaseFinalDate { get; set; }
        public string SellerID { get; set; }
    }
}