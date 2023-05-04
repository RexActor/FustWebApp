using Microsoft.Build.Framework;

using System.ComponentModel;

namespace FustWebApp.Models.Domain
{
    public class FustType
    {

        public int FustTypeId { get; set; }
        [DisplayName("Type")]
        public string FustTypeName { get; set; }
        public string? Comment { get; set; } = "N/A";
       
    }
}
