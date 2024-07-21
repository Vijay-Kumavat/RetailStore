USE [master]
GO
/****** Object:  Database [RetailStore]    Script Date: 14-07-2024 19:39:40 ******/
CREATE DATABASE [RetailStore]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'RetailStore', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\RetailStore.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'RetailStore_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\RetailStore_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [RetailStore] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [RetailStore].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [RetailStore] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [RetailStore] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [RetailStore] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [RetailStore] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [RetailStore] SET ARITHABORT OFF 
GO
ALTER DATABASE [RetailStore] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [RetailStore] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [RetailStore] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [RetailStore] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [RetailStore] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [RetailStore] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [RetailStore] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [RetailStore] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [RetailStore] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [RetailStore] SET  DISABLE_BROKER 
GO
ALTER DATABASE [RetailStore] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [RetailStore] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [RetailStore] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [RetailStore] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [RetailStore] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [RetailStore] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [RetailStore] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [RetailStore] SET RECOVERY FULL 
GO
ALTER DATABASE [RetailStore] SET  MULTI_USER 
GO
ALTER DATABASE [RetailStore] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [RetailStore] SET DB_CHAINING OFF 
GO
ALTER DATABASE [RetailStore] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [RetailStore] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [RetailStore] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [RetailStore] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'RetailStore', N'ON'
GO
ALTER DATABASE [RetailStore] SET QUERY_STORE = OFF
GO
USE [RetailStore]
GO
/****** Object:  StoredProcedure [dbo].[CalculateRestockPlan]    Script Date: 14-07-2024 19:39:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CalculateRestockPlan]
    @salesData NVARCHAR(MAX)  -- JSON array containing past sales transactions
AS
BEGIN
    -- Create a temporary table to hold parsed sales data
    CREATE TABLE #TempSales (
        ProductID VARCHAR(50),
        QuantitySold INT,
        SaleDate DATE
    );

    -- Insert sales data from JSON array into the temporary table
    INSERT INTO #TempSales (ProductID, QuantitySold, SaleDate)
    SELECT 
        j.ProductID,
        j.QuantitySold,
        CONVERT(DATE, j.Timestamp) AS SaleDate
    FROM 
        OPENJSON(@salesData) WITH (
            ProductID VARCHAR(50),
            QuantitySold INT,
            Timestamp DATETIME
        ) AS j;

    -- Temporary table to calculate average daily sales per product
    WITH DailySales AS (
        SELECT 
            ProductID,
            SUM(QuantitySold) AS TotalQuantitySold,
            COUNT(DISTINCT SaleDate) AS SalesDays
        FROM 
            #TempSales
        GROUP BY 
            ProductID
    )

    -- Calculate restock recommendations based on provided logic
    SELECT 
        ds.ProductID,
        CEILING((ds.TotalQuantitySold / ds.SalesDays) * (COUNT(#TempSales.ProductID))) AS RecommendedQuantity
    FROM 
        DailySales ds
    JOIN 
        #TempSales ON ds.ProductID = #TempSales.ProductID
    GROUP BY 
        ds.ProductID, ds.TotalQuantitySold, ds.SalesDays;

    -- Drop the temporary table after use
    DROP TABLE #TempSales;
END
GO
/****** Object:  StoredProcedure [dbo].[OptimizeInventory]    Script Date: 14-07-2024 19:39:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[OptimizeInventory]
    @PopularityData NVARCHAR(MAX),
    @ShelfLifeData NVARCHAR(MAX),
    @CurrentInventory NVARCHAR(MAX)
AS
BEGIN
    -- Temporary tables to hold the JSON data
    DECLARE @TempPopularityData TABLE (
        ProductID VARCHAR(50),
        PopularityScore FLOAT
    );

    DECLARE @TempShelfLifeData TABLE (
        ProductID VARCHAR(50),
        ShelfLife INT
    );

    DECLARE @TempCurrentInventory TABLE (
        ProductID VARCHAR(50),
        CurrentStock INT
    );

    -- Insert the JSON data into the temporary tables
    INSERT INTO @TempPopularityData (ProductID, PopularityScore)
    SELECT 
        JSON_VALUE(value, '$.ProductID'),
        CAST(JSON_VALUE(value, '$.PopularityScore') AS FLOAT)
    FROM 
        OPENJSON(@PopularityData);

    INSERT INTO @TempShelfLifeData (ProductID, ShelfLife)
    SELECT 
        JSON_VALUE(value, '$.ProductID'),
        CAST(JSON_VALUE(value, '$.ShelfLife') AS INT)
    FROM 
        OPENJSON(@ShelfLifeData);

    INSERT INTO @TempCurrentInventory (ProductID, CurrentStock)
    SELECT 
        JSON_VALUE(value, '$.ProductID'),
        CAST(JSON_VALUE(value, '$.CurrentStock') AS INT)
    FROM 
        OPENJSON(@CurrentInventory);

    -- Calculate the inventory adjustments
    SELECT 
        ci.ProductID,
        CAST(
            (ci.CurrentStock * (pd.PopularityScore - 1)) +
            CASE 
                WHEN sd.ShelfLife > 30 THEN 10
                WHEN sd.ShelfLife < 30 THEN -10
                ELSE 0
            END 
            AS INT
        ) AS RecommendedAdjustment
    INTO 
        #TempInventoryAdjustments
    FROM 
        @TempCurrentInventory ci
    JOIN 
        @TempPopularityData pd ON ci.ProductID = pd.ProductID
    JOIN 
        @TempShelfLifeData sd ON ci.ProductID = sd.ProductID;

    -- Select the results
    SELECT 
        ProductID, RecommendedAdjustment
    FROM 
        #TempInventoryAdjustments;

    -- Cleanup
    DROP TABLE #TempInventoryAdjustments;
END
GO
/****** Object:  StoredProcedure [dbo].[UpdatedPrices]    Script Date: 14-07-2024 19:39:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UpdatedPrices]
    @CompetitorPrices NVARCHAR(MAX),
    @DemandTrends NVARCHAR(MAX)
AS
BEGIN
    -- Temporary tables to hold the JSON data
    DECLARE @TempCompetitorPrices TABLE (
        ProductID VARCHAR(50),
        Price DECIMAL(18, 2)
    );

    DECLARE @TempDemandTrends TABLE (
        ProductID VARCHAR(50),
        Trend VARCHAR(50)
    );

    -- Insert the JSON data into the temporary tables
    INSERT INTO @TempCompetitorPrices (ProductID, Price)
    SELECT 
        JSON_VALUE(value, '$.ProductID'),
        CAST(JSON_VALUE(value, '$.Price') AS DECIMAL(18, 2))
    FROM 
        OPENJSON(@CompetitorPrices);

    INSERT INTO @TempDemandTrends (ProductID, Trend)
    SELECT 
        JSON_VALUE(value, '$.ProductID'),
        JSON_VALUE(value, '$.Trend')
    FROM 
        OPENJSON(@DemandTrends);

    -- Calculate the updated prices
    SELECT 
        cp.ProductID,
        ROUND(
            CASE 
                WHEN dt.Trend = 'increasing' THEN cp.Price * 1.10
                WHEN dt.Trend = 'decreasing' THEN cp.Price * 0.90
                ELSE cp.Price
            END, 2) AS Price
    INTO 
        #TempUpdatedPrices
    FROM 
        @TempCompetitorPrices cp
    JOIN 
        @TempDemandTrends dt ON cp.ProductID = dt.ProductID;

    -- Return the final results
    SELECT 
        ProductID, Price
    FROM 
        #TempUpdatedPrices;

    -- Cleanup
    DROP TABLE #TempUpdatedPrices;
END
GO
USE [master]
GO
ALTER DATABASE [RetailStore] SET  READ_WRITE 
GO
