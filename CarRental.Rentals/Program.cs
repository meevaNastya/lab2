using CarRental.Rentals.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<RentalsDbContext>(options =>
    options.UseNpgsql(
        "Host=localhost;Port=5432;Database=car_rental_rentals;Username=anastasia"));

var app = builder.Build();

app.MapControllers();

app.Run();
