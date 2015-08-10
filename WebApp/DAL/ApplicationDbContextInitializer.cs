using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace WebApp.DAL
{
    public class ApplicationDbContextInitializer : CreateDatabaseIfNotExists<ApplicationDbContext>
    {

        private readonly ApplicationUser _seller1 = new ApplicationUser
        {
            Email = @"seller1@sellseverything.com",
            UserName = @"seller1@sellseverything.com"
        };

        private readonly ApplicationUser _seller2 = new ApplicationUser
        {
            Email = @"seller2@sellseverything.com",
            UserName = @"seller2@sellseverything.com"
        };

        private readonly ApplicationUser _admin = new ApplicationUser
        {
            Email = @"admin@sellseverything.com",
            UserName = @"admin@sellseverything.com"
        };

        protected override void Seed(ApplicationDbContext context)
        {
            IdentitySeed(context);

            var cities = new List<City> {
                new City {Name = "Porto Alegre"}
            };

            cities.ForEach(c => context.Cities.Add(c));
            context.SaveChanges();

            var cityRegions = new List<CityRegion>
            {
                new CityRegion {City = cities[0], Name = "Center"},
                new CityRegion {City = cities[0], Name = "North"},
                new CityRegion {City = cities[0], Name = "South"},
                new CityRegion {City = cities[0], Name = "East"},
                new CityRegion {City = cities[0], Name = "West"}
            };

            cityRegions.ForEach(cr => context.CityRegions.Add(cr));
            context.SaveChanges();

            var customerClassifications = new List<CustomerClassification>
            {
                new CustomerClassification{Name="VIP"},
                new CustomerClassification{Name="Regular"},
                new CustomerClassification{Name="Sporadic"}
            };

            customerClassifications.ForEach(c => context.CustomerClassifications.Add(c));
            context.SaveChanges();

            var customers = new List<Customer>
            {
                new Customer{Name = "Martha Sales", PhoneNumber = "51 6589 6589", CityRegion = cityRegions[0], Classification = customerClassifications[0], Gender = Gender.Female, LastPurchase = new DateTime(2015, 05, 13), Seller = _seller1},
                new Customer{Name = "Beta Rocha", PhoneNumber = "51 6982 6952", CityRegion = cityRegions[1],Classification = customerClassifications[1], Gender = Gender.Female, LastPurchase = new DateTime(2015, 03, 20), Seller = _seller1},
                new Customer{Name = "Lucas Costa", PhoneNumber = "51 9965 3654",  CityRegion = cityRegions[2],Classification = customerClassifications[2], Gender = Gender.Male, LastPurchase = new DateTime(2015, 07, 31), Seller = _seller1},
                new Customer{Name = "Arnold Zaratrusta", PhoneNumber = "51 6545 2158", CityRegion = cityRegions[3], Classification = customerClassifications[0], Gender = Gender.Male, LastPurchase = new DateTime(2013, 01, 05), Seller = _seller2},
                new Customer{Name = "Christian da Silva", PhoneNumber = "51 9965 3654", CityRegion = cityRegions[4],Classification = customerClassifications[1], Gender = Gender.Male, LastPurchase = new DateTime(2014, 12, 25), Seller = _seller2},
                new Customer{Name = "Zuleide Santos", PhoneNumber = "51 5698 2145", CityRegion = cityRegions[0], Classification = customerClassifications[2], Gender = Gender.Female, LastPurchase = new DateTime(2011, 10, 31), Seller = _seller2},
            };
            customers.ForEach(c => context.Customers.Add(c));
            context.SaveChanges();
        }

        private void IdentitySeed(ApplicationDbContext context)
        {
            // Roles
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            roleManager.Create(new IdentityRole("Admin"));
            roleManager.Create(new IdentityRole("Seller"));

            // Users
            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));

            userManager.Create(_seller1, ConfigurationManager.AppSettings["Seller1Psw"]);
            userManager.Create(_seller2, ConfigurationManager.AppSettings["Seller2Psw"]);
            userManager.Create(_admin, ConfigurationManager.AppSettings["AdminPsw"]);

            // Add Users To Roles
            userManager.AddToRole(_seller1.Id, "Seller");
            userManager.AddToRole(_seller2.Id, "Seller");
            userManager.AddToRole(_admin.Id, "Admin");
        }
    }
}