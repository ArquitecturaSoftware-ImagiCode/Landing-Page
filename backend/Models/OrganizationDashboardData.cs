namespace backend.Models
{
    public class OrganizationDashboardData
    {
        public Organization Organization { get; set; } = null!;
        public List<Module> EnabledModules { get; set; } = new();
        public SubscriptionStatus SubscriptionStatus { get; set; } = null!;
        public List<Notification> Notifications { get; set; } = new();
        public int UnreadNotificationsCount { get; set; }
    }

    public class Module
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsEnabled { get; set; }
        public DateTime? ActivatedAt { get; set; }
    }

    public class SubscriptionStatus
    {
        public bool IsActive { get; set; }
        public string PlanName { get; set; } = string.Empty;
        public string? ExpiresAt { get; set; }
        public string Status { get; set; } = string.Empty;
        public bool IsExpiringSoon { get; set; }
    }

    public class Notification
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public bool IsRead { get; set; }
        public bool IsImportant { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

