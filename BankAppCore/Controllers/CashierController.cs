using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BankAppCore.Models;
using BankAppCore.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BankAppCore.Controllers
{
    public class CashierController : Controller
    {
        // GET: /<controller>/

        private BankAppDataContext _context;

        public CashierController(BankAppDataContext context)
        {
            _context = context;
        }
        public IActionResult CreateCustomer()
        {
            var model = new CreateCustomerViewModel();

            return View(model);
        }

        [HttpPost]
        public IActionResult CreateCustomer(CreateCustomerViewModel model)
        {
            if (ModelState.IsValid)
            {
                Accounts account = new Accounts
                {
                    Created = DateTime.Now,
                    Frequency = "Monthly",
                    Balance = 0m,
                };

                Dispositions disposition = new Dispositions
                {
                    Type = "Owner",
                    Customer = model.Customer,
                    Account = account
                };

                model.Accounts = account;
                model.Dispositions = disposition;
                _context.AddRange(model.Customer, model.Dispositions, model.Accounts);
                _context.SaveChanges();

                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

        public IActionResult EditCustomer()
        {
            var model = new EditCustomerViewModel();

            return View(model);
        }

        // Ändra kund
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditCustomer(EditCustomerViewModel model, int CustomerId)
        {
            var c = new Customers();

            c = _context.Customers.Where(x => x.CustomerId == CustomerId).SingleOrDefault();

            if (c.CustomerId == CustomerId)
            {
                c.Birthday = model.Birthday;
                c.City = model.City;
                c.Country = model.Country;
                c.CountryCode = model.CountryCode;
                c.Emailaddress = model.Emailaddress;
                c.Gender = model.Gender;
                c.Givenname = model.Givenname;
                c.Surname = model.Surname;
                c.Telephonecountrycode = model.Telephonecountrycode;
                c.Telephonenumber = model.Telephonenumber;
                c.Zipcode = model.Zipcode;
                c.NationalId = model.NationalId;
                c.Streetaddress = model.Streetaddress;
            }

            if (ModelState.IsValid)
            {
                _context.Customers.Update(c);
                _context.SaveChanges();
                ModelState.Clear();

                return RedirectToAction("EditCustomerSuccessful", "Cashier");
            }
            else
            {
                return RedirectToAction("EditCustomerFailure", "Cashier");
            }
        }

        public IActionResult EditCustomerSuccessful()
        {
            return View();
        }

        public IActionResult EditCustomerFailure()
        {
            return View();
        }
    }
}
