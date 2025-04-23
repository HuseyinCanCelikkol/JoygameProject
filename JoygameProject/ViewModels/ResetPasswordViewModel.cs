using System.ComponentModel.DataAnnotations;

namespace JoygameProject.Web.ViewModels
{
    public class ResetPasswordViewModel
    {
        public string Token { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; } = string.Empty;

        [Required]
        [Compare("NewPassword")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
