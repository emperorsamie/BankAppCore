using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankAppCore.Models;

namespace BankAppCore.ViewModels
{
    public class CreateCustomerViewModel
    {
        public Customers Customer { get; set; }
        public Dispositions Dispositions { get; set; }
        public Accounts Accounts { get; set; }
    }
}
