using LotteryDDD.Application;
using LotteryDDD.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<EfDbContext>(options =>
{
    options
    .UseLazyLoadingProxies()
    .UseNpgsql("Host=localhost;Port=5432;Database=LotteryDDD;Username=postgres;Password=postgres");
});

builder.Services.AddControllers();
builder.Services.AddScoped<IUsersNotifierService, UsersNotifierService>();
builder.Services.AddScoped<KafkaProducerService>();


var app = builder.Build();

app.MapControllers();

app.Run();
