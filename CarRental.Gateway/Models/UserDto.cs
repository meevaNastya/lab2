namespace CarRental.Gateway.Models;

public class UserDto
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public DateOnly BirthDate { get; set; }
    public DateOnly? DriverLicenseIssueDate { get; set; }
    public List<int> Roles { get; set; } = new();
}
