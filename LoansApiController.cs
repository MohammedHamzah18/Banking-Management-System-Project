using BankingSystem.Domain.DTOs;
using BankingSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.Web.ApiControllers;

[ApiController]
[Route("api/loans")]
[Authorize]
public sealed class LoansApiController : ControllerBase
{
    private readonly ILoanService _loanService;

    public LoansApiController(ILoanService loanService)
    {
        _loanService = loanService;
    }

    [HttpPost]
    public async Task<ActionResult<LoanDto>> Apply(LoanApplyRequest request, CancellationToken cancellationToken) => Ok(await _loanService.ApplyAsync(request, cancellationToken));

    [HttpPatch("{id:int}/decision")]
    [Authorize(Roles = "Admin,Employee")]
    public async Task<IActionResult> Decide(int id, LoanDecisionRequest request, CancellationToken cancellationToken)
    {
        await _loanService.DecideAsync(id, request, cancellationToken);
        return NoContent();
    }
}
