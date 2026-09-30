using LmpSystem.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace LmpSystem.ViewModels
{   
    public class LessonDetailViewModel
    {
        [Key]
        public int Id { get; set; } 
        [Required]
        [Display(Name = "Tên bài học")]
        public string Name { get; set; }
        [Display(Name = "Ảnh minh họa")]
        public string Avatar { get; set; }
        [AllowHtml]
        [Display(Name = "Video bài học")]
        public string Video { get; set; }
        [AllowHtml]
        [Display(Name = "Tóm tắt")]
        public string Resume { get; set; }
        [AllowHtml]
        [Display(Name = "Nội dung bài học")]        
        public string Content { get; set; }
        [Display(Name = "Vị trí")]
        public Nullable<int> Position { get; set; }
        [AllowHtml]
        [Display(Name = "Bài tập thực hành")]
        public string Exercises { get; set; }
        [Display(Name = "Mã khóa học")]
        public int PostId { get; set; }
        [Display(Name = "Thuộc bài viết")]
        public string PostName { get; set; }
        [Display(Name = "Trạng thái hiển thị")]
        public Nullable<bool> Status { get; set; }
        [Display(Name = "Danh sách thẻ được gắn vào bài học")]
        //public List<LessonTag> LessonTags { get; set; }
        public  ICollection<Tag> LessonTags { get; set; }
        [Display(Name = "Danh sách câu hỏi")]
        public ICollection<Question> Questions { get; set; }

        //[Display(Name = "Danh sách điểm")]
        //public List<UserScore> UserScores { get; set; }

    }

}