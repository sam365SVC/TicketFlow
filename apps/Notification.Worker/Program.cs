using Notification.Infrastructure.Configurations;
using Notification.Infrastructure.Mongo.Data;
using Notification.Infrastructure.Storage;
using Notification.Worker;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.Configure<AwsS3Options>(builder.Configuration.GetSection(AwsS3Options.SectionName));


builder.Services.AddSingleton<MongoDbContext>();
builder.Services.AddTransient<IAwsS3Client, AwsS3Client>();
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
