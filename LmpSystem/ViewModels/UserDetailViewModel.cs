using LmpSystem.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace LmpSystem.ViewModels
{
    public class UserDetailViewModel
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Mã")]
        public string Code { get; set; }
        [Display(Name = "Chức danh")]
        public string Title { get; set; }
        [Display(Name = "Tên đăng nhập")]
        public string Username { get; set; }
        [Display(Name = "Họ")]
        public string FirstName { get; set; }
        [Display(Name = "Tên")]
        public string LastName { get; set; }
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }
        [Display(Name = "Giới tính")]
        public string Gender { get; set; }
        [Display(Name = "Ngày sinh")]
        public Nullable<System.DateTime> Birthday { get; set; }
        [Display(Name = "Số điện thoại")]
        public string Phone { get; set; }
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }
        //public string Resume { get; set; }
        //public string Content { get; set; }
        [Display(Name = "Ảnh đại diện")]
        public string Avatar { get; set; }
        [Display(Name = "Trạng thái")]
        public Nullable<bool> Status { get; set; }
        //public Nullable<int> DepartmentId { get; set; }
        [Display(Name = "Người tạo")]
        public string CreatedByUser { get; set; }
        [Display(Name = "Người sửa")]
        public string LastModifiedByUser { get; set; }
        [Display(Name = "Ngày tạo")]
        public Nullable<System.DateTime> CreatedOnDate { get; set; }
        [Display(Name = "Ngày sửa")]
        public Nullable<System.DateTime> LastModifiedOnDate { get; set; }

        [Display(Name = "Vai trò")]
        public string UserRole { get; set; }

        //public virtual Department Department { get; set; }
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<UserTask> UserTask { get; set; }

    }
}