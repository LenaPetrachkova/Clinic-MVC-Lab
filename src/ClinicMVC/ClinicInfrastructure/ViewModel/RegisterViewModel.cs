using System.ComponentModel.DataAnnotations;

namespace ClinicInfrastructure.ViewModel
{
    public class RegisterViewModel
    {
        [Display(Name = "ПІБ")]
        public string Username { get; set; }

        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name ="Пароль")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage ="Паролі не співпадають")]
        [Display(Name ="Підвердження паролю")]
        [DataType (DataType.Password)]

        public string ConfirmPassword { get; set; }

        public int ClinicId { get; set; }
    }
}
