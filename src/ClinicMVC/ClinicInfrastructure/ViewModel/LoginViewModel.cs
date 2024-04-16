using System.ComponentModel.DataAnnotations;


namespace ClinicInfrastructure.ViewModel
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Ім'я користувача")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Display(Name = "Запам'ятати?")]
        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }

    }
}

