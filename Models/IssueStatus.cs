namespace MunicipalServicesMVP.Models
{
    public enum IssueStatusType
    {
        Submitted = 1,
        UnderReview = 2,
        Resolved = 3,
        Rejected = 4
    }

    public class IssueStatus
    {
        public int StatusId { get; set; }
        public IssueStatusType StatusType { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ColorCode { get; set; } = "#000000"; // For UI display
    }
}
