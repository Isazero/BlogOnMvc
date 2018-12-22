using System.ComponentModel.DataAnnotations;

namespace MvcBlog.Models
{
    public class LoginModel
    {

        [Display(Name = "Login")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Поле Логин обязательное")]
        [DataType(DataType.Text)]
        public string Login { get; set; }

        [Display(Name = "Password")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Поле Пароль обязательное")]
        [DataType(DataType.Password)]
        public string Password { get; set; }


    }

  
}