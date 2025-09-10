using System.ComponentModel.DataAnnotations;

namespace MunicipalServicesMVP.Models
{
    public class IssueCategory
    {
        public int CategoryId { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;
        
        [StringLength(100)]
        public string ResponsibleDepartment { get; set; } = string.Empty;
        
        public bool IsActive { get; set; } = true;
    }
}



