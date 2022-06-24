using ClubManagementSystem.Data;
using ClubManagementSystem.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClubManagementSystem.Services
{
    public class MembershipNotificationService : IHostedService, IDisposable
    {
        private Timer? _timer = null;
        private readonly ILogger<MembershipNotificationService> _logger;
        private readonly IServiceProvider _serviceProvider;

        public MembershipNotificationService(
            IServiceProvider serviceProvider,
            ILogger<MembershipNotificationService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            var currentDate = DateTime.Now;
            var startDate = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 23, 0, 0, 0);
            var dueTime = startDate > currentDate
                        ? startDate - currentDate
                        : startDate.AddDays(1) - currentDate;
            _logger.LogInformation($"Starting at {DateTime.Now}. Time until work {dueTime}");
            _timer = new Timer(DoWork, null, dueTime, TimeSpan.FromDays(1));
            return Task.CompletedTask;
        }

        private void DoWork(object? state)
        {
            _logger.LogInformation($"The service is doing work at {DateTime.Now}");
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var today = DateTime.Today;
                    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    var memberships = context.Memberships.Include(m => m.Member).ToList();

                    foreach (var membership in memberships)
                    {
                        if (membership.EndDate.Date == today.Date)
                        {
                            // Send sms to this member
                        }
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
            }
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"Stopping at {DateTime.Now}");
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose() => _timer?.Dispose();
    }
}
