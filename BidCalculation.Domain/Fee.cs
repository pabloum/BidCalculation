using BidCalculation.Domain.Enums;

namespace BidCalculation.Domain;

public class Fee 
{
    public FeeType Type { get; set; }
    public int Cost { get; set; }
}