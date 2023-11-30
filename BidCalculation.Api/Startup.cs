using BidCalculation.Api.Middleware;
using BidCalculation.Business;
using BidCalculation.Business.Implementations;

namespace BidCalculation.Api;

public class Startup
{
    public IConfiguration _configuration { get; set; }
    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void AddAservice(IServiceCollection services)
    {
        // Add services to the container.
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddScoped<IVehiclePriceService, VehiclePriceService>();
    }

    public void RunApp(WebApplication app)
    {
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseMiddleware<HandleExceptions>();

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}