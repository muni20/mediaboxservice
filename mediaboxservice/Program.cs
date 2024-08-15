using mediaboxservice;
using Serilog;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddWindowsService(options =>
{
    options.ServiceName = "MeidaBox Watcher Service";
});

// Configure Serilog
builder.Logging.ClearProviders(); // Optional: Clear other logging providers
builder.Logging.AddSerilog(new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)  // Read settings from appsettings.json
    .Enrich.FromLogContext()
    .WriteTo.Console()  // Optional: write logs to the console
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)  // Write logs to a file, rolling every day
    .CreateLogger());

builder.Services.AddHostedService<Worker>();


var host = builder.Build();

host.Run();
