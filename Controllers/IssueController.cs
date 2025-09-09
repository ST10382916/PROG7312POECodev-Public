using Microsoft.AspNetCore.Mvc;
using MunicipalServicesMVP.Models;
using MunicipalServicesMVP.Models.DataStructures;

namespace MunicipalServicesMVP.Controllers
{
    public class IssueController : Controller
    {
        private readonly ILogger<IssueController> _logger;

        // Static collections to store data in memory (for MVP - in production would use database)
        private static IssueReportCollection<IssueReport> _issueCollection = new IssueReportCollection<IssueReport>();
        private static CategoryCollection<IssueCategory> _categoryCollection = new CategoryCollection<IssueCategory>();
        
        // Counter for generating unique IDs
        private static int _nextIssueId = 1;
        private static int _nextCategoryId = 1;

        public IssueController(ILogger<IssueController> logger)
        {
            _logger = logger;
            
            // Initialize default categories if collection is empty
            LoadDefaultCategoriesIfEmpty();
        }

        /// <summary>
        /// Display the Report Issues form
        /// </summary>
        public IActionResult ReportIssue()
        {
            // Load active categories for dropdown
            var activeCategories = _categoryCollection.GetActiveCategories();
            ViewBag.Categories = activeCategories;
            
            ViewBag.Title = "Report an Issue";
            ViewBag.Instructions = "Please provide details about the issue you would like to report.";
            
            // Create a new issue report model for the form
            var model = new IssueReport();
            
            return View(model);
        }

        /// <summary>
        /// Handle the submission of a new issue report
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SubmitIssue(IssueReport issue, IList<IFormFile> attachments)
        {
            try
            {
                // Validate the model
                if (!ModelState.IsValid)
                {
                    // Reload categories for dropdown in case of validation errors
                    var activeCategories = _categoryCollection.GetActiveCategories();
                    ViewBag.Categories = activeCategories;
                    ViewBag.ErrorMessage = "Please correct the errors below and try again.";
                    return View("ReportIssue", issue);
                }

                // Assign unique ID
                issue.IssueId = _nextIssueId++;
                
                // Set submission timestamp
                issue.SubmittedAt = DateTime.Now;
                issue.CurrentStatus = IssueStatusType.Submitted;

                // Handle file attachments
                if (attachments != null && attachments.Count > 0)
                {
                    issue.Attachments = ProcessFileAttachments(attachments, issue.IssueId);
                }

                // Find and assign the category
                var category = _categoryCollection.GetAt(issue.CategoryId - 1); // Assuming 1-based CategoryId
                issue.Category = category;

                // Add the issue to our custom collection
                _issueCollection.Add(issue);

                // Log the submission
                _logger.LogInformation($"New issue reported: ID {issue.IssueId}, Category: {issue.Category?.Name}, Location: {issue.Location}");

                // Set success message
                TempData["SuccessMessage"] = $"Your issue has been successfully submitted! Reference ID: {issue.IssueId:D6}";
                TempData["IssueId"] = issue.IssueId;
                
                return RedirectToAction("Success");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error submitting issue report");
                
                // Reload categories for dropdown
                var activeCategories = _categoryCollection.GetActiveCategories();
                ViewBag.Categories = activeCategories;
                ViewBag.ErrorMessage = "An error occurred while submitting your report. Please try again.";
                
                return View("ReportIssue", issue);
            }
        }

        /// <summary>
        /// Display success page after issue submission
        /// </summary>
        public IActionResult Success()
        {
            ViewBag.Title = "Issue Submitted Successfully";
            
            if (TempData["SuccessMessage"] != null)
            {
                ViewBag.SuccessMessage = TempData["SuccessMessage"].ToString();
                ViewBag.IssueId = TempData["IssueId"];
            }
            else
            {
                // If accessed directly without submission, redirect to report form
                return RedirectToAction("ReportIssue");
            }
            
            return View();
        }

        /// <summary>
        /// View all submitted issues (for testing/admin purposes)
        /// </summary>
        public IActionResult ViewIssues()
        {
            var allIssues = _issueCollection.GetAll();
            
            ViewBag.Title = "All Reported Issues";
            ViewBag.TotalCount = _issueCollection.Count;
            
            return View(allIssues);
        }

        /// <summary>
        /// View details of a specific issue
        /// </summary>
        public IActionResult IssueDetails(int id)
        {
            try
            {
                // Find the issue by ID
                var allIssues = _issueCollection.GetAll();
                var issue = allIssues.FirstOrDefault(i => i.IssueId == id);
                
                if (issue == null)
                {
                    ViewBag.ErrorMessage = $"Issue with ID {id} not found.";
                    return View("NotFound");
                }
                
                ViewBag.Title = $"Issue Details - #{issue.IssueId:D6}";
                
                return View(issue);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving issue details for ID {id}");
                ViewBag.ErrorMessage = "An error occurred while retrieving issue details.";
                return View("Error");
            }
        }

        /// <summary>
        /// Get issues by category (AJAX endpoint)
        /// </summary>
        [HttpGet]
        public JsonResult GetIssuesByCategory(int categoryId)
        {
            try
            {
                var allIssues = _issueCollection.GetAll();
                var categoryIssues = allIssues.Where(i => i.CategoryId == categoryId).ToArray();
                
                var result = categoryIssues.Select(i => new {
                    id = i.IssueId,
                    description = i.Description.Length > 100 ? i.Description.Substring(0, 100) + "..." : i.Description,
                    location = i.Location.ToString(),
                    status = i.GetStatusText(),
                    submittedAt = i.SubmittedAt.ToString("yyyy-MM-dd HH:mm")
                });
                
                return Json(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting issues by category {categoryId}");
                return Json(new { error = "An error occurred while retrieving issues." });
            }
        }

        /// <summary>
        /// Process uploaded file attachments
        /// </summary>
        private MediaAttachmentCollection<MediaAttachment> ProcessFileAttachments(IList<IFormFile> files, int issueId)
        {
            var attachmentCollection = new MediaAttachmentCollection<MediaAttachment>();
            int attachmentId = 1;

            foreach (var file in files)
            {
                if (file != null && file.Length > 0)
                {
                    try
                    {
                        // Create attachment record
                        var attachment = new MediaAttachment
                        {
                            AttachmentId = attachmentId++,
                            FileName = file.FileName,
                            FileType = file.ContentType,
                            FileSize = file.Length,
                            IssueReportId = issueId,
                            UploadTime = DateTime.Now
                        };

                        // In a real application, you would save the file to disk/cloud storage
                        // For MVP, we'll just store the file information
                        var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                        if (!Directory.Exists(uploadsPath))
                        {
                            Directory.CreateDirectory(uploadsPath);
                        }

                        var fileName = $"{issueId}_{attachmentId}_{file.FileName}";
                        var filePath = Path.Combine(uploadsPath, fileName);
                        
                        // Save file to disk
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }
                        
                        attachment.FilePath = $"/uploads/{fileName}";
                        
                        // Add to collection using custom method
                        attachmentCollection.AddAttachment(attachment);
                        
                        _logger.LogInformation($"File uploaded: {file.FileName} ({file.Length} bytes)");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Error processing file attachment: {file.FileName}");
                        // Continue processing other files
                    }
                }
            }

            return attachmentCollection;
        }

        /// <summary>
        /// Load default categories if the collection is empty
        /// </summary>
        private void LoadDefaultCategoriesIfEmpty()
        {
            if (_categoryCollection.IsEmpty())
            {
                // Add default South African municipal service categories
                var categories = new[]
                {
                    new IssueCategory { CategoryId = _nextCategoryId++, Name = "Roads and Transport", Description = "Potholes, traffic lights, road maintenance", ResponsibleDepartment = "Public Works", IsActive = true },
                    new IssueCategory { CategoryId = _nextCategoryId++, Name = "Water and Sanitation", Description = "Water leaks, blocked drains, sewage issues", ResponsibleDepartment = "Water Services", IsActive = true },
                    new IssueCategory { CategoryId = _nextCategoryId++, Name = "Electricity", Description = "Power outages, streetlight issues, electrical faults", ResponsibleDepartment = "Electricity Services", IsActive = true },
                    new IssueCategory { CategoryId = _nextCategoryId++, Name = "Waste Management", Description = "Refuse collection, illegal dumping, recycling", ResponsibleDepartment = "Waste Services", IsActive = true },
                    new IssueCategory { CategoryId = _nextCategoryId++, Name = "Housing", Description = "Housing maintenance, property issues", ResponsibleDepartment = "Housing Department", IsActive = true },
                    new IssueCategory { CategoryId = _nextCategoryId++, Name = "Parks and Recreation", Description = "Park maintenance, recreational facilities", ResponsibleDepartment = "Parks Department", IsActive = true },
                    new IssueCategory { CategoryId = _nextCategoryId++, Name = "Public Safety", Description = "Security concerns, emergency services", ResponsibleDepartment = "Public Safety", IsActive = true },
                    new IssueCategory { CategoryId = _nextCategoryId++, Name = "Other", Description = "General municipal issues", ResponsibleDepartment = "General Services", IsActive = true }
                };

                foreach (var category in categories)
                {
                    _categoryCollection.Add(category);
                }

                _logger.LogInformation($"Loaded {categories.Length} default issue categories");
            }
        }

        /// <summary>
        /// Get engagement statistics for the progress bar feature
        /// </summary>
        [HttpGet]
        public JsonResult GetEngagementStats()
        {
            try
            {
                var totalIssues = _issueCollection.Count;
                var progressPercentage = Math.Min(100, (totalIssues * 10)); // Simple engagement calculation
                
                var encouragingMessages = new[]
                {
                    "Thank you for helping improve our community!",
                    "Your participation makes a difference!",
                    "Together we can build a better municipality!",
                    "Every report helps us serve you better!",
                    "Your civic engagement is appreciated!"
                };

                var randomMessage = encouragingMessages[new Random().Next(encouragingMessages.Length)];

                return Json(new 
                { 
                    totalReports = totalIssues,
                    progressPercentage = progressPercentage,
                    message = randomMessage
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting engagement stats");
                return Json(new { error = "Unable to load engagement statistics" });
            }
        }
    }
}

