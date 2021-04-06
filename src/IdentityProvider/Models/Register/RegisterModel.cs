using System.ComponentModel.DataAnnotations;

namespace IdentityProvider.Models.Register
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Login обязателен")]
        [RegularExpression("^[A-Za-z0-9]+$", ErrorMessage = "Имя пользователя должно содержать только латинские буквы и цифры")]
        [StringLength(50, ErrorMessage = "Имя должно быть минимум 3 и максимум 50 символов", MinimumLength = 3)]
        [Display(Name = "Имя пользователя (login)")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Электронная почта обязательна")]
        [EmailAddress]
        [Display(Name = "Эл. почта")]
        public string Email { get; set; }

        [Required(ErrorMessage = "ФИО обязательно")]
        [StringLength(50, ErrorMessage = "ФИО должно быть минимум 3 и максимум 50 символов", MinimumLength = 3)]
        [Display(Name = "ФИО")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Пароль обязателен")]
        [StringLength(100, ErrorMessage = "Пароль должени иметь длину минимум 6 символов", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Подтверждение пароля")]
        [Required(ErrorMessage = "Поле обязательно")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; }
        
        public string ReturnUrl { get; set; }
    }
}
