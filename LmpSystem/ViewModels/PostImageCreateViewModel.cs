using LmpSystem.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace LmpSystem.ViewModels
{   
    public class PostImageCreateViewModel
    {
        public int Id { get; set; }
        //public string[] ImageFileNames { get; set; }
        //[DataType(DataType.Upload)]
        [Required(ErrorMessage = "Phải chọn tập tin")]
        [Display(Name = "Browse Files")]
        public HttpPostedFileBase[] ImageFiles { get; set; }
        [Required]        
        public Nullable<int> PostId { get; set; }        
    }

}