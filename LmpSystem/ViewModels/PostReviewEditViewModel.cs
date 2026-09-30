using LmpSystem.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace LmpSystem.ViewModels
{   
    public class PostReviewEditViewModel
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Thời gian đánh giá")]
        public Nullable<System.DateTime> Time { get; set; }
        [Required]
        [Display(Name = "Bài viết được đánh giá")]
        public int PostId { get; set; }
        [Display(Name = "Danh sách bài viết")]
        public List<SelectListItem> Posts { get; set; }
        
        [Display(Name = "Điểm đánh giá")]
        [Range(1,5, ErrorMessage ="Điểm đánh giá từ chỉ 1-5")]
        public Nullable<int> Rating { get; set; }
        [Display(Name = "Lời đánh giá")]
        public string Comment { get; set; }
        [Display(Name = "Thích bài viết")]
        public Nullable<int> LikeIt { get; set; }
        //public int UserId { get; set; }
        [Required]
        [Display(Name = "Người đánh giá")]
        public int UserId { get; set; }
        [Display(Name = "Danh sách người dùng")]
        public List<SelectListItem> Users { get; set; }
        [Display(Name = "Trạng thái đã xem")]
        public Nullable<bool> Status { get; set; }

    }

}