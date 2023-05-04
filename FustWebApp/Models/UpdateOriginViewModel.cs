using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FustWebApp.Models
{
    public class UpdateOriginViewModel
    {

        public Guid Id { get; set; }

        [Required(ErrorMessage ="Field is required")]
        public string OriginName { get; set; }
    }
}
