using Microsoft.EntityFrameworkCore;

namespace CarRental.Cars.Data;

public class CarsDbContext : DbContext
{
    public CarsDbContext(DbContextOptions<CarsDbContext> options)
        : base(options)
    {
    }

    public DbSet<Car> Cars => Set<Car>();
}
