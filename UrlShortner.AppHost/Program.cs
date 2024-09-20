using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var sqlServer = builder.AddSqlServer("sql");
var sqlDb = sqlServer.AddDatabase("sqlDb");

//builder.AddProject<Web_Gateway>("web-gateway");

var rabbitUser = builder.AddParameter("RabbitUser", true);
var rabbitPass = builder.AddParameter("RabbitPass", true);
var rabbitMq = builder.AddRabbitMQ("RabbitMq", userName: rabbitUser, password: rabbitPass).WithManagementPlugin().WithImage("masstransit/rabbitmq");
var mongo = builder.AddMongoDB("mongo");
var mongoDb = mongo.AddDatabase("mongodb");
var redis = builder.AddRedis("redis");


builder.AddProject<RedirectLog_API>("redirect-log-api").WithReference(sqlDb).WithReference(rabbitMq);

builder.AddProject<Shortner_API>("shortener-api")
    .WithReference(mongoDb)
    .WithReference(rabbitMq)
    .WithReference(redis);

builder.Build().Run();