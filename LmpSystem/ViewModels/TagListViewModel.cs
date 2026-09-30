using System.ComponentModel.DataAnnotations;

namespace LmpSystem.ViewModels
{
    public class TagListViewModel
    {
        [Key]
        public int TagID { get; set; }

        [StringLength(50)]
        [Display(Name = "Tên thẻ")]
        public string TagName { get; set; }
        [Display(Name = "Số lượng bài viết")]
        public int PostCount { get; set; }
    }
}