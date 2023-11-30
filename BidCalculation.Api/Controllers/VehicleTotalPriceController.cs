using BidCalculation.Business;
using BidCalculation.Domain.DTO;
using Microsoft.AspNetCore.Mvc;

namespace BidCalculation.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class VehicleTotalPriceController : ControllerBase
{
    private readonly ILogger<VehicleTotalPriceController> _logger;
    private readonly IVehiclePriceService _vehiclePriceService;

    public VehicleTotalPriceController(ILogger<VehicleTotalPriceController> logger, IVehiclePriceService vehiclePriceService)
    {
        _logger = logger;
        _vehiclePriceService = vehiclePriceService;
    }

    [HttpPost("[action]")]
    public IActionResult Calculate([FromBody]RequestVehiclePrice requestVehiclePrice)
    {
        var totalPrice = _vehiclePriceService.GetTotalPrice(requestVehiclePrice);
        return Ok(totalPrice);
    }
}
