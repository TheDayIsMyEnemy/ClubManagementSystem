using ClubManagementSystem.Data;
using ClubManagementSystem.Data.Entities;
using ClubManagementSystem.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClubManagementSystem.Services
{
    public class MembershipService : IMembershipService
    {
        private readonly AppDbContext _context;

        public MembershipService(IDbContextFactory<AppDbContext> dbContextFactory)
        {
            _context = dbContextFactory.CreateDbContext();
        }

        public async Task<List<Membership>> GetAllMemberships()
        {
            return await _context
                .Memberships
                .Include(m => m.Member)
                .ToListAsync();
        }
    }
}
