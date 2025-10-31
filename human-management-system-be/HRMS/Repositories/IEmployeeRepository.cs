using HRMS.Model1s;

namespace HRMS.Repositories;

public interface IEmployeeRepository
{
    Task<IEnumerable<Employee>> GetAllAsync();
    Task<Employee?> GetByIdAsync(int id);
    Task<IEnumerable<Employee>> GetByDepartmentIdAsync(int departmentId);
}

