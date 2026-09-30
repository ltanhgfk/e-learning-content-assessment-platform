using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LmpSystem.Common;
using LmpSystem.ViewModels;

namespace LmpSystem.ViewModels
{    
    public class CateEditViewModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Tiêu đề danh mục")]
        public string Name { get; set; }
        //[Required]
        public string Icon { get; set; }
        [Display(Name = "Vị trí")]
        public Nullable<int> Position { get; set; }
        [Display(Name = "Danh mục gốc")]
        public int ParentId { get; set; }  //parentId
                                           //public IEnumerable<CateList>ParentCateList { get; set; }  //parentId
        [Display(Name = "Thể loại danh mục")]
        public PostType CateType { get; set; }
        [Display(Name = "Trạng thái")]
        public Nullable<bool> Status { get; set; }//bool 
        [Display(Name = "Tên tập tin câu hỏi")]
        public string QuestionFile { get; set; }
        [Display(Name = "Tải tập tin câu hỏi")]
        [DataType(DataType.Upload)]
        public HttpPostedFileBase UploadedQuestionFile { get; set; }
    }
}