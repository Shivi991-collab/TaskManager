using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TaskManager.Models;
using TaskManager.Services;

namespace TaskManager.Pages
{
    public class IndexModel : PageModel
    {
        private readonly TaskService _taskService;
        private readonly ILogger<IndexModel> _logger;

        public List<TaskItem> Tasks { get; set; } = new();

        public IndexModel(TaskService taskService, ILogger<IndexModel> logger)
        {
            _taskService = taskService;
            _logger = logger;
        }

        public void OnGet()
        {
            Tasks = _taskService.GetAllTasks();
        }

        public IActionResult OnPostToggleComplete(int id)
        {
            _taskService.ToggleComplete(id);
            return RedirectToPage();
        }

        public IActionResult OnPostDelete(int id)
        {
            _taskService.DeleteTask(id);
            _logger.LogInformation("Task {TaskId} deleted from Index page", id);
            return RedirectToPage();
        }
    }
}
