using System.ComponentModel.DataAnnotations;

namespace TaskManager.Models
{
    public class TaskItem
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Task title is required.")]
        [StringLength(100, ErrorMessage = "Task title cannot exceed 100 characters.")]
        public string Title { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Subject / Unit is required.")]
        [StringLength(50, ErrorMessage = "Subject cannot exceed 50 characters.")]
        public string Subject { get; set; } = string.Empty;

        [Required(ErrorMessage = "Due date is required.")]
        public DateTime DueDate { get; set; }

        [Required(ErrorMessage = "Priority is required.")]
        public string Priority { get; set; } = "Medium";

        public bool IsCompleted { get; set; }

        // Convenience flags used by the UI to group / style cards
        public bool IsOverdue => !IsCompleted && DueDate.Date < DateTime.Today;
        public bool IsDueSoon => !IsCompleted && !IsOverdue && DueDate.Date <= DateTime.Today.AddDays(3);
    }
}