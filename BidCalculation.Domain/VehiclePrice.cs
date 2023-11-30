namespace BidCalculation.Domain;

public class VehiclePrice
{
    public int BasePrice { get; set; }
    public IEnumerable<Fee> Fees { get; set; } = new List<Fee>();
    public double TotalPrice { get; set; }
}
