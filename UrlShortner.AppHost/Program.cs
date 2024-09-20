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

var elasticSearch = builder.AddContainer("elasticsearch", "elasticsearch", "8.15.1")
    .WithHttpEndpoint(9200, 9200, "elastic-endpoint")
    .WithEnvironment("discovery.type", "single-node");
var elasticEndpoint = elasticSearch.GetEndpoint("elastic-endpoint");
var kibana = builder.AddContainer("kibana", "kibana", "8.15.1")
    .WithHttpEndpoint(5601, 5601)
    .WithEnvironment("ELASTICSEARCH_HOSTS", "http://elasticsearch:9200")
    .WithReference(elasticEndpoint);

builder.AddProject<RedirectLog_API>("redirect-log-api").WithReference(sqlDb).WithReference(rabbitMq);

builder.AddProject<Shortner_API>("shortener-api")
    .WithReference(mongoDb)
    .WithReference(rabbitMq)
    .WithReference(redis);


builder.Build().Run();