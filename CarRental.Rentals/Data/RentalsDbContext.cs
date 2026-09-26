using CarRental.Rentals.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Rentals.Data;

public class RentalsDbContext : DbContext
{
    public RentalsDbContext(DbContextOptions<RentalsDbContext> options)
        : base(options)
    {
    }

    public DbSet<RentalRequest> RentalRequests => Set<RentalRequest>();

    public DbSet<RentalAgreement> RentalAgreements => Set<RentalAgreement>();
}
