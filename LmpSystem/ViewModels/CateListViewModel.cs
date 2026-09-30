using System;
using System.ComponentModel.DataAnnotations;


namespace LmpSystem.ViewModels
{
    public class CateListViewModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Tiêu đề danh mục")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Ảnh Icon")]
        public string Icon { get; set; }
        [Display(Name = "Vị trí")]
        public Nullable<int> Position { get; set; }
        [Display(Name = "Danh mục cấp trên")]
        public string ParentName { get; set; }  //parentId
        [Display(Name = "Thể loại danh mục")]
        public string CateType { get; set; }
        [Display(Name = "Trạng thái")]
        public bool Status { get; set; }//bool 
        
    }
}