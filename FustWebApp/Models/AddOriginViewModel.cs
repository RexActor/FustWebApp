using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FustWebApp.Models
{
    public class AddOriginViewModel
    {
        [Required(ErrorMessage ="Field is required")]
        [DisplayName("Origin")]
        
        public string OriginName { get; set; }
    }
}
