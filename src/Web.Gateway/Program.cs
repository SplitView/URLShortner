using Elastic.Apm.NetCoreAll;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Serilog;
using URLShortner.Common.Infrastructure.Logging;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Host.ConfigureAppConfiguration((context, config) =>
{
    config.AddJsonFile("ocelot.json");
});

builder.Host.UseSerilog(SeriLogger.Configure);
builder.Host.UseAllElasticApm();

builder.Services.AddOcelot();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();

app.UseOcelot();


app.Run();