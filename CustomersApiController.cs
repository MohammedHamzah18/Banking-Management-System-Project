using BankingSystem.Domain.DTOs;
using BankingSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.Web.ApiControllers;

[ApiController]
[Route("api/customers")]
[Authorize(Roles = "Admin,Employee")]
public sealed class CustomersApiController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomersApiController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpGet]
    public async Task<IActionResult> Search([FromQuery] string? search, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, CancellationToken cancellationToken = default)
    {
        return Ok(await _customerService.SearchAsync(search, pageNumber, pageSize, cancellationToken));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CustomerDto>> Get(int id, CancellationToken cancellationToken)
    {
        return Ok(await _customerService.GetByIdAsync(id, cancellationToken));
    }

    [HttpPost]
    public async Task<ActionResult<CustomerDto>> Create(CustomerCreateRequest request, CancellationToken cancellationToken)
    {
        var customer = await _customerService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(Get), new { id = customer.Id }, customer);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, CustomerUpdateRequest request, CancellationToken cancellationToken)
    {
        await _customerService.UpdateAsync(id, request, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await _customerService.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}
