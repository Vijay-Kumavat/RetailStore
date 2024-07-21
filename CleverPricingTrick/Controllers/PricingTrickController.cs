using CleverPricingTrick.Models;
using CleverPricingTrick.Services;
using Microsoft.AspNetCore.Mvc;
using static RetailStore.EF.Models.CleverPricingTrickModel;

namespace CleverPricingTrick.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PricingTrickController(IPricingService pricingService) : ControllerBase
{
    private readonly IPricingService _pricingService = pricingService;

    [HttpPost("UpdatedPrices")]
    public async Task<ActionResult<List<UpdatedPrice>>> UpdatedPricesAsync(
        [FromBody] PricingRequest request)
    {
        var updatedPrices = await _pricingService.UpdatedPricesAsync(request.CompetitorPrices, request.DemandTrends);
        return Ok(updatedPrices);
    }
}
