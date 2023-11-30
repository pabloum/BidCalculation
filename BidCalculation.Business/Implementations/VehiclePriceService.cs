using System.Collections.Concurrent;
using BidCalculation.Business.PriceCalculator;
using BidCalculation.Domain;
using BidCalculation.Domain.DTO;
using BidCalculation.Domain.Enums;

namespace BidCalculation.Business.Implementations;

public class VehiclePriceService : IVehiclePriceService
{
    private readonly IFeeCalculator _feeCalculator;
    
    public VehiclePriceService(IFeeCalculator feeCalculator)
    {
        _feeCalculator = feeCalculator;
    }

    public VehiclePrice GetTotalPrice(RequestVehiclePrice requsetVehiclePrice)
    {
        var fees = _feeCalculator.GetFees(requsetVehiclePrice.BasePrice, requsetVehiclePrice.VehicleType);

        return new VehiclePrice
        {
            BasePrice = requsetVehiclePrice.BasePrice,
            Fees = fees,
            TotalPrice = Math.Round(requsetVehiclePrice.BasePrice + fees.Select(f => f.Cost).Sum(), 2) 
        };
    }
}