namespace CarRental.Gateway.Models;

public class RentalAgreementDto
{
    public int Id { get; set; }
    public int RentalRequestId { get; set; }
    public decimal TotalPrice { get; set; }
    public DateTime CreatedAt { get; set; }
}
