using Microsoft.EntityFrameworkCore;
using MysteryGuestAPI.Contexts;
using MysteryGuestAPI.DbContext;

namespace MysteryGuestAPI.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _dbContext;

    public UserRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<ApplicationUser?> FindUserByEmailAsync(string email)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == email);
    }
}

public interface IUserRepository
{
    Task<ApplicationUser?> FindUserByEmailAsync(string email);
}