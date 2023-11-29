using BidCalculation.Api;

var builder = WebApplication.CreateBuilder(args);

var startup = new Startup(builder.Configuration);
startup.AddAservice(builder.Services);
var app = builder.Build();
// await PopulateInMemoryDb.Populate(app);
startup.RunApp(app);
