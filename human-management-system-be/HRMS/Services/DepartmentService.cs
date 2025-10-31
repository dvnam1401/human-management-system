using HRMS.DTOs;
using HRMS.Model1s;
using HRMS.Repositories;

namespace HRMS.Services;

public class DepartmentService : IDepartmentService
{
    private readonly IDepartmentRepository _departmentRepository;

    public DepartmentService(IDepartmentRepository departmentRepository)
    {
        _departmentRepository = departmentRepository;
    }

    public async Task<IEnumerable<DepartmentDto>> GetAllDepartmentsAsync()
    {
        var departments = await _departmentRepository.GetAllAsync();
        return departments.Select(MapToDto);
    }

    public async Task<DepartmentDto?> GetDepartmentByIdAsync(int id)
    {
        var department = await _departmentRepository.GetByIdAsync(id);
        return department == null ? null : MapToDto(department);
    }

    public async Task<DepartmentDto> CreateDepartmentAsync(CreateDepartmentDto createDto)
    {
        // Check if code already exists
        if (await _departmentRepository.ExistsByCodeAsync(createDto.Code))
        {
            throw new InvalidOperationException($"Department with code '{createDto.Code}' already exists.");
        }

        // Check if name already exists
        if (await _departmentRepository.ExistsByNameAsync(createDto.Name))
        {
            throw new InvalidOperationException($"Department with name '{createDto.Name}' already exists.");
        }

        var department = new Department
        {
            Name = createDto.Name,
            Code = createDto.Code,
            Description = createDto.Description,
            HeadOfDepartmentId = createDto.HeadOfDepartmentId
        };

        var createdDepartment = await _departmentRepository.CreateAsync(department);
        return MapToDto(createdDepartment);
    }

    public async Task<DepartmentDto?> UpdateDepartmentAsync(int id, UpdateDepartmentDto updateDto)
    {
        var existingDepartment = await _departmentRepository.GetByIdAsync(id);
        if (existingDepartment == null)
            return null;

        var department = new Department
        {
            Id = id,
            Name = updateDto.Name,
            Description = updateDto.Description,
            IsActive = updateDto.IsActive
        };

        var updatedDepartment = await _departmentRepository.UpdateAsync(department);
        return updatedDepartment == null ? null : MapToDto(updatedDepartment);
    }

    public async Task<DepartmentDto?> SetDepartmentLeadAsync(int id, SetLeadDto setLeadDto)
    {
        var department = await _departmentRepository.SetHeadOfDepartmentAsync(id, setLeadDto.HeadOfDepartmentId);
        return department == null ? null : MapToDto(department);
    }

    public async Task<bool> DeleteDepartmentAsync(int id)
    {
        return await _departmentRepository.DeleteAsync(id);
    }

    private static DepartmentDto MapToDto(Department department)
    {
        return new DepartmentDto
        {
            Id = department.Id,
            Name = department.Name,
            Code = department.Code,
            Description = department.Description,
            HeadOfDepartmentId = department.HeadOfDepartmentId,
            HeadOfDepartmentName = department.HeadOfDepartment != null 
                ? $"{department.HeadOfDepartment.FirstName} {department.HeadOfDepartment.LastName}" 
                : null,
            IsActive = department.IsActive,
            CreatedAt = department.CreatedAt,
            UpdatedAt = department.UpdatedAt,
            EmployeeCount = department.Employees?.Count ?? 0
        };
    }
}

