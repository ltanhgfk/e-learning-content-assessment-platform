using LmpSystem.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace LmpSystem.ViewModels
{
    public class PostImageEditViewModel
    {
        [Key]
        public int Id { get; set; }
        //public bool ChangeAvatar { get; set; } = false;
        [Display(Name = "Ảnh đại diện")]
        public string Avatar { get; set; }
        [DataType(DataType.Upload)]
        [Display(Name = "File upload")]
        public HttpPostedFileBase AvatarFile { get; set; }
        public int PostId { get; set; }
    }
    
}