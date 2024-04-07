using LotteryDDD.Application;
using LotteryDDD.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<EfDbContext>(options =>
{
    options.UseNpgsql("Host=localhost;Port=5432;Database=LotteryDDD;Username=postgres;Password=HGRMxLa6"); // удалить потом
});

builder.Services.AddControllers();
builder.Services.AddScoped<IGameService, GameService>();

var app = builder.Build();

app.MapControllers();

app.Run();
