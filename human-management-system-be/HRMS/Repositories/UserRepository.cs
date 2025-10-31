using HRMS.Model1s;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Repositories;

public class UserRepository : IUserRepository
{
    private readonly HrmsContext _context;

    public UserRepository(HrmsContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByUsernameOrEmailAsync(string identifier)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Username == identifier || u.Email == identifier);
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<bool> UpdatePasswordAsync(int userId, string passwordHash)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null)
            return false;

        user.PasswordHash = passwordHash;
        user.UpdatedAt = DateTime.Now;
        
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<User?> GetUserWithEmployeeDetailsAsync(int userId)
    {
        return await _context.Users
            .Include(u => u.EmployeeIdNavigation)
                .ThenInclude(e => e!.Department)
            .FirstOrDefaultAsync(u => u.Id == userId);
    }
}

