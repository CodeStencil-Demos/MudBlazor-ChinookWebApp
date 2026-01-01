using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Application.Common.Models;
using Application.DTOs;
using Application.Services;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController(IEmployeeService employeeService, IMapper mapper) : ControllerBase
    {
        private readonly IMapper _mapper = mapper;

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetAll()
        {
            var employees = await employeeService.GetAllAsync();
            return Ok(employees);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<EmployeeDto>> GetById(int id)
        {
            var employee = await employeeService.GetByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return Ok(employee);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<EmployeeDto>> Create(EmployeeDto employeeDto)
        {
            var createdEmployee = await employeeService.CreateAsync(employeeDto);
            return CreatedAtAction(nameof(GetById), new { id = createdEmployee.EmployeeId }, createdEmployee);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<EmployeeDto>> Update(int id, EmployeeDto employeeDto)
        {
            if (id != employeeDto.EmployeeId)
            {
                return BadRequest();
            }
            var updatedEmployee = await employeeService.UpdateAsync(employeeDto);
            return Ok(updatedEmployee);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> Delete(int id)
        {
            await employeeService.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("paged")]
        [Authorize]
        public async Task<ActionResult<PaginatedResult<EmployeeDto>>> GetPaged([FromQuery] QueryParameters parameters)
        {
            var pagedEmployees = await employeeService.GetPagedAsync(parameters);
            return Ok(pagedEmployees);
        }

    }
}

