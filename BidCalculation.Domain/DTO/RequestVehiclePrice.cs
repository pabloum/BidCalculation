using BidCalculation.Domain.Enums;

namespace BidCalculation.Domain.DTO;

public class RequestVehiclePrice
{
    public int BasePrice { get; set; }
    public VehicleType VehicleType { get; set; }
}