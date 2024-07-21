using InventoryMagic.Models;
using InventoryMagic.Services;
using Microsoft.AspNetCore.Mvc;
using static RetailStore.EF.Models.InventoryMagicModel;

namespace InventoryMagic.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InventoryMagicController(IInventoryMagicService inventoryService) : ControllerBase
{
    private readonly IInventoryMagicService _inventoryService = inventoryService;

    [HttpPost("OptimizeInventory")]
    public async Task<ActionResult<List<InventoryOptimization>>> OptimizeInventoryAsync([FromBody] InventoryOptimizationRequest request)
    {
        try
        {
            var adjustments = await _inventoryService.OptimizeInventoryAsync(request);

            return Ok(adjustments);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error optimizing inventory: {ex.Message}");
        }
    }
}
