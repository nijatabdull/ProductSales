using System.ComponentModel.DataAnnotations;

namespace ProductSale.Web.Models
{
    public class SignInInput
    {
        public string Email { get; set; }
        public string Password { get; set; }
        [Display(Name ="Remember me")]
        public bool IsRemember { get; set; }
    }
}
