using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using RedirectLog.Infrastructure.Extensions;
using RedirectLog.Infrastructure.Persistence;
using System.Reflection;
using RedirectLog.Application.Extensions;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using URLShortner.Common.Infrastructure.Logging;
using Elastic.Apm.NetCoreAll;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Host.UseSerilog(SeriLogger.Configure);
builder.Host.UseAllElasticApm();

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddApiVersioning(config =>
{
    config.DefaultApiVersion = new ApiVersion(1, 0);
    config.AssumeDefaultVersionWhenUnspecified = true;
    config.ReportApiVersions = true;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "URLShortner.API", Version = "v1" });
    // Set the comments path for the Swagger JSON and UI.
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.MapDefaultEndpoints();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<RedirectLogContext>();
    var hasPendingMigrations = db.Database.GetPendingMigrations().Any();
    if (hasPendingMigrations)
        db.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
