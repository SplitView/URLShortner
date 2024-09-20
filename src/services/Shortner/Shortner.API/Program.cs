using System.Reflection;

using Asp.Versioning;

using Microsoft.OpenApi.Models;

using Serilog;

using Shortner.Application.Extensions;
using Shortner.Infrastructure.Extensions;

using URLShortner.Common.Infrastructure.Logging;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(x => x.AddPolicy(name: "Default", p =>
{
    p.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
}));

builder.AddServiceDefaults();
//builder.Host.UseSerilog(SeriLogger.Configure);
// Add services to the container.
builder.Host.UseSerilog(SeriLogger.Configure);
//builder.Host.UseAllElasticApm();

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
builder.AddInfrastructure(builder.Configuration);
var app = builder.Build();

app.MapDefaultEndpoints();
app.UseCors("Default");
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