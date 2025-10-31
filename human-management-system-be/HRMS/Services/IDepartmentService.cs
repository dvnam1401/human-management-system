using HRMS.DTOs;

namespace HRMS.Services;

public interface IDepartmentService
{
    Task<IEnumerable<DepartmentDto>> GetAllDepartmentsAsync();
    Task<DepartmentDto?> GetDepartmentByIdAsync(int id);
    Task<DepartmentDto> CreateDepartmentAsync(CreateDepartmentDto createDto);
    Task<DepartmentDto?> UpdateDepartmentAsync(int id, UpdateDepartmentDto updateDto);
    Task<DepartmentDto?> SetDepartmentLeadAsync(int id, SetLeadDto setLeadDto);
    Task<bool> DeleteDepartmentAsync(int id);
}

