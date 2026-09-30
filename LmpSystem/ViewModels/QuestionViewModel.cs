using System;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;


namespace LmpSystem.ViewModels
{    
    public class QuestionViewModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [AllowHtml]
        [Display(Name = "Nội dung câu hỏi")]
        public string QuestionContent { get; set; }
        [Required]
        [AllowHtml]
        [Display(Name = "Phương án 1")]
        public string Option1 { get; set; }
        [Required]
        [AllowHtml]
        [Display(Name = "Phương án 2")]
        public string Option2 { get; set; }
        [Required]
        [AllowHtml]
        [Display(Name = "Phương án 3")]
        public string Option3 { get; set; }
        [Required]
        [AllowHtml]
        [Display(Name = "Phương án 4")]
        public string Option4 { get; set; }
        [Required]       
        [Display(Name = "Đáp án")]
        public string Answer { get; set; }
        [Required]
        [AllowHtml]
        [Display(Name = "Giải thích")]
        public string AnswerExplain { get; set; }        
        //[Display(Name = "Tên tập tin câu hỏi")]        
        //public string QuestionFile { get; set; }
        //[Display(Name = "Tải tập tin câu hỏi")]
        //[DataType(DataType.Upload)]
        //public HttpPostedFileBase UploadedQuestionFile { get; set; }
        //public Nullable<bool> ChangePos { get; set; }
        [Display(Name = "Thuộc bài viết")]
        public Nullable<int> PostId { get; set; }
        [Display(Name = "Thuộc bài học")]
        public Nullable<int> LessonId { get; set; }        
        [Display(Name = "Trạng thái")]
        public Nullable<bool> Status { get; set; }
        public Nullable<System.DateTime> CreatedOnDate { get; set; }
        public Nullable<int> CreatedByUserId { get; set; }
        public Nullable<System.DateTime> LastModifiedOnDate { get; set; }
        public Nullable<int> LastModifiedByUserId { get; set; }
        public string YourAnswer { get; set; }

}   
}