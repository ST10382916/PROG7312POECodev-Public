using System.ComponentModel.DataAnnotations;

namespace MunicipalServicesMVP.Models
{
    public class MediaAttachment
    {
        public int AttachmentId { get; set; }
        
        [Required]
        [StringLength(255)]
        public string FileName { get; set; } = string.Empty;
        
        [Required]
        [StringLength(500)]
        public string FilePath { get; set; } = string.Empty;
        
        [StringLength(100)]
        public string FileType { get; set; } = string.Empty;
        
        public long FileSize { get; set; }
        
        public DateTime UploadTime { get; set; } = DateTime.Now;
        
        // Foreign key to associate with issue report
        public int IssueReportId { get; set; }
        
        public bool IsValidImageType()
        {
            var validImageTypes = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".tiff" };
            var extension = Path.GetExtension(FileName).ToLowerInvariant();
            return validImageTypes.Contains(extension);
        }
        
        public bool IsValidDocumentType()
        {
            var validDocTypes = new[] { ".pdf", ".doc", ".docx", ".txt", ".rtf" };
            var extension = Path.GetExtension(FileName).ToLowerInvariant();
            return validDocTypes.Contains(extension);
        }
        
        public string GetFileExtension()
        {
            return Path.GetExtension(FileName).ToLowerInvariant();
        }
    }
}

