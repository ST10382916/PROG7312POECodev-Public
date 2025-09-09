using System.ComponentModel.DataAnnotations;

namespace MunicipalServicesMVP.Models
{
    public class Location
    {
        [Required]
        [StringLength(500)]
        public string Address { get; set; } = string.Empty;
        
        [StringLength(100)]
        public string Area { get; set; } = string.Empty;
        
        [StringLength(100)]
        public string City { get; set; } = string.Empty;
        
        [StringLength(20)]
        public string PostalCode { get; set; } = string.Empty;
        
        public override string ToString()
        {
            return $"{Address}, {Area}, {City} {PostalCode}".Trim(' ', ',');
        }
    }
}
