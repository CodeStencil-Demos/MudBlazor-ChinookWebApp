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
    public class InvoicesController(IInvoiceService invoiceService, IMapper mapper) : ControllerBase
    {
        private readonly IMapper _mapper = mapper;

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<InvoiceDto>>> GetAll()
        {
            var invoices = await invoiceService.GetAllAsync();
            return Ok(invoices);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<InvoiceDto>> GetById(int id)
        {
            var invoice = await invoiceService.GetByIdAsync(id);
            if (invoice == null)
            {
                return NotFound();
            }
            return Ok(invoice);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<InvoiceDto>> Create(InvoiceDto invoiceDto)
        {
            var createdInvoice = await invoiceService.CreateAsync(invoiceDto);
            return CreatedAtAction(nameof(GetById), new { id = createdInvoice.InvoiceId }, createdInvoice);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<InvoiceDto>> Update(int id, InvoiceDto invoiceDto)
        {
            if (id != invoiceDto.InvoiceId)
            {
                return BadRequest();
            }
            var updatedInvoice = await invoiceService.UpdateAsync(invoiceDto);
            return Ok(updatedInvoice);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> Delete(int id)
        {
            await invoiceService.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("paged")]
        [Authorize]
        public async Task<ActionResult<PaginatedResult<InvoiceDto>>> GetPaged([FromQuery] QueryParameters parameters)
        {
            var pagedInvoices = await invoiceService.GetPagedAsync(parameters);
            return Ok(pagedInvoices);
        }

    }
}

