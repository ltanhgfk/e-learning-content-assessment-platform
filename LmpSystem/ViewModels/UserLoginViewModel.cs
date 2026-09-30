using System.ComponentModel.DataAnnotations;


namespace LmpSystem.ViewModels
{
    public class UserLoginViewModel
    {
        [Display(Name ="Tên đăng nhập")]
        [MaxLength(20)]
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }
        [Display(Name = "Mật khẩu")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required")]
        [MaxLength(20)]
        public string Password { get; set; }
        [Display(Name = "Ghi nhớ")]
        public bool RememberMe { get; set; }
    }
}