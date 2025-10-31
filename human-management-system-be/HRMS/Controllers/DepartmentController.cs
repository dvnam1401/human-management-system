using HRMS.DTOs;
using HRMS.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

namespace HRMS.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DepartmentController : ControllerBase
{
    private readonly IDepartmentService _departmentService;
    private readonly ILogger<DepartmentController> _logger;

    public DepartmentController(IDepartmentService departmentService, ILogger<DepartmentController> logger)
    {
        _departmentService = departmentService;
        _logger = logger;
    }

    /// <summary>
    /// Get all departments with OData support
    /// Supports: $filter, $orderby, $top, $skip, $count, $select
    /// </summary>
    [HttpGet]
    [EnableQuery(MaxTop = 100, AllowedQueryOptions = AllowedQueryOptions.All)]
    public async Task<ActionResult<IEnumerable<DepartmentDto>>> GetAllDepartments()
    {
        try
        {
            var departments = await _departmentService.GetAllDepartmentsAsync();
            return Ok(departments.AsQueryable());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting all departments");
            return StatusCode(500, new { message = "An error occurred while processing your request." });
        }
    }

    /// <summary>
    /// Get department by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<DepartmentDto>> GetDepartmentById(int id)
    {
        try
        {
            var department = await _departmentService.GetDepartmentByIdAsync(id);
            if (department == null)
            {
                return NotFound(new { message = $"Department with ID {id} not found." });
            }
            return Ok(department);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting department with ID {Id}", id);
            return StatusCode(500, new { message = "An error occurred while processing your request." });
        }
    }

    /// <summary>
    /// Create a new department
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<DepartmentDto>> CreateDepartment([FromBody] CreateDepartmentDto createDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var department = await _departmentService.CreateDepartmentAsync(createDto);
            return CreatedAtAction(nameof(GetDepartmentById), new { id = department.Id }, department);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Invalid operation while creating department");
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating department");
            return StatusCode(500, new { message = "An error occurred while processing your request." });
        }
    }

    /// <summary>
    /// Update department details
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<DepartmentDto>> UpdateDepartment(int id, [FromBody] UpdateDepartmentDto updateDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var department = await _departmentService.UpdateDepartmentAsync(id, updateDto);
            if (department == null)
            {
                return NotFound(new { message = $"Department with ID {id} not found." });
            }

            return Ok(department);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating department with ID {Id}", id);
            return StatusCode(500, new { message = "An error occurred while processing your request." });
        }
    }

    /// <summary>
    /// Set the head/lead of the department
    /// </summary>
    [HttpPut("{id}/lead")]
    public async Task<ActionResult<DepartmentDto>> SetDepartmentLead(int id, [FromBody] SetLeadDto setLeadDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var department = await _departmentService.SetDepartmentLeadAsync(id, setLeadDto);
            if (department == null)
            {
                return NotFound(new { message = $"Department with ID {id} not found." });
            }

            return Ok(department);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while setting department lead for ID {Id}", id);
            return StatusCode(500, new { message = "An error occurred while processing your request." });
        }
    }

    /// <summary>
    /// Delete a department
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteDepartment(int id)
    {
        try
        {
            var result = await _departmentService.DeleteDepartmentAsync(id);
            if (!result)
            {
                return NotFound(new { message = $"Department with ID {id} not found." });
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting department with ID {Id}", id);
            return StatusCode(500, new { message = "An error occurred while processing your request." });
        }
    }
}

