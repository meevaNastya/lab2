using CarRental.Gateway.GraphQL;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddHttpClient(
    "Cars",
    client =>
    {
        client.BaseAddress = new Uri("http://localhost:5001");
    });

builder.Services.AddHttpClient(
    "Users",
    client =>
    {
        client.BaseAddress = new Uri("http://localhost:5002");
    });

builder.Services.AddHttpClient(
    "Rentals",
    client =>
    {
        client.BaseAddress = new Uri("http://localhost:5003");
    });

builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>();

var app = builder.Build();

app.UseCors("Frontend");

app.MapGraphQL();

app.Run();
