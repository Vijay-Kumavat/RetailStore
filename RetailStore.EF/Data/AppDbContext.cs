using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RetailStore.EF.Models;
using static RetailStore.EF.Models.CleverPricingTrickModel;
using static RetailStore.EF.Models.InventoryMagicModel;
using static RetailStore.EF.Models.SmartRestockingPlanModel;

namespace RetailStore.EF.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<ProductStock> ProductStocks { get; set; }
        public DbSet<UpdatedPrice> UpdatedPrices { get; set; }
        public DbSet<InventoryOptimization> InventoryOptimization { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductStock>().HasNoKey();
            modelBuilder.Entity<UpdatedPrice>().HasNoKey();
            modelBuilder.Entity<InventoryOptimization>().HasNoKey();
            base.OnModelCreating(modelBuilder);
        }

        public async Task<List<ProductStock>> CalculateRestockPlanAsync(List<SaleTransaction> salesData)
        {
            var salesDataParam = new SqlParameter("@salesData", System.Data.SqlDbType.NVarChar)
            {
                Value = JsonConvert.SerializeObject(salesData)
            };

            return await Set<ProductStock>()
                .FromSqlRaw("EXEC CalculateRestockPlan @salesData", salesDataParam)
                .ToListAsync();
        }

        public async Task<List<UpdatedPrice>> UpdatedPricesPlanAsync(string competitorPricesJson, string demandTrendsJson)
        {
            var competitorPricesParam = new SqlParameter("@CompetitorPrices", competitorPricesJson);
            var demandTrendsParam = new SqlParameter("@DemandTrends", demandTrendsJson);

            return await Set<UpdatedPrice>()
                .FromSqlRaw("EXEC UpdatedPrices @CompetitorPrices, @DemandTrends", competitorPricesParam, demandTrendsParam)
                .ToListAsync();
        }

        public async Task<List<InventoryOptimization>> OptimizeInventoryPlanAsync(string popularityDatasJson, string shelfLifeDatasJson, string currentInventorysJson)
        {
            var popularityParam = new SqlParameter("@PopularityData", System.Data.SqlDbType.NVarChar) { Value = popularityDatasJson };
            var shelfLifeParam = new SqlParameter("@ShelfLifeData", System.Data.SqlDbType.NVarChar) { Value = shelfLifeDatasJson };
            var currentInventoryParam = new SqlParameter("@CurrentInventory", System.Data.SqlDbType.NVarChar) { Value = currentInventorysJson };

            var query = "EXEC OptimizeInventory @PopularityData, @ShelfLifeData, @CurrentInventory";

            return await Set<InventoryOptimization>()
                                        .FromSqlRaw(query, popularityParam, shelfLifeParam, currentInventoryParam)
                                        .ToListAsync();
        }
    }
}
