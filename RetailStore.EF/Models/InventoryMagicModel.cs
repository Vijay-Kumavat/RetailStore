namespace RetailStore.EF.Models;

public class InventoryMagicModel
{
    public class PopularityData
    {
        public required string ProductID { get; set; }
        public required double PopularityScore { get; set; }
    }

    public class ShelfLifeData
    {
        public required string ProductID { get; set; }
        public required int ShelfLife { get; set; }
    }

    public class CurrentInventory
    {
        public required string ProductID { get; set; }
        public required int CurrentStock { get; set; }
    }

    public class InventoryOptimization
    {
        public required string ProductID { get; set; }
        public required int RecommendedAdjustment { get; set; }
    }
}
