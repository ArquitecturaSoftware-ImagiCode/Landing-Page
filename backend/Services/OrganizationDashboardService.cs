using backend.Db;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Services
{
    public class OrganizationDashboardService
    {
        private readonly AppDbContext _context;

        public OrganizationDashboardService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<OrganizationDashboardData> GetDashboardDataAsync(string clerkOrgId)
        {
            var organization = await _context.Organizations
                .Include(o => o.Owner)
                .FirstOrDefaultAsync(o => o.ClerkOrgId == clerkOrgId);

            if (organization == null)
            {
                throw new ArgumentException($"Organización no encontrada con ClerkOrgId: {clerkOrgId}");
            }

            // Los módulos actualmente son simulados. 
            // Integración futura con AgoraSoftAdmin para ver los modulos
            var modules = GetSimulatedModules();

            var subscription = await _context.Subscriptions
                .FirstOrDefaultAsync(s => s.OrganizationId == clerkOrgId);

            var subscriptionStatus = new SubscriptionStatus
            {
                IsActive = subscription?.Status == "active",
                PlanName = subscription?.PriceId ?? "Sin plan",
                ExpiresAt = subscription?.CurrentPeriodEnd.ToString("yyyy-MM-dd"),
                Status = subscription?.Status ?? "inactive",
                IsExpiringSoon = subscription != null && 
                                 subscription.CurrentPeriodEnd <= DateTime.UtcNow.AddDays(7)
            };

            //Las notificaciones actualmente son simuladas lo mimo de arriba
            var notifications = GetSimulatedNotifications(organization.Id);

            return new OrganizationDashboardData
            {
                Organization = organization,
                EnabledModules = modules,
                SubscriptionStatus = subscriptionStatus,
                Notifications = notifications,
                UnreadNotificationsCount = notifications.Count(n => !n.IsRead)
            };
        }

        //Conectar con AgoraSoft Admin para obtener módulos reales ya que lo hice para que retorne módulos de ejemplo para demostración
        private List<Module> GetSimulatedModules()
        {
            return new List<Module>
            {
                new Module 
                { 
                    Id = 1, 
                    Name = "Gestión de Locales", 
                    Description = "Administra los locales de tu plaza",
                    IsEnabled = true,
                    ActivatedAt = DateTime.UtcNow.AddMonths(-2)
                },
                new Module 
                { 
                    Id = 2, 
                    Name = "Control de Pagos", 
                    Description = "Gestiona pagos y facturas",
                    IsEnabled = true,
                    ActivatedAt = DateTime.UtcNow.AddMonths(-1)
                },
                new Module 
                { 
                    Id = 3, 
                    Name = "Reportes Avanzados", 
                    Description = "Genera reportes personalizados",
                    IsEnabled = false,
                    ActivatedAt = null
                },
                new Module 
                { 
                    Id = 4, 
                    Name = "Notificaciones SMS", 
                    Description = "Envía notificaciones por SMS",
                    IsEnabled = false,
                    ActivatedAt = null
                }
            };
        }

        //Notificaciones de ejemplo
        private List<Notification> GetSimulatedNotifications(int organizationId)
        {
            return new List<Notification>
            {
                new Notification
                {
                    Id = 1,
                    Title = "Pago pendiente",
                    Message = "Tienes un pago pendiente para el local 101",
                    IsRead = false,
                    IsImportant = true,
                    CreatedAt = DateTime.UtcNow.AddDays(-1)
                },
                new Notification
                {
                    Id = 2,
                    Title = "Nuevo contrato",
                    Message = "Se ha registrado un nuevo contrato para el local 205",
                    IsRead = false,
                    IsImportant = false,
                    CreatedAt = DateTime.UtcNow.AddDays(-3)
                },
                new Notification
                {
                    Id = 3,
                    Title = "Mantenimiento programado",
                    Message = "Mantenimiento del sistema el próximo domingo",
                    IsRead = true,
                    IsImportant = false,
                    CreatedAt = DateTime.UtcNow.AddDays(-5)
                }
            };
        }

        
        public async Task MarkNotificationAsReadAsync(int notificationId, string clerkOrgId)
        {
            await Task.CompletedTask;
        }
    }
}

