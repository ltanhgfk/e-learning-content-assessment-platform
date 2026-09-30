using System.ComponentModel.DataAnnotations;


namespace LmpSystem.ViewModels
{
    public class TagEditViewModel
    {
        [Key]
        public int TagID { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Tên thẻ")]
        public string TagName { get; set; }        
    }
}