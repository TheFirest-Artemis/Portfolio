using System.ComponentModel.DataAnnotations;

namespace GameSIteV01.Models
{
    public class UserModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Никнейм пуст")]
        [Display(Name = "Username")]
        [StringLength(16, MinimumLength = 4, ErrorMessage = "Длина: 4 - 16 символов")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email пуст")]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Пароль пуст")]
        [StringLength(32, MinimumLength = 6, ErrorMessage = "Длина: 6 - 32 символов")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required(ErrorMessage ="Подтверждение пусто")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; }

        public DateTime RegistrationDate { get; set; } = DateTime.Now.Date;

        public int TimeInGame { get; set; }

        public int Kills { get; set; }
    }
}
