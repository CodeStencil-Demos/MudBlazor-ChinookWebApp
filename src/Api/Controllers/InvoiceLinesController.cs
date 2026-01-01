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
    public class InvoiceLinesController(IInvoiceLineService invoicelineService, IMapper mapper) : ControllerBase
    {
        private readonly IMapper _mapper = mapper;

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<InvoiceLineDto>>> GetAll()
        {
            var invoicelines = await invoicelineService.GetAllAsync();
            return Ok(invoicelines);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<InvoiceLineDto>> GetById(int id)
        {
            var invoiceline = await invoicelineService.GetByIdAsync(id);
            if (invoiceline == null)
            {
                return NotFound();
            }
            return Ok(invoiceline);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<InvoiceLineDto>> Create(InvoiceLineDto invoicelineDto)
        {
            var createdInvoiceLine = await invoicelineService.CreateAsync(invoicelineDto);
            return CreatedAtAction(nameof(GetById), new { id = createdInvoiceLine.InvoiceLineId }, createdInvoiceLine);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<InvoiceLineDto>> Update(int id, InvoiceLineDto invoicelineDto)
        {
            if (id != invoicelineDto.InvoiceLineId)
            {
                return BadRequest();
            }
            var updatedInvoiceLine = await invoicelineService.UpdateAsync(invoicelineDto);
            return Ok(updatedInvoiceLine);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> Delete(int id)
        {
            await invoicelineService.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("paged")]
        [Authorize]
        public async Task<ActionResult<PaginatedResult<InvoiceLineDto>>> GetPaged([FromQuery] QueryParameters parameters)
        {
            var pagedInvoiceLines = await invoicelineService.GetPagedAsync(parameters);
            return Ok(pagedInvoiceLines);
        }

    }
}

