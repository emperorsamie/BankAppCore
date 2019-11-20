using BankAppCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace BankAppCore.ViewModels
{
    public class CustomerDetailsViewModel
    {
        public Customers Customer { get; set; }
        public List<Transactions> Transactions { get; set; }
        public List<Dispositions> Dispositions { get; set; }
        public List<Accounts> Accounts{ get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public int TotalNumberOfItems { get; set; }

        public bool CanShowMore { get; set; }
        public decimal Balance { get; internal set; }
    }
}
