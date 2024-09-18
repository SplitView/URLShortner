using Projects;

var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<RedirectLog_API>("redirectlog-api");

builder.AddProject<Web_Gateway>("web-gateway");

builder.AddProject<Shortner_API>("shortner-api");

builder.Build().Run();