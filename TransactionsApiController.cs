using BankingSystem.Domain.DTOs;
using BankingSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.Web.ApiControllers;

[ApiController]
[Route("api/transactions")]
[Authorize]
public sealed class TransactionsApiController : ControllerBase
{
    private readonly ITransactionService _transactionService;

    public TransactionsApiController(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    [HttpPost("deposit")]
    public async Task<ActionResult<TransactionDto>> Deposit(MoneyRequest request, CancellationToken cancellationToken) => Ok(await _transactionService.DepositAsync(request, cancellationToken));

    [HttpPost("withdraw")]
    public async Task<ActionResult<TransactionDto>> Withdraw(MoneyRequest request, CancellationToken cancellationToken) => Ok(await _transactionService.WithdrawAsync(request, cancellationToken));

    [HttpPost("transfer")]
    public async Task<ActionResult<TransactionDto>> Transfer(TransferRequest request, CancellationToken cancellationToken) => Ok(await _transactionService.TransferAsync(request, cancellationToken));

    [HttpGet("account/{accountId:int}")]
    public async Task<IActionResult> History(int accountId, CancellationToken cancellationToken) => Ok(await _transactionService.HistoryAsync(accountId, 50, cancellationToken));

    [HttpGet("account/{accountId:int}/mini-statement")]
    public async Task<IActionResult> MiniStatement(int accountId, CancellationToken cancellationToken) => Ok(await _transactionService.MiniStatementAsync(accountId, cancellationToken));
}
