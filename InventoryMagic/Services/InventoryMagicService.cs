using InventoryMagic.Models;
using InventoryMagic.Repositorys;
using Newtonsoft.Json;
using RetailStore.EF.Models;
using static RetailStore.EF.Models.CleverPricingTrickModel;

namespace InventoryMagic.Services
{
    public class InventoryMagicService(IInventoryMagicRepository inventoryMagicRepository) : IInventoryMagicService
    {
        private readonly IInventoryMagicRepository _inventoryMagicRepository = inventoryMagicRepository;

        public async Task<List<InventoryMagicModel.InventoryOptimization>> OptimizeInventoryAsync(InventoryOptimizationRequest request)
        {
            var popularityDatasJson = JsonConvert.SerializeObject(request.PopularityData);
            var shelfLifeDatasJson = JsonConvert.SerializeObject(request.ShelfLifeData);
            var currentInventorysJson = JsonConvert.SerializeObject(request.CurrentInventory);

            return await _inventoryMagicRepository.OptimizeInventoryAsync(popularityDatasJson, shelfLifeDatasJson, currentInventorysJson);
        }
    }
}
