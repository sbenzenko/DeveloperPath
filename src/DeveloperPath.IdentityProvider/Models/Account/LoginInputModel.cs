using System.ComponentModel.DataAnnotations;

namespace IdentityProvider.Models.Account
{
    public class LoginInputModel
    {
        [Required(ErrorMessage = "Имя пользователя должно быть заполнено")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Пароль обязателен")]
        public string Password { get; set; }
        public bool RememberLogin { get; set; }
        public string ReturnUrl { get; set; }
    }
}