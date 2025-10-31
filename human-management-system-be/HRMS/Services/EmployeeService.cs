using HRMS.DTOs;
using HRMS.Model1s;
using HRMS.Repositories;

namespace HRMS.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;

    public EmployeeService(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public async Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync()
    {
        var employees = await _employeeRepository.GetAllAsync();
        return employees.Select(MapToDto);
    }

    public async Task<EmployeeDto?> GetEmployeeByIdAsync(int id)
    {
        var employee = await _employeeRepository.GetByIdAsync(id);
        return employee == null ? null : MapToDto(employee);
    }

    public async Task<IEnumerable<EmployeeDto>> GetEmployeesByDepartmentIdAsync(int departmentId)
    {
        var employees = await _employeeRepository.GetByDepartmentIdAsync(departmentId);
        return employees.Select(MapToDto);
    }

    private static EmployeeDto MapToDto(Employee employee)
    {
        return new EmployeeDto
        {
            Id = employee.Id,
            FirstName = employee.IdNavigation?.FirstName,
            LastName = employee.IdNavigation?.LastName,
            Email = employee.IdNavigation?.Email,
            PhoneNumber = employee.IdNavigation?.PhoneNumber,
            DepartmentId = employee.DepartmentId,
            DepartmentName = employee.Department?.Name,
            HireDate = employee.HireDate,
            TerminationDate = employee.TerminationDate,
            Salary = employee.Salary,
            ManagerId = employee.ManagerId,
            ManagerName = employee.Manager != null 
                ? $"{employee.Manager.FirstName} {employee.Manager.LastName}" 
                : null,
            Address = employee.Address,
            IsActive = employee.IdNavigation?.IsActive
        };
    }
}

