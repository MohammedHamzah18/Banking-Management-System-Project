using BankingSystem.Domain.DTOs;
using BankingSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.Web.ApiControllers;

[ApiController]
[Route("api/accounts")]
[Authorize]
public sealed class AccountsApiController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountsApiController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<AccountDto>> Get(int id, CancellationToken cancellationToken) => Ok(await _accountService.GetByIdAsync(id, cancellationToken));

    [HttpGet("customer/{customerId:int}")]
    public async Task<IActionResult> ByCustomer(int customerId, CancellationToken cancellationToken) => Ok(await _accountService.GetByCustomerAsync(customerId, cancellationToken));

    [HttpPost]
    [Authorize(Roles = "Admin,Employee")]
    public async Task<ActionResult<AccountDto>> Create(CreateAccountRequest request, CancellationToken cancellationToken)
    {
        var account = await _accountService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(Get), new { id = account.Id }, account);
    }

    [HttpPatch("{id:int}/status")]
    [Authorize(Roles = "Admin,Employee")]
    public async Task<IActionResult> SetStatus(int id, [FromQuery] bool activate, CancellationToken cancellationToken)
    {
        await _accountService.SetStatusAsync(id, activate, cancellationToken);
        return NoContent();
    }
}
