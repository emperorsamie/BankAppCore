using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankAppCore.Models;

namespace BankAppCore.ViewModels
{
    public class TransactionCustomerViewModel
    {
        public List<Customers> Customer { get; set; }
        public List<Dispositions> Dispositions { get; set; }
        public List<Accounts> Accounts { get; set; }
        public List<Transactions> Transactions { get; set; }
        public decimal TotalBalance { get; set; }
        public int Amount { get; set; }
        public Customers Custome { get; set; }
        public Accounts Account{ get; set; }
    }
}
