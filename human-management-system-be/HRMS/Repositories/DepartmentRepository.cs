using HRMS.Model1s;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Repositories;

public class DepartmentRepository : IDepartmentRepository
{
    private readonly HrmsContext _context;

    public DepartmentRepository(HrmsContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Department>> GetAllAsync()
    {
        return await _context.Departments
            .Include(d => d.HeadOfDepartment)
            .Include(d => d.Employees)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Department?> GetByIdAsync(int id)
    {
        return await _context.Departments
            .Include(d => d.HeadOfDepartment)
            .Include(d => d.Employees)
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.Id == id);
    }

    public async Task<Department> CreateAsync(Department department)
    {
        department.CreatedAt = DateTime.Now;
        department.UpdatedAt = DateTime.Now;
        department.IsActive = true;

        _context.Departments.Add(department);
        await _context.SaveChangesAsync();
        return department;
    }

    public async Task<Department?> UpdateAsync(Department department)
    {
        var existingDepartment = await _context.Departments.FindAsync(department.Id);
        if (existingDepartment == null)
            return null;

        existingDepartment.Name = department.Name;
        existingDepartment.Description = department.Description;
        existingDepartment.IsActive = department.IsActive;
        existingDepartment.UpdatedAt = DateTime.Now;

        await _context.SaveChangesAsync();
        return existingDepartment;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var department = await _context.Departments.FindAsync(id);
        if (department == null)
            return false;

        _context.Departments.Remove(department);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsByCodeAsync(string code)
    {
        return await _context.Departments.AnyAsync(d => d.Code == code);
    }

    public async Task<bool> ExistsByNameAsync(string name)
    {
        return await _context.Departments.AnyAsync(d => d.Name == name);
    }

    public async Task<Department?> SetHeadOfDepartmentAsync(int departmentId, int headOfDepartmentId)
    {
        var department = await _context.Departments.FindAsync(departmentId);
        if (department == null)
            return null;

        department.HeadOfDepartmentId = headOfDepartmentId;
        department.UpdatedAt = DateTime.Now;

        await _context.SaveChangesAsync();
        
        return await GetByIdAsync(departmentId);
    }
}

