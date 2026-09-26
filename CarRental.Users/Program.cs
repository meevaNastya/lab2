using CarRental.Users.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<UsersDbContext>(options =>
    options.UseNpgsql(
        "Host=localhost;Port=5432;Database=car_rental_users;Username=anastasia"));

var app = builder.Build();

app.MapControllers();

app.Run();
