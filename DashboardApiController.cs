using BankingSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.Web.ApiControllers;

[ApiController]
[Route("api/dashboard")]
[Authorize(Roles = "Admin,Employee")]
public sealed class DashboardApiController : ControllerBase
{
    private readonly IDashboardService _dashboardService;

    public DashboardApiController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken) => Ok(await _dashboardService.GetDashboardAsync(cancellationToken));
}
