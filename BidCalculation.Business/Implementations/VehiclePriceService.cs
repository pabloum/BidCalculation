using System.Collections.Concurrent;
using BidCalculation.Domain;
using BidCalculation.Domain.DTO;
using BidCalculation.Domain.Enums;

namespace BidCalculation.Business.Implementations;

public class VehiclePriceService : IVehiclePriceService
{
    public VehiclePriceService()
    {
        
    }

    public VehiclePrice GetTotalPrice(RequestVehiclePrice requsetVehiclePrice)
    {
        var fees = CalculateFees(requsetVehiclePrice);

        return new VehiclePrice
        {
            BasePrice = requsetVehiclePrice.BasePrice,
            Fees = fees,
            TotalPrice = Math.Round(requsetVehiclePrice.BasePrice + fees.Select(f => f.Cost).Sum(), 2) 
        };
    }

    private IEnumerable<Fee> CalculateFees(RequestVehiclePrice requsetVehiclePrice)
    {
        var vehicleType = requsetVehiclePrice.VehicleType;
        var basePrice = requsetVehiclePrice.BasePrice;

        var fees = new ConcurrentBag<Fee>();

        Parallel.Invoke(
            () => fees.Add(GetBasicFee(vehicleType, basePrice)),
            () => fees.Add(GetSpecialFee(vehicleType, basePrice)),
            () => fees.Add(GetAssociationFee(basePrice)),
            () => fees.Add(GetStorageFee())
        );

        return fees;
    }

    private double CheckRange(double min, double max, ref double cost)
    {
        if (cost < min)
        {
            return min;
        }
        else if (cost > max) 
        {
            return max;
        }
        else 
        {
            return cost;
        }
    }

    private Fee GetBasicFee(VehicleType vehicleType, int basePrice)
    {
        var cost = basePrice * 0.1;

        return new Fee
        {
            Type = FeeType.BASIC,
            Cost = Math.Round(vehicleType == VehicleType.COMMON ? CheckRange(10, 50, ref cost) : CheckRange(25, 200, ref cost),2)
        };
    }

    private Fee GetSpecialFee(VehicleType vehicleType, int basePrice)
    {
        return new Fee
        {
            Type = FeeType.SPECIAL,
            Cost = Math.Round(vehicleType == VehicleType.COMMON ? basePrice * 0.02 : basePrice * 0.04, 2)
        };
    }

    private Fee GetAssociationFee(int basePrice)
    {
        var cost = 0;

        if (basePrice == 1 || basePrice <= 500)
        {
            cost = 5;
        }
        else if (basePrice <= 1000)
        {
            cost = 10;
        }
        else if (basePrice <= 3000)
        {
            cost = 15;
        }
        else
        {
            cost = 20;
        }

        return new Fee
        {
            Type = FeeType.ASSOCIATION,
            Cost = cost
        };
    }

    private Fee GetStorageFee()
    {
        return new Fee
        {
            Type = FeeType.STORAGE,
            Cost = 100 // Fixed fee
        };
    }
}