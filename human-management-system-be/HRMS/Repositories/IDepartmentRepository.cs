using HRMS.Model1s;

namespace HRMS.Repositories;

public interface IDepartmentRepository
{
    Task<IEnumerable<Department>> GetAllAsync();
    Task<Department?> GetByIdAsync(int id);
    Task<Department> CreateAsync(Department department);
    Task<Department?> UpdateAsync(Department department);
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistsByCodeAsync(string code);
    Task<bool> ExistsByNameAsync(string name);
    Task<Department?> SetHeadOfDepartmentAsync(int departmentId, int headOfDepartmentId);
}

