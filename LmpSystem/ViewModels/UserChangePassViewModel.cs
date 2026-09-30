using System.ComponentModel.DataAnnotations;


namespace LmpSystem.ViewModels
{
    public class UserChangePassViewModel
    {
        [Required(ErrorMessage = "Chưa nhập password")]
        [StringLength(100)]
        [DataType(DataType.Password)]
        [Display(Name ="Mật khẩu mới")]
        public string Password { get; set; }

        [Required]
        [StringLength(100)]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Bạn nhập mật khẩu mới không trùng nhau!")]
        [Display(Name = "Nhập lại mật khẩu mới")]
        public string RePassword { get; set; }

        [Required(ErrorMessage = "Chưa nhập password cũ")]
        [StringLength(100)]
        [DataType(DataType.Password)]
        [Display(Name = "Nhập mật khẩu cũ")]
        public string OldPassword { get; set; }
    }
}