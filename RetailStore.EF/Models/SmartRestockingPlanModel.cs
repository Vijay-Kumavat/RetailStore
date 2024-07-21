using System.ComponentModel.DataAnnotations;

namespace RetailStore.EF.Models;

public class SmartRestockingPlanModel
{
    public class ProductStock
    {
        public required string ProductID { get; set; }
        public int RecommendedQuantity { get; set; }
    }

    public class SaleTransaction
    {
        public required string ProductID { get; set; }
        public required int QuantitySold { get; set; }
        public required DateTime Timestamp { get; set; }
    }
}
