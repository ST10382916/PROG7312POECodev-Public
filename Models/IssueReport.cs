using System.ComponentModel.DataAnnotations;
using MunicipalServicesMVP.Models.DataStructures;

namespace MunicipalServicesMVP.Models
{
    public class IssueReport
    {
        public int IssueId { get; set; }
        
        [Required]
        public Location Location { get; set; } = new Location();
        
        [Required]
        public int CategoryId { get; set; }
        
        // Navigation property
        public IssueCategory? Category { get; set; }
        
        [Required]
        [StringLength(2000)]
        public string Description { get; set; } = string.Empty;
        
        public DateTime SubmittedAt { get; set; } = DateTime.Now;
        
        public DateTime? LastUpdated { get; set; }
        
        
        public IssueStatusType CurrentStatus { get; set; } = IssueStatusType.Submitted;
        
        // Collection of media attachments using custom data structure
        public MediaAttachmentCollection<MediaAttachment> Attachments { get; set; } = new MediaAttachmentCollection<MediaAttachment>();
        
        [StringLength(1000)]
        public string AdminNotes { get; set; } = string.Empty;
        
        public int Priority { get; set; } = 1;
        
        public string GetPriorityText()
        {
            return Priority switch
            {
                1 => "Low",
                2 => "Medium", 
                3 => "High",
                _ => "Unknown"
            };
        }
        
        public string GetStatusText()
        {
            return CurrentStatus switch
            {
                IssueStatusType.Submitted => "Submitted",
                IssueStatusType.UnderReview => "Under Review",
                IssueStatusType.Resolved => "Resolved",
                IssueStatusType.Rejected => "Rejected",
                _ => "Unknown"
            };
        }
    }
}
