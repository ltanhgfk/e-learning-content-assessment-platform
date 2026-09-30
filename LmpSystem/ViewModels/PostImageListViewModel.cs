using LmpSystem.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace LmpSystem.ViewModels
{
    public class PostImageListViewModel
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Ảnh đại diện")]
        public string Avatar { get; set; }//image
        [Display(Name = "Tên bài viết")]
        public string PostName { get; set; }
    }
}