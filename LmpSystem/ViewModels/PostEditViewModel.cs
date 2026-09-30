using LmpSystem.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace LmpSystem.ViewModels
{   
    public class PostEditViewModel
    {
        [Key]
        public int Id { get; set; }       
        [Display(Name = "Mã")]
        public string Code { get; set; }
        [Required]
        [Display(Name = "Tên bài viết")]
        public string Name { get; set; }
        [Display(Name = "Tiêu đề")]
        public string Title { get; set; }
        [Display(Name = "Liên kết trang")]
        public string Pagelink { get; set; }
        [Display(Name = "Ảnh minh họa")]
        public string Avatar { get; set; }
        [DataType(DataType.Upload)]
        public HttpPostedFileBase AvatarFile { get; set; }
        [AllowHtml]
        [Display(Name = "Video giới thiệu (nhúng)")]
        public string Video { get; set; }        
        [AllowHtml]
        [Display(Name = "Tóm tắt")]
        public string Resume { get; set; }
        [AllowHtml]
        [Display(Name = "Nội dung")]
        public string Content { get; set; }
        [Display(Name = "Vị trí")]
        public Nullable<int> Position { get; set; }
        [Display(Name = "Danh mục cha")]
        public int SuperCateId { get; set; }
        [Display(Name = "Danh mục con")]
        //[Range(1, 118, ErrorMessage = "Vui lòng chọn danh mục!")]
        public int CateId { get; set; }
        [Display(Name = "Danh mục bài viết")]
        //[Range(1,118, ErrorMessage = "Vui lòng chọn danh mục!")]
        public List<SelectListItem> CateList { get; set; }        
        [Display(Name = "Thể loại bài viết")]
        //[Range(1, 30, ErrorMessage = "Vui lòng chọn thể loại!")]
        public PostType PostType { get; set; }
        [Display(Name = "Hiển thị")]
        public Nullable<bool> Status { get; set; }        
        [Display(Name = "Tổng thời gian")]
        public string TotalTime { get; set; }
        [Display(Name = "Giá")]
        public Nullable<decimal> Price { get; set; }
        [Display(Name = "Ngày bắt đầu")]
        [DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> BeginDate { get; set; }
        [Display(Name = "Giảm giá")]
        public Nullable<int> Discount { get; set; }
        [Display(Name = "Số lượng")]
        public Nullable<int> Quantity { get; set; }
        [Display(Name = "Người đăng")]
        public int UserId { get; set; } //UserId
        [Display(Name = "Ngày tạo")]
        public System.DateTime CreatedOnDate { get; set; }
        [Display(Name = "Người tạo")]
        public int CreatedByUserId { get; set; }
        [Display(Name = "Ngày sửa")]
        public System.DateTime LastModifiedOnDate { get; set; }
        [Display(Name = "Người sửa")]
        public int LastModifiedByUserId { get; set; }        
        [Display(Name = "Danh sách thẻ được gắn vào bài viết")]        
        public List<SelectListItem> TagList { get; set; }
        [Display(Name = "Danh sách người dùng")]
        //[Range(1, 118, ErrorMessage = "Vui lòng chọn người dùng!")]
        public List<SelectListItem> Users { get; set; }
        [Display(Name = "Tên tập tin câu hỏi")]
        public string QuestionFile { get; set; }
        [Display(Name = "Tải tập tin câu hỏi")]
        [DataType(DataType.Upload)]
        public HttpPostedFileBase UploadedQuestionFile { get; set; }
    }

}