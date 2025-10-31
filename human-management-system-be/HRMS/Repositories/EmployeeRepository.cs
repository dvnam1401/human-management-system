using HRMS.Model1s;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly HrmsContext _context;

    public EmployeeRepository(HrmsContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Employee>> GetAllAsync()
    {
        return await _context.Employees
            .Include(e => e.IdNavigation)
            .Include(e => e.Department)
            .Include(e => e.Manager)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Employee?> GetByIdAsync(int id)
    {
        return await _context.Employees
            .Include(e => e.IdNavigation)
            .Include(e => e.Department)
            .Include(e => e.Manager)
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<IEnumerable<Employee>> GetByDepartmentIdAsync(int departmentId)
    {
        return await _context.Employees
            .Include(e => e.IdNavigation)
            .Include(e => e.Department)
            .Include(e => e.Manager)
            .Where(e => e.DepartmentId == departmentId)
            .AsNoTracking()
            .ToListAsync();
    }
}

