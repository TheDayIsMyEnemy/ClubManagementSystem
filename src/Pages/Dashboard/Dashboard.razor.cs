using ClubManagementSystem.Data;
using ClubManagementSystem.Data.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using MudBlazor;

namespace ClubManagementSystem.Pages.Dashboard
{
    public class DashboardBase : ComponentBase
    {
        [Inject]
        private IDbContextFactory<AppDbContext> _dbContextFactory { get; set; } = null!;

        protected double[] DonutData = new double[2];
        protected string[] DonutLabels = { "Active", "Expired" };
        protected double TotalIncome { get; set; }
        protected double NewMembersCount { get; set; }
        protected List<Member> MembersWithRecentBirthday { get; set; } = new();
        protected List<Member> ExpiringMemberships { get; set; } = new();
        protected List<ChartSeries> _series = new List<ChartSeries>()
        {
            new ChartSeries() { Name = "Series 1", Data = new double[] { 90, 79, 72, 69, 62, 62, 55, 65, 70 } },
            new ChartSeries() { Name = "Series 2", Data = new double[] { 10, 41, 35, 51, 49, 62, 69, 91, 148 } },
        };
        protected string[] _xAxisLabels = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep" };
        private Random random = new Random();

        protected void RandomizeData()
        {
            var new_series = new List<ChartSeries>()
            {
                new ChartSeries() { Name = "Series 1", Data = new double[9] },
                new ChartSeries() { Name = "Series 2", Data = new double[9] },
            };
            for (int i = 0; i < 9; i++)
            {
                new_series[0].Data[i] = random.NextDouble() * 100;
                new_series[1].Data[i] = random.NextDouble() * 100;
            }
            _series = new_series;
            StateHasChanged();
        }

        protected override async Task OnInitializedAsync()
        {
            var today = DateTime.Today;
            var activeCount = 0;
            var expiredCount = 0;

            using var context = _dbContextFactory.CreateDbContext();
            var members = await context.Members
                    .Include(m => m.Membership)
                    .Include(m => m.MembershipHistory)
                    .AsNoTracking()
                    .ToListAsync();

            foreach (var member in members)
            {
                if (member.Membership != null)
                {
                    if (member.Membership.IsActive)
                        activeCount++;
                    else
                        expiredCount++;

                    TotalIncome += member.Membership.Fee;
                    TotalIncome += member.MembershipHistory.Sum(h => h.Fee);

                    var daysLeft = (member.Membership.EndDate - today).Days;
                    if (daysLeft >= 0 & daysLeft <= 10)
                        ExpiringMemberships.Add(member);

                    if (member.Created.Date.Year == today.Year
                        && member.Created.Date.Month == today.Month)
                        NewMembersCount++;
                }

                if (member.BirthDate != null
                    && member.BirthDate.Value.Date.Month == today.Month
                    && member.BirthDate.Value.Date.Day >= today.Date.Day)
                {
                    MembersWithRecentBirthday.Add(member);
                }
            }
            DonutData[0] = activeCount;
            DonutData[1] = expiredCount;
        }
    }
}
