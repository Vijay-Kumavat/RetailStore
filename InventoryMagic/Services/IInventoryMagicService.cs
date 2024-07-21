using InventoryMagic.Models;
using static RetailStore.EF.Models.InventoryMagicModel;

namespace InventoryMagic.Services
{
    public interface IInventoryMagicService
    {
        Task<List<InventoryOptimization>> OptimizeInventoryAsync(InventoryOptimizationRequest request);
    }
}
