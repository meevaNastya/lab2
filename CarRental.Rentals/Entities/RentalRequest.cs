using CarRental.Rentals.Enums;

namespace CarRental.Rentals.Entities;

public class RentalRequest
{
    private RentalRequest()
    {
    }

    public RentalRequest(
        int clientId,
        int carId,
        DateOnly startDate,
        DateOnly endDate)
    {
        if (clientId <= 0)
            throw new ArgumentOutOfRangeException(nameof(clientId));

        if (carId <= 0)
            throw new ArgumentOutOfRangeException(nameof(carId));

        if (startDate == default)
            throw new ArgumentException(
                "Start date is required.",
                nameof(startDate));

        if (endDate < startDate)
            throw new ArgumentException(
                "End date cannot be earlier than start date.",
                nameof(endDate));

        ClientId = clientId;
        CarId = carId;
        StartDate = startDate;
        EndDate = endDate;
        Status = RentalRequestStatus.Created;
    }

    public int Id { get; private set; }

    public int ClientId { get; private set; }

    public int CarId { get; private set; }

    public DateOnly StartDate { get; private set; }

    public DateOnly EndDate { get; private set; }

    public RentalRequestStatus Status { get; private set; }

    public void SetId(int id)
    {
        if (id <= 0)
            throw new ArgumentOutOfRangeException(nameof(id));

        Id = id;
    }

    public void Approve()
    {
        if (Status != RentalRequestStatus.Created)
        {
            throw new InvalidOperationException(
                "Only a created request can be approved.");
        }

        Status = RentalRequestStatus.Approved;
    }

    public void Reject()
    {
        if (Status != RentalRequestStatus.Created)
        {
            throw new InvalidOperationException(
                "Only a created request can be rejected.");
        }

        Status = RentalRequestStatus.Rejected;
    }

    public void Cancel()
    {
        if (Status != RentalRequestStatus.Created &&
            Status != RentalRequestStatus.Approved)
        {
            throw new InvalidOperationException(
                "Only a created or approved request can be cancelled.");
        }

        Status = RentalRequestStatus.Cancelled;
    }
}
