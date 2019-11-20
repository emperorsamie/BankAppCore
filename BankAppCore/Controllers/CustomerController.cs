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
    public class CustomerController : Controller
    {
        private BankAppDataContext _context;

        public CustomerController(BankAppDataContext context)
        {
            _context = context;
        }

        public ActionResult Index(int CustomerId, int page = 1)
        {
            CustomerDetailsViewModel vm = new CustomerDetailsViewModel();
            var isAjax = Request.Headers["X-Requested-With"] == "XMLHttpRequest";
            const int pageSize = 20;

            var customer = _context.Customers.FirstOrDefault(c => c.CustomerId == CustomerId);

            var dispositionAccountIds = _context.Dispositions
                .Where(d => d.CustomerId == customer.CustomerId)
                .Select(x => x.AccountId)
                .ToList();

            if (dispositionAccountIds == null || dispositionAccountIds.Count == 0)
            {
                return RedirectToAction("Index", "Home");
            }

            var accounts = _context.Accounts
                            .Where(x => dispositionAccountIds.Contains(x.AccountId))
                            .ToList();
            int totalNumber = 0;

            vm.Transactions = new List<Transactions>();

            foreach (var acc in accounts)
            {
                vm.Transactions.AddRange(_context.Transactions
                    .OrderBy(x => x.TransactionId)
                    .Where(t => t.AccountId == acc.AccountId));
            }
            vm.Balance = vm.Transactions.Sum(x => x.Balance);
            totalNumber = vm.Transactions.Count();
            var trans = vm.Transactions
                .OrderBy(x => x.TransactionId)
                .Take(pageSize * page)
                .ToList();

            vm.Customer = customer;
            vm.Accounts = accounts;
            vm.TotalNumberOfItems = totalNumber;
            vm.CanShowMore = page * pageSize < totalNumber;
            vm.PageNumber = page;
            vm.PageSize = pageSize;
            vm.Transactions = trans;

            return PartialView("_CustomerDetails", vm);
        }

        // GET: /<controller>/
        [HttpGet]
        public IActionResult Search(CustomerDetailsViewModel model, int page = 1)
        {
            //var isAjax = Request.Headers["X-Requested-With"] == "XMLHttpRequest";

            //const int pageSize = 20;

            //var totalNumber = _context.Transactions.Where(a => a.AccountId == accountId).Count();

            //var transactions = _context.Transactions.Where(a => a.AccountId == accountId).Skip(pageSize * (page - 1)).Take(pageSize);

            //var model = new CustomerDetailsViewModel
            //{
            //    PageNumber = page,
            //    PageSize = pageSize,
            //    TotalNumberOfItems = totalNumber,
            //    CanShowMore = page * pageSize < totalNumber,
            //    Transactions = transactions.OrderBy(p => p.TransactionId).ToList()
            //};

            //if (isAjax)
            //{
            //    return PartialView("_TransactionRows", model);
            //}


            return PartialView("_CustomerDetails");
        }

        private List<Accounts> GetAccountTransactions(List<int> dispositionAccountIds, int currentPage, int recordsPerPage)
        {
            var accounts = _context.Accounts
                            .Where(x => dispositionAccountIds.Contains(x.AccountId))
                            .ToList();

            foreach (var acc in accounts)
            {
                acc.Transactions = _context.Transactions
                    .Where(t => t.AccountId == acc.AccountId)
                    .Skip(currentPage)
                    .Take(recordsPerPage)
                    .ToList();
            }

            return accounts;
        }
    }
}
