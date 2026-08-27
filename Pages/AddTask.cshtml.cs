using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TaskManager.Models;
using TaskManager.Services;

namespace TaskManager.Pages
{
    public class AddTaskModel : PageModel
    {
        private readonly TaskService _taskService;

        [BindProperty]
        public TaskItem Task { get; set; } = new();

        public AddTaskModel(TaskService taskService)
        {
            _taskService = taskService;
        }

        public void OnGet()
        {
            Task.DueDate = DateTime.Today;
            Task.Priority = "Medium";
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _taskService.AddTask(Task);

            return RedirectToPage("/Index");
        }
    }
}
