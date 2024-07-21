using static RetailStore.EF.Models.InventoryMagicModel;

namespace InventoryMagic.Models
{
    public class InventoryOptimizationRequest
    {
        public required List<PopularityData> PopularityData { get; set; }
        public required List<ShelfLifeData> ShelfLifeData { get; set; }
        public required List<CurrentInventory> CurrentInventory { get; set; }
    }
}
