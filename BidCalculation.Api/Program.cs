using BidCalculation.Api.Controllers;

var builder = WebApplication.CreateBuilder(args);

var startup = new Startup(builder.Configuration);
startup.AddAservice(builder.Services);
var app = builder.Build();
startup.RunApp(app);
