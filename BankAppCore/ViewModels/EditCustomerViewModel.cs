using BankAppCore.Models;
using System;
using System.ComponentModel.DataAnnotations;


namespace BankAppCore.ViewModels
{
    public class EditCustomerViewModel
    {
        [Required]
        public int CustomerId { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        public string Givenname { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required]
        public string Streetaddress { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Zipcode { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public string CountryCode { get; set; }

        public DateTime? Birthday { get; set; }
        public string NationalId { get; set; }
        public string Telephonecountrycode { get; set; }
        public string Telephonenumber { get; set; }
        public string Emailaddress { get; set; }

        public virtual Customers Customer { get; set; }
        public virtual Accounts Account { get; set; }
        public virtual Dispositions Disposition { get; set; }
    }
}
