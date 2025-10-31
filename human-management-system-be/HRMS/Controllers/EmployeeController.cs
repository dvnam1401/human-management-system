using HRMS.DTOs;
using HRMS.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

namespace HRMS.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeService _employeeService;
    private readonly ILogger<EmployeeController> _logger;

    public EmployeeController(IEmployeeService employeeService, ILogger<EmployeeController> logger)
    {
        _employeeService = employeeService;
        _logger = logger;
    }

    /// <summary>
    /// Get all employees with OData support
    /// Supports: $filter, $orderby, $top, $skip, $count, $select
    /// </summary>
    [HttpGet]
    [EnableQuery(MaxTop = 100, AllowedQueryOptions = AllowedQueryOptions.All)]
    public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetAllEmployees()
    {
        try
        {
            var employees = await _employeeService.GetAllEmployeesAsync();
            return Ok(employees.AsQueryable());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting all employees");
            return StatusCode(500, new { message = "An error occurred while processing your request." });
        }
    }

    /// <summary>
    /// Get employee by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<EmployeeDto>> GetEmployeeById(int id)
    {
        try
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            if (employee == null)
            {
                return NotFound(new { message = $"Employee with ID {id} not found." });
            }
            return Ok(employee);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting employee with ID {Id}", id);
            return StatusCode(500, new { message = "An error occurred while processing your request." });
        }
    }

    /// <summary>
    /// Get employees by department ID with OData support
    /// Supports: $filter, $orderby, $top, $skip, $count, $select
    /// </summary>
    [HttpGet("department/{departmentId}")]
    [EnableQuery(MaxTop = 100, AllowedQueryOptions = AllowedQueryOptions.All)]
    public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetEmployeesByDepartment(int departmentId)
    {
        try
        {
            var employees = await _employeeService.GetEmployeesByDepartmentIdAsync(departmentId);
            return Ok(employees.AsQueryable());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting employees for department {DepartmentId}", departmentId);
            return StatusCode(500, new { message = "An error occurred while processing your request." });
        }
    }
}

