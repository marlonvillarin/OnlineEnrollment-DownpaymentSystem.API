using System.ComponentModel.DataAnnotations;

namespace OnlineEnrollment_DownpaymentSystem.API.Model
{
    public class RegisterAccountModel
    {
        public string? Username { get; set; }

        public string? Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string? ConfirmPassword { get; set; }

        [Required(ErrorMessage = "StudentID is required")] 
        public int StudentID { get; set; }
    }
}
