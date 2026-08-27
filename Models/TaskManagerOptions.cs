namespace TaskManager.Models
{
    /// <summary>
    /// Settings that are intentionally kept OUT of source code and instead read
    /// from configuration (appsettings.json locally, Azure App Service
    /// "Application settings" / environment variables when deployed).
    /// This satisfies the Task 2.3 configuration-management requirement.
    /// </summary>
    public class TaskManagerOptions
    {
        public const string SectionName = "TaskManager";

        /// <summary>Display name shown in the navbar / page title.</summary>
        public string AppDisplayName { get; set; } = "Student Task Manager";

        /// <summary>Whether the app seeds a couple of sample tasks on first run.</summary>
        public bool SeedSampleData { get; set; } = true;

        /// <summary>A "secret-shaped" setting to demonstrate reading values from
        /// Azure App Service Application Settings rather than hardcoding them.</summary>
        public string SupportContactEmail { get; set; } = "student@example.edu.au";
    }
}
