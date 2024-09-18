using Projects;

var builder = DistributedApplication.CreateBuilder(args);

//builder.AddProject<RedirectLog_API>("redirectlog-api");

//builder.AddProject<Web_Gateway>("web-gateway");

var rabbitMq = builder.AddRabbitMQ("rabbitmq").WithManagementPlugin().WithImage("masstransit/rabbitmq");
var mongo = builder.AddMongoDB("mongo");
var mongoDb = mongo.AddDatabase("mongo-database");
var redis = builder.AddRedis("redis");

builder.AddProject<Shortner_API>("shortener-api")
    .WithReference(mongoDb)
    .WithReference(rabbitMq)
    .WithReference(redis);

builder.Build().Run();