using System.ComponentModel.DataAnnotations;

namespace Lab1_THKTPM.Areas.Accounts.Models
{
    public class CustomerRegistrationViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        public string UserName { get; set; }

        public bool IsEdit { get; set; }

        public bool IsActive { get; set; }
    }
}