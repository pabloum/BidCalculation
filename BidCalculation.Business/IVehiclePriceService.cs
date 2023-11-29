using BidCalculation.Domain;
using BidCalculation.Domain.DTO;

namespace BidCalculation.Business;

public interface IVehiclePriceService
{
    VehiclePrice GetTotalPrice(RequestVehiclePrice requsetVehiclePrice);
}
