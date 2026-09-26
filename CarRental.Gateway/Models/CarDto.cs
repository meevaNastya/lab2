namespace CarRental.Gateway.Models;

public class CarDto
{
    public int Id { get; set; }
    public string Brand { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public string Vin { get; set; } = string.Empty;
    public int Category { get; set; }
    public decimal PricePerDay { get; set; }
}
