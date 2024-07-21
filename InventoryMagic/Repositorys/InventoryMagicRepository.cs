using InventoryMagic.Models;
using RetailStore.EF.Data;
using RetailStore.EF.Models;

namespace InventoryMagic.Repositorys
{
    public class InventoryMagicRepository(AppDbContext context) : IInventoryMagicRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<List<InventoryMagicModel.InventoryOptimization>> OptimizeInventoryAsync(string popularityDatasJson, string shelfLifeDatasJson, string currentInventorysJson)
        {
            return await _context.OptimizeInventoryPlanAsync(popularityDatasJson, shelfLifeDatasJson, currentInventorysJson);
        }
    }
}
