using BankAppCore.Models;
using System.Collections.Generic;

namespace BankAppCore.ViewModels
{
    public class StatisticsViewModel
    {
        public int totCustomers { get; set; }
        public int totAccounts { get; set; }
        public decimal totBalance { get; set; }
        public List<Customers> Customers { get; set; }

        public StatisticsViewModel()
        {
            totCustomers = 0;
            totAccounts = 0;
            totBalance = 0;
        }

    }
}
