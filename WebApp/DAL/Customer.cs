using System;

namespace WebApp.DAL
{
    public class Customer
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        public int CityRegionID { get; set; }

        public int CustomerClassificationID { get; set; }
        public string SellerID { get; set; }

        public Gender Gender { get; set; }
        public DateTime LastPurchase { get; set; }

        public virtual ApplicationUser Seller { get; set; }
        public virtual CityRegion CityRegion { get; set; }
        public virtual CustomerClassification Classification { get; set; }

    }
}