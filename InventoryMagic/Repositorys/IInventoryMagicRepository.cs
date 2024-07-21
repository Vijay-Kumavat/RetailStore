using InventoryMagic.Models;
using static RetailStore.EF.Models.InventoryMagicModel;

namespace InventoryMagic.Repositorys
{
    public interface IInventoryMagicRepository
    {
        Task<List<InventoryOptimization>> OptimizeInventoryAsync(string popularityDatasJson, string shelfLifeDatasJson, string currentInventorysJson);
    }
}
