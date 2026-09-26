using CarRental.Gateway.Models;

namespace CarRental.Gateway.GraphQL;

public class Query
{
    public string Hello() => "Car Rental GraphQL Gateway is working!";

    public async Task<List<CarDto>> GetCars(
        [Service] IHttpClientFactory httpClientFactory)
    {
        var client = httpClientFactory.CreateClient("Cars");

        var cars = await client.GetFromJsonAsync<List<CarDto>>("/api/cars");

        return cars ?? new List<CarDto>();
    }

    public async Task<List<UserDto>> GetUsers(
        [Service] IHttpClientFactory httpClientFactory)
    {
        var client = httpClientFactory.CreateClient("Users");

        var users = await client.GetFromJsonAsync<List<UserDto>>("/api/users");

        return users ?? new List<UserDto>();
    }

    public async Task<List<RentalDto>> GetRentals(
        [Service] IHttpClientFactory httpClientFactory)
    {
        var client = httpClientFactory.CreateClient("Rentals");

        var rentals = await client.GetFromJsonAsync<List<RentalDto>>("/api/rentals");

        return rentals ?? new List<RentalDto>();
    }

    public async Task<List<RentalAgreementDto>> GetRentalAgreements(
        [Service] IHttpClientFactory httpClientFactory)
    {
        var client = httpClientFactory.CreateClient("Rentals");

        var agreements =
            await client.GetFromJsonAsync<List<RentalAgreementDto>>(
                "/api/rentals/agreements");

        return agreements ?? new List<RentalAgreementDto>();
    }
}
