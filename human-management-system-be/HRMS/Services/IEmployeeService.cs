using HRMS.DTOs;

namespace HRMS.Services;

public interface IEmployeeService
{
    Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync();
    Task<EmployeeDto?> GetEmployeeByIdAsync(int id);
    Task<IEnumerable<EmployeeDto>> GetEmployeesByDepartmentIdAsync(int departmentId);
}

