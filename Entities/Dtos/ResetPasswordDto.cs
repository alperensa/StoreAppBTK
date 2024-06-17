using System.ComponentModel.DataAnnotations;

namespace Entities.Dtos
{
    public record ResetPasswordDto
    {
        public String? Username { get; init; }
        [Required(ErrorMessage = "Enter Password")]
        [DataType(DataType.Password)]
        public String? Password { get; init; }
        [Required(ErrorMessage = "Enter Password again")]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage ="Passwords are not the same")]
        public String? ConfirmPassword { get; init; }
    }
}