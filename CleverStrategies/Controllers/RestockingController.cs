using Microsoft.AspNetCore.Mvc;
using SmartRestockingPlan.Services;
using static RetailStore.EF.Models.SmartRestockingPlanModel;

namespace SmartRestockingPlan.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RestockingController(IRestockingService restockingService) : ControllerBase
{
    private readonly IRestockingService _restockingService = restockingService;

    [HttpPost("Calculate")]
    public async Task<ActionResult<List<ProductStock>>> CalculateRestocking([FromBody] List<SaleTransaction> salesData)
    {
        var recommendations = await _restockingService.CalculateRestockingAsync(salesData);
        return Ok(recommendations);
    }
}
