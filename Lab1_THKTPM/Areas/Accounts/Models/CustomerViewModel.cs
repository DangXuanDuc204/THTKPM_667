using Lab1_THKTPM.Areas.Accounts.Models;
using Microsoft.AspNetCore.Identity;

namespace Lab1_THKTPM.Areas.Accounts.Models
{
    public class CustomerViewModel
    {
        public List<IdentityUser> Customers { get; set; }
        public CustomerRegistrationViewModel Registration { get; set; }
    }
}
