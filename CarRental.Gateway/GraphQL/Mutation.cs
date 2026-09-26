using CarRental.Gateway.Models;

namespace CarRental.Gateway.GraphQL;

public class Mutation
{
    public async Task<CarDto?> CreateCar(
        string brand,
        string model,
        string vin,
        int category,
        decimal pricePerDay,
        [Service] IHttpClientFactory httpClientFactory)
    {
        var client = httpClientFactory.CreateClient("Cars");

        var url =
            $"/api/cars?brand={Uri.EscapeDataString(brand)}" +
            $"&model={Uri.EscapeDataString(model)}" +
            $"&vin={Uri.EscapeDataString(vin)}" +
            $"&category={category}" +
            $"&pricePerDay={pricePerDay}";

        var response = await client.PostAsync(url, null);

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<CarDto>();
    }

    public async Task<CarDto?> ChangeCarPrice(
        int id,
        decimal pricePerDay,
        [Service] IHttpClientFactory httpClientFactory)
    {
        var client = httpClientFactory.CreateClient("Cars");

        var response = await client.PutAsync(
            $"/api/cars/{id}/price?pricePerDay={pricePerDay}",
            null);

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<CarDto>();
    }

    public async Task<bool> DeleteCar(
        int id,
        [Service] IHttpClientFactory httpClientFactory)
    {
        var client = httpClientFactory.CreateClient("Cars");

        var response = await client.DeleteAsync($"/api/cars/{id}");

        return response.IsSuccessStatusCode;
    }

    public async Task<UserDto?> CreateUser(
        string username,
        string passwordHash,
        DateOnly birthDate,
        DateOnly? driverLicenseIssueDate,
        int role,
        [Service] IHttpClientFactory httpClientFactory)
    {
        var client = httpClientFactory.CreateClient("Users");

        var url =
            $"/api/users?username={Uri.EscapeDataString(username)}" +
            $"&passwordHash={Uri.EscapeDataString(passwordHash)}" +
            $"&birthDate={birthDate:yyyy-MM-dd}" +
            $"&driverLicenseIssueDate={driverLicenseIssueDate:yyyy-MM-dd}" +
            $"&role={role}";

        var response = await client.PostAsync(url, null);

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<UserDto>();
    }

    public async Task<UserDto?> ChangeUserPassword(
        int id,
        string passwordHash,
        [Service] IHttpClientFactory httpClientFactory)
    {
        var client = httpClientFactory.CreateClient("Users");

        var response = await client.PutAsync(
            $"/api/users/{id}/password?passwordHash={Uri.EscapeDataString(passwordHash)}",
            null);

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<UserDto>();
    }

    public async Task<bool> DeleteUser(
        int id,
        [Service] IHttpClientFactory httpClientFactory)
    {
        var client = httpClientFactory.CreateClient("Users");

        var response = await client.DeleteAsync($"/api/users/{id}");

        return response.IsSuccessStatusCode;
    }

    public async Task<RentalDto?> CreateRental(
        int clientId,
        int carId,
        DateOnly startDate,
        DateOnly endDate,
        [Service] IHttpClientFactory httpClientFactory)
    {
        var client = httpClientFactory.CreateClient("Rentals");

        var url =
            $"/api/rentals?clientId={clientId}" +
            $"&carId={carId}" +
            $"&startDate={startDate:yyyy-MM-dd}" +
            $"&endDate={endDate:yyyy-MM-dd}";

        var response = await client.PostAsync(url, null);

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<RentalDto>();
    }

    public async Task<RentalDto?> ApproveRental(
        int id,
        [Service] IHttpClientFactory httpClientFactory)
    {
        var client = httpClientFactory.CreateClient("Rentals");

        var response = await client.PutAsync(
            $"/api/rentals/{id}/approve",
            null);

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<RentalDto>();
    }

    public async Task<RentalAgreementDto?> CreateRentalAgreement(
        int id,
        decimal totalPrice,
        [Service] IHttpClientFactory httpClientFactory)
    {
        var client = httpClientFactory.CreateClient("Rentals");

        var response = await client.PostAsync(
            $"/api/rentals/{id}/agreement?totalPrice={totalPrice}",
            null);

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<RentalAgreementDto>();
    }

    public async Task<bool> DeleteRental(
        int id,
        [Service] IHttpClientFactory httpClientFactory)
    {
        var client = httpClientFactory.CreateClient("Rentals");

        var response = await client.DeleteAsync(
            $"/api/rentals/{id}");

        return response.IsSuccessStatusCode;
    }
}
