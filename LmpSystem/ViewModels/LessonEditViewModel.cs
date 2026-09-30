using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace LmpSystem.ViewModels
{   
    public class LessonEditViewModel
    {
        [Key]
        public int Id { get; set; }        
        [Required]
        [Display(Name = "Tiêu đề bài học")]
        public string Name { get; set; }       
        [Display(Name = "Ảnh minh họa")]
        public string Avatar { get; set; }
        [Display(Name = "Ảnh upload")]
        [DataType(DataType.Upload)]
        public HttpPostedFileBase AvatarFile { get; set; }
        [AllowHtml]
        [Display(Name = "Video bài học")]
        public string Video { get; set; }        
        [AllowHtml]
        [Display(Name = "Tóm tắt")]
        public string Resume { get; set; }
        [AllowHtml]
        [Display(Name = "Nội dung")]
        public string Content { get; set; }
        [Display(Name = "Vị trí")]
        public Nullable<int> Position { get; set; }
        [Display(Name = "Mã khóa học")]
        public int PostId { get; set; }
        [AllowHtml]
        [Display(Name = "Bài tập thực hành")]
        public string Exercises { get; set; }
        [Display(Name = "Trạng thái")]
        public Nullable<bool> Status { get; set; }       
        public List<SelectListItem> TagList { get; set; }
        [Display(Name = "Tên tập tin câu hỏi")]
        public string QuestionFile { get; set; }
        [Display(Name = "Tải tập tin câu hỏi")]
        [DataType(DataType.Upload)]
        public HttpPostedFileBase UploadedQuestionFile { get; set; }

    }

}