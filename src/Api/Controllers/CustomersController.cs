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
    public class CustomersController(ICustomerService customerService, IMapper mapper) : ControllerBase
    {
        private readonly IMapper _mapper = mapper;

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<CustomerDto>>> GetAll()
        {
            var customers = await customerService.GetAllAsync();
            return Ok(customers);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<CustomerDto>> GetById(int id)
        {
            var customer = await customerService.GetByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return Ok(customer);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<CustomerDto>> Create(CustomerDto customerDto)
        {
            var createdCustomer = await customerService.CreateAsync(customerDto);
            return CreatedAtAction(nameof(GetById), new { id = createdCustomer.CustomerId }, createdCustomer);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<CustomerDto>> Update(int id, CustomerDto customerDto)
        {
            if (id != customerDto.CustomerId)
            {
                return BadRequest();
            }
            var updatedCustomer = await customerService.UpdateAsync(customerDto);
            return Ok(updatedCustomer);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> Delete(int id)
        {
            await customerService.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("paged")]
        [Authorize]
        public async Task<ActionResult<PaginatedResult<CustomerDto>>> GetPaged([FromQuery] QueryParameters parameters)
        {
            var pagedCustomers = await customerService.GetPagedAsync(parameters);
            return Ok(pagedCustomers);
        }

    }
}

