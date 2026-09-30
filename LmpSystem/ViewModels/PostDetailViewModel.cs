using LmpSystem.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace LmpSystem.ViewModels
{   
    public class PostDetailViewModel
    {
        [Key]
        public int Id { get; set; }        
        [Display(Name = "Mã bài viết")]
        public string Code { get; set; }
        [Required]
        [Display(Name = "Tên bài viết")]
        public string Name { get; set; }
        [Display(Name = "Tiêu đề bài viết")]
        public string Title { get; set; }
        [Display(Name = "Địa chỉ trang liên kết")]
        public string Pagelink { get; set; }
        [Display(Name = "Ảnh minh họa")]
        public string Avatar { get; set; }
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
        [Display(Name = "Id danh mục bài viết")]
        public int CateId { get; set; }
        [Display(Name = "Danh mục bài viết")]
        public string CateName { get; set; }
        [Display(Name = "Thể loại bài viết")]
        public string PostType { get; set; }
        [Display(Name = "Hiển thị")]
        public Nullable<bool> Status { get; set; }
        //[Display(Name = "Bài tập tổng hợp")]
        //public string Exercises { get; set; }
        [Display(Name = "Tổng thời gian")]
        public string TotalTime { get; set; }
        [Display(Name = "Giá")]
        public Nullable<decimal> Price { get; set; }
        [Display(Name = "Ngày bắt đầu")]
        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public string BeginDate { get; set; }
        [Display(Name = "Giảm giá")]
        public Nullable<int> Discount { get; set; }
        [Display(Name = "Số lượng")]
        public Nullable<int> Quantity { get; set; }
        [Display(Name = "Id Người dùng")]
        public string Username { get; set; } //UserId
        [Display(Name = "Tên Người dùng")]
        public int UserId { get; set; } //UserId
        [Display(Name = "Người tạo")]
        public int CreatedByUserId { get; set; }
        [Display(Name = "Ngày tạo")]
        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public System.DateTime CreatedOnDate { get; set; }
        [Display(Name = "Ngày sửa")]
        public System.DateTime LastModifiedOnDate { get; set; }
        [Display(Name = "Người sửa")]
        public int LastModifiedByUserId { get; set; }
        [Display(Name = "Danh sách bình luận")]
        public List<PostReview> Reviews { get; set; }
        [Display(Name = "Danh sách thẻ được gắn vào bài viết")]
        public ICollection<Tag> PostTags { get; set; }
        //public List<PostTag> PostTags { get; set; }
        [Display(Name = "Danh sách hình ảnh bài viết")]
        public List<PostImage> PostImages { get; set; }
        [Display(Name = "Mục lục bài học")]
        public List<Lesson> Lessons { get; set; }
        [Display(Name = "Danh sách câu hỏi")]
        public ICollection<Question> Questions { get; set; }
    }

}