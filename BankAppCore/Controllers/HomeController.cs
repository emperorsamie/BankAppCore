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
    public class HomeController : Controller
    {
        private BankAppDataContext _context;

        public HomeController(BankAppDataContext context)
        {
            _context = context;
        }

        // GET: /<controller>/
        [HttpGet]
        public IActionResult Index()
        {
            var vm = new StatisticsViewModel();

            vm.totCustomers = _context.Customers.Count();
            vm.totAccounts = _context.Accounts.Count();

            var accs = _context.Accounts.ToList();

            foreach (var acc in accs)
            {
                vm.totBalance += acc.Balance;
            }

            return View(vm);
        }

        [HttpPost]
        public IActionResult Index(string searchName, string searchCity)
        {
            var vm = new StatisticsViewModel();

            vm.totAccounts = _context.Accounts.Count();

            var accs = _context.Accounts.ToList();

            foreach (var acc in accs)
            {
                vm.totBalance += acc.Balance;
            }

            if (!string.IsNullOrEmpty(searchName) || !string.IsNullOrEmpty(searchCity))
            {
                var customers = SearchForCustomer(searchName, searchCity);
                vm.Customers = customers;

                vm.totCustomers = vm.Customers.Count();
            }

            return View(vm);
        }

        private List<Customers> SearchForCustomer(string searchName, string searchCity)
        {
            var customers = new List<Customers>();

            if (!string.IsNullOrEmpty(searchName) && !string.IsNullOrEmpty(searchCity))
            {
                customers = _context.Customers
                    .Where(c => (c.Givenname.StartsWith(searchName) || c.Surname.StartsWith(searchName)) && c.City.StartsWith(searchCity))
                    .ToList();
            }
            else if (!string.IsNullOrEmpty(searchName))
            {
                customers = _context.Customers
                    .Where(c => c.Givenname.StartsWith(searchName) || c.Surname.StartsWith(searchName))
                    .ToList();
            }
            else if (!string.IsNullOrEmpty(searchCity))
            {
                customers = _context.Customers
                    .Where(c => c.City.StartsWith(searchCity))
                    .ToList();
            }

            return customers;
        }

        public IActionResult Deposit()
        {
            return View();
        }

        public IActionResult DepositMoney(decimal amount, int accountId)
        {
            var deposit = _context.Accounts.Where(a => a.AccountId == accountId).SingleOrDefault();

            if (amount > 0)
            {
                Transactions transactions = new Transactions
                {
                    AccountId = deposit.AccountId,
                    Date = DateTime.Now,
                    Type = "Debit",
                    Operation = "Withdrawal in cash",
                    Amount = amount,
                    Balance = deposit.Balance + amount
                };

                deposit.Balance += amount;
                _context.Add(transactions);
                _context.SaveChanges();
                return RedirectToAction("DepositApproved");
            }
            else
            {
                return RedirectToAction("DepositDenied");
            }
        }

        public IActionResult DepositApproved()
        {
            return View();
        }

        public IActionResult DepositDenied()
        {
            return View();
        }

        public IActionResult Withdraw()
        {
            return View();
        }

        public IActionResult WithdrawMoney(decimal amount, int accountId)
        {
            var withdraw = _context.Accounts.Where(a => a.AccountId == accountId).SingleOrDefault();

            if (amount > 0)
            {
                Transactions transactions = new Transactions
                {
                    AccountId = withdraw.AccountId,
                    Date = DateTime.Now,
                    Type = "Debit",
                    Operation = "Withdrawal in cash",
                    Amount = amount,
                    Balance = withdraw.Balance - amount
                };

                withdraw.Balance -= amount;
                _context.Add(transactions);
                _context.SaveChanges();
                return RedirectToAction("WithdrawalApproved");
            }
            else if (amount > 0 && amount > withdraw.Balance)
            {
                return RedirectToAction("WithdrawalOverdraw");
            }
            else
            {
                return RedirectToAction("WithdrawalDenied");
            }

        }

        public IActionResult WithdrawalApproved()
        {
            return View();
        }

        public IActionResult WithdrawalDenied()
        {
            return View();
        }

        public IActionResult Transfer()
        {
            return View();
        }

        public IActionResult TransferMoney(decimal amount, int with, int dep)
        {
            var withdraw = _context.Accounts.SingleOrDefault(a => a.AccountId == with);
            var deposit = _context.Accounts.SingleOrDefault(a => a.AccountId == dep);


            if (amount <= withdraw.Balance && amount > 0)
            {
                Transactions withdrawal = new Transactions
                {
                    AccountId = withdraw.AccountId,
                    Date = DateTime.Now,
                    Type = "Debit",
                    Operation = "Withdrawal in cash",
                    Amount = amount * -1,
                    Balance = withdraw.Balance - amount
                };
                deposit.Balance -= amount;

                Transactions deposition = new Transactions
                {
                    AccountId = deposit.AccountId,
                    Date = DateTime.Now,
                    Type = "Debit",
                    Operation = "Withdrawal in cash",
                    Amount = amount,
                    Balance = deposit.Balance + amount
                };

                withdraw.Balance += amount;
                _context.Add(withdrawal);
                _context.Add(deposition);
                _context.SaveChanges();
                return RedirectToAction("TransferApproved");
            }
            //else if (amount > 0 && amount > withdraw.Balance)
            //{
            //    return RedirectToAction("TransferOverdraw");
            //}
            else
            {
                return RedirectToAction("TransferDenied");
            }
        }

        public IActionResult TransferApproved()
        {
            return View();
        }

        public IActionResult TransferDenied()
        {
            return View();
        }
    }
}
