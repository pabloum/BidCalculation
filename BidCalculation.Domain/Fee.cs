using BidCalculation.Domain.Enums;

namespace BidCalculation.Domain;

public class Fee 
{
    public FeeType Type { get; set; }
    public double Cost { get; set; }
}