var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.RedirectLog_API>("redirectlog-api");

builder.AddProject<Projects.Web_Gateway>("web-gateway");

builder.AddProject<Projects.Shortner_API>("shortner-api");

builder.Build().Run();
