using System;
using System.ComponentModel.DataAnnotations;


namespace LmpSystem.ViewModels
{   
    public class PostReviewDetailViewModel
    {
        [Key]
        public int Id { get; set; }       
        [Display(Name = "Thời gian đánh giá")]
        public Nullable<System.DateTime> Time { get; set; }
        [Display(Name = "Bài viết được đánh giá")]
        public string PostName { get; set; }
        [Display(Name = "Điểm đánh giá")]
        public Nullable<int> Rating { get; set; }
        [Display(Name = "Lời đánh giá")]
        public string Comment { get; set; }
        [Display(Name = "Thích bài viết")]
        public Nullable<int> LikeIt { get; set; }
        //public int UserId { get; set; }
        [Display(Name = "Người đánh giá")]
        public string Username { get; set; }
        [Display(Name = "Trạng thái đã xem")]
        public Nullable<bool> Status { get; set; }
    }
}