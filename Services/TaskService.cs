using Microsoft.Extensions.Options;
using TaskManager.Models;

namespace TaskManager.Services
{
    /// <summary>
    /// Simple thread-safe in-memory store for TaskItems.
    /// (Swap this for EF Core / a database without touching the Pages.)
    /// </summary>
    public class TaskService
    {
        private readonly List<TaskItem> _tasks = new();
        private readonly object _lock = new();
        private readonly ILogger<TaskService> _logger;
        private int _nextId = 1;

        public TaskService(IOptions<TaskManagerOptions> options, ILogger<TaskService> logger)
        {
            _logger = logger;

            if (options.Value.SeedSampleData)
            {
                SeedSampleData();
            }
        }

        private void SeedSampleData()
        {
            AddTask(new TaskItem
            {
                Title = "Deploy ASP.NET Core app to Azure",
                Description = "Publish the task manager to Azure App Service and verify it's publicly reachable.",
                Subject = "SWE40006",
                DueDate = DateTime.Today.AddDays(2),
                Priority = "High"
            });

            AddTask(new TaskItem
            {
                Title = "Write deployment report",
                Description = "Document setup, screenshots, and troubleshooting for the submission.",
                Subject = "SWE40006",
                DueDate = DateTime.Today.AddDays(7),
                Priority = "Medium"
            });

            _logger.LogInformation("Seeded {Count} sample tasks", _tasks.Count);
        }

        public List<TaskItem> GetAllTasks()
        {
            lock (_lock)
            {
                return _tasks
                    .OrderBy(t => t.IsCompleted)
                    .ThenBy(t => t.DueDate)
                    .ToList();
            }
        }

        public TaskItem? GetTask(int id)
        {
            lock (_lock)
            {
                return _tasks.FirstOrDefault(t => t.Id == id);
            }
        }

        public TaskItem AddTask(TaskItem task)
        {
            lock (_lock)
            {
                task.Id = _nextId++;
                _tasks.Add(task);
                _logger.LogInformation("Added task {TaskId} - {Title}", task.Id, task.Title);
                return task;
            }
        }

        public bool UpdateTask(TaskItem updated)
        {
            lock (_lock)
            {
                var existing = _tasks.FirstOrDefault(t => t.Id == updated.Id);
                if (existing == null)
                {
                    return false;
                }

                existing.Title = updated.Title;
                existing.Description = updated.Description;
                existing.Subject = updated.Subject;
                existing.DueDate = updated.DueDate;
                existing.Priority = updated.Priority;

                _logger.LogInformation("Updated task {TaskId}", existing.Id);
                return true;
            }
        }

        public bool ToggleComplete(int id)
        {
            lock (_lock)
            {
                var task = _tasks.FirstOrDefault(t => t.Id == id);
                if (task == null)
                {
                    return false;
                }

                task.IsCompleted = !task.IsCompleted;
                _logger.LogInformation("Task {TaskId} completion set to {IsCompleted}", task.Id, task.IsCompleted);
                return true;
            }
        }

        public bool DeleteTask(int id)
        {
            lock (_lock)
            {
                var task = _tasks.FirstOrDefault(t => t.Id == id);
                if (task == null)
                {
                    return false;
                }

                _tasks.Remove(task);
                _logger.LogInformation("Deleted task {TaskId}", id);
                return true;
            }
        }
    }
}
