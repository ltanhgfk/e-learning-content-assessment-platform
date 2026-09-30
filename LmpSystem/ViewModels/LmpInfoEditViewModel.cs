using LmpSystem.Common;
using LmpSystem.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace LmpSystem.ViewModels
{    
    public class LmpInfoEditViewModel
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Mã code biến")]
        //[MaxLength(50, ErrorMessage = "Tối đa 50 ký tự")]
        public Code Code { get; set; }
        //public string Code { get; set; }
        //[Display(Name = "")]
        //[Required(ErrorMessage = "Bắt buộc chọn trong danh sách mã")]
        //public Code CodeList { get; set; }
        [Display(Name = "Tên thông tin")]
        public string Name { get; set; }
        [Display(Name = "Ảnh đại diện")]
        public string Avatar { get; set; }
        [Display(Name = "Upload ảnh")]
        [DataType(DataType.Upload)]
        public HttpPostedFileBase AvatarFile { get; set; }
        [AllowHtml]
        [Display(Name = "Nội dung thông tin")]
        public string Content { get; set; }
        [Display(Name = "Liên kết")]
        public string Link { get; set; }
        [Display(Name = "Trạng thái")]
        public Nullable<bool> Status { get; set; }
        [Display(Name = "Người tạo")]
        public string CreatedByUser { get; set; }
        [Display(Name = "Người sửa")]
        public string LastModifiedByUser { get; set; }
        [Display(Name = "Ngày tạo")]
        public Nullable<System.DateTime> CreatedOnDate { get; set; }
        [Display(Name = "Ngày sửa")]
        public Nullable<System.DateTime> LastModifiedOnDate { get; set; }

    }

}