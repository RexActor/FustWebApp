using Microsoft.Build.Framework;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FustWebApp.Models.Domain
{
    public class Origin
    {
        public Guid Id { get; set; }

        [Display(Name ="Name")]
        public string OriginName { get; set; }

    }
}
