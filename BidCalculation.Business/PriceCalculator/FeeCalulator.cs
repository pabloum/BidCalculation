using System.Collections.Concurrent;
using BidCalculation.Domain;
using BidCalculation.Domain.Enums;

namespace BidCalculation.Business.PriceCalculator;

public interface IFeeCalculator
{
    IEnumerable<Fee> GetFees(int basePrice, VehicleType vehicleType);
}

public class FeeCalculator : IFeeCalculator
{
    private readonly IDictionary<FeeType, Func<VehicleType, int, double>> _feeCostCalculator = new Dictionary<FeeType, Func<VehicleType, int, double>>
    {
        {FeeType.BASIC, GetBasicFee},
        {FeeType.SPECIAL, GetSpecialFee},
        {FeeType.ASSOCIATION,  (vehicleType, basePrice) => GetAssociationFee(basePrice)},
        {FeeType.STORAGE, (vehicleType, basePrice)  => GetStorageFee()},
    };

    public IEnumerable<Fee> GetFees(int basePrice, VehicleType vehicleType)
    {
        var fees = new ConcurrentBag<Fee>();

        Parallel.ForEach(Enum.GetValues<FeeType>(), feeType => 
        {
            fees.Add(CreateFee(feeType, _feeCostCalculator[feeType].Invoke(vehicleType, basePrice)));
        });

        return fees;
    }

    private Fee CreateFee(FeeType feeType, double cost)
    {
        return new Fee 
        {
            Type = feeType,
            Cost = cost
        };
    }

    private static double GetBasicFee(VehicleType vehicleType, int basePrice)
    {
        var cost = basePrice * 0.1;
        return Math.Round(vehicleType == VehicleType.COMMON ? CheckRange(10, 50, ref cost) : CheckRange(25, 200, ref cost),2);
    }

    private static double GetSpecialFee(VehicleType vehicleType, int basePrice)
    {
        return Math.Round(vehicleType == VehicleType.COMMON ? basePrice * 0.02 : basePrice * 0.04, 2);
    }

    private static double GetAssociationFee(int basePrice)
    {
        if (basePrice == 1 || basePrice <= 500)
        {
            return 5.00;
        }
        else if (basePrice <= 1000)
        {
            return 10.00;
        }
        else if (basePrice <= 3000)
        {
            return 15.00;
        }
        else
        {
            return 20.00;
        }
    }

    private static double GetStorageFee()
    {
        return 100.00; // Fixed fee
    }

    private static double CheckRange(double min, double max, ref double cost)
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
}