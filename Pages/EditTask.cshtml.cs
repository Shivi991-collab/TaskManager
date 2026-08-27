using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TaskManager.Models;
using TaskManager.Services;

namespace TaskManager.Pages
{
    public class EditTaskModel : PageModel
    {
        private readonly TaskService _taskService;

        [BindProperty]
        public TaskItem Task { get; set; } = new();

        public EditTaskModel(TaskService taskService)
        {
            _taskService = taskService;
        }

        public IActionResult OnGet(int id)
        {
            var task = _taskService.GetTask(id);

            if (task == null)
            {
                return NotFound();
            }

            Task = task;

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var updated = _taskService.UpdateTask(Task);

            if (!updated)
            {
                return NotFound();
            }

            return RedirectToPage("/Index");
        }
    }
}
