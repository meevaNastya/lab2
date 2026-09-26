namespace CarRental.Rentals.Entities;

public class RentalAgreement
{
    private RentalAgreement()
    {
    }

    public RentalAgreement(
        int rentalRequestId,
        decimal totalPrice)
    {
        if (rentalRequestId <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(rentalRequestId));

        if (totalPrice <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(totalPrice),
                "Total price must be positive.");

        RentalRequestId = rentalRequestId;
        TotalPrice = totalPrice;
        CreatedAt = DateTime.UtcNow;
    }

    public int Id { get; private set; }

    public int RentalRequestId { get; private set; }

    public decimal TotalPrice { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public void SetId(int id)
    {
        if (id <= 0)
            throw new ArgumentOutOfRangeException(nameof(id));

        Id = id;
    }
}
