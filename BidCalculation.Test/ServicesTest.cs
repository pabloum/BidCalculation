using BidCalculation.Business;
using BidCalculation.Business.Implementations;
using BidCalculation.Domain;
using BidCalculation.Domain.DTO;
using BidCalculation.Domain.Enums;

namespace BidCalculation.Test;

public class ServicesTest
{
    private readonly IVehiclePriceService _vehiclePriceService;

    public ServicesTest()
    {
        _vehiclePriceService = new VehiclePriceService();
    }

    [Theory]
    [InlineData(1000,    VehicleType.COMMON, 50, 20, 10, 100, 1180)]
    [InlineData(398,     VehicleType.COMMON, 39.80, 7.96, 5.00, 100.00, 550.76)]
    [InlineData(501,     VehicleType.COMMON, 50.00, 10.02, 10.00, 100.00, 671.02)]
    [InlineData(57,      VehicleType.COMMON, 10.00, 1.14, 5.00, 100.00, 173.14)]
    [InlineData(1800,    VehicleType.LUXURY, 180.00, 72.00, 15.00, 100.00, 2167.00)]
    [InlineData(1100,    VehicleType.COMMON, 50.00, 22.00, 15.00, 100.00, 1287.00)]
    [InlineData(1000000, VehicleType.LUXURY, 200.00, 40000.00, 20.00, 100.00, 1040320.00)]
    public void Test1(
        int basePrice, VehicleType vehicleTypeType,
        double expectedBasicFeeCost,
        double expectedSpecialFeeCost,
        double expectedAssociationFeeCost,
        double expectedStorageFeeCost,
        double expectedTotalCost)
    {
        // Arrange
        var expectedResult = new VehiclePrice
        {
            BasePrice = basePrice,
            TotalPrice = expectedTotalCost,
            Fees = new List<Fee> 
            {
                new Fee { Type = FeeType.BASIC, Cost = expectedBasicFeeCost},
                new Fee { Type = FeeType.SPECIAL, Cost = expectedSpecialFeeCost},
                new Fee { Type = FeeType.ASSOCIATION, Cost = expectedAssociationFeeCost},
                new Fee { Type = FeeType.STORAGE, Cost = expectedStorageFeeCost},
            }
        };

        var request = new RequestVehiclePrice
        {
            BasePrice = basePrice,
            VehicleType = vehicleTypeType
        };

        // Act 
        var result = _vehiclePriceService.GetTotalPrice(request);

        // Assert
        Assert.Equal(expectedResult.BasePrice, result.BasePrice);
        Assert.Equal(expectedResult.Fees.Single(f => f.Type == FeeType.BASIC).Cost, result.Fees.Single(f => f.Type == FeeType.BASIC).Cost);
        Assert.Equal(expectedResult.Fees.Single(f => f.Type == FeeType.SPECIAL).Cost, result.Fees.Single(f => f.Type == FeeType.SPECIAL).Cost);
        Assert.Equal(expectedResult.Fees.Single(f => f.Type == FeeType.ASSOCIATION).Cost, result.Fees.Single(f => f.Type == FeeType.ASSOCIATION).Cost);
        Assert.Equal(expectedResult.Fees.Single(f => f.Type == FeeType.STORAGE).Cost, result.Fees.Single(f => f.Type == FeeType.STORAGE).Cost);
        Assert.Equal(expectedResult.TotalPrice, result.TotalPrice);
    }
}