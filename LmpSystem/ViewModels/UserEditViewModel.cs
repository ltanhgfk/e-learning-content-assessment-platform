using LmpSystem.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations.Schema;

namespace LmpSystem.ViewModels
{
    public class UserEditViewModel
    {
        [Key]
        [Column(Order = 1)]
        public int Id { get; set; }
        [Display(Name = "Mã người dùng")]
        public string Code { get; set; }
        [Display(Name = "Chức danh/Danh xưng")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Chưa nhập Username, tối đa 50 ký tự")]
        [StringLength(50)]
        [Display(Name = "Tên đăng nhập")]
        [Key]
        [Column(Order = 2)]
        public string Username { get; set; }
        [Required(ErrorMessage = "Chưa nhập họ")]
        [StringLength(50)]
        [Display(Name = "Nhập họ")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Chưa nhập tên")]
        [StringLength(50)]
        [Display(Name = "Nhập tên")]
        public string LastName { get; set; }

        //[Required(ErrorMessage = "Chưa nhập mật khẩu")]
        //[StringLength(100)]
        //[DataType(DataType.Password)]
        //[Display(Name = "Nhập mật khẩu")]
        //public string Password { get; set; }
        //[Required(ErrorMessage = "Bạn phải nhập lại mật khẩu")]
        //[StringLength(100)]
        //[DataType(DataType.Password)]
        //[System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Bạn nhập mật khẩu không trùng nhau!")]
        //[Display(Name = "Nhập lại mật khẩu")]
        //public string RePassword { get; set; }

        [Display(Name = "Giới tính")]
        public string Gender { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Ngày sinh")]
        public Nullable<System.DateTime> Birthday { get; set; }
        [Phone]
        [Display(Name = "Số điện thoại")]
        public string Phone { get; set; }
        [EmailAddress(ErrorMessage ="Địa chỉ email không hợp lệ!")]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }
        [AllowHtml]
        [Display(Name = "Tóm tắt tiểu sử")]
        public string Resume { get; set; }
        [AllowHtml]
        [Display(Name = "Thông tin chi tiết")]
        public string Content { get; set; }
        //public bool ChangeAvatar { get; set; } = false;
        [Display(Name = "Tên đăng nhập")]
        public string Avatar { get; set; }
        [DataType(DataType.Upload)]
        [Display(Name = "Tập tin ảnh")]
        public HttpPostedFileBase AvatarFile { get; set; }
        [Display(Name = "Trạng thái")]
        public Nullable<bool> Status { get; set; }
        //[Display(Name = "Mã phòng ban")]
        //public Nullable<int> DepartmentId { get; set; }
        //[Display(Name = "Danh sách phòng ban")]
        //public virtual List<SelectListItem> Department { get; set; }
        [Display(Name = "Vai trò người dùng")]
        public Role UserRole { get; set; }
        public Nullable<int> CreatedByUserId { get; set; }
        public Nullable<int> LastModifiedByUserId { get; set; }
        public Nullable<System.DateTime> CreatedOnDate { get; set; }
        public Nullable<System.DateTime> LastModifiedOnDate { get; set; }
        
    }
}