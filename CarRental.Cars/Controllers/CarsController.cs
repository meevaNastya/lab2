using CarRental.Cars.Data;
using CarRental.Cars.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Cars.Controllers;

[ApiController]
[Route("api/cars")]
public class CarsController : ControllerBase
{
    private readonly CarsDbContext _db;

    public CarsController(CarsDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Car>>> GetAll()
    {
        return Ok(await _db.Cars.ToListAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Car>> GetById(int id)
    {
        var car = await _db.Cars.FindAsync(id);

        if (car is null)
            return NotFound();

        return Ok(car);
    }

    [HttpPost]
    public async Task<ActionResult<Car>> Create(
        string brand,
        string model,
        string vin,
        CarCategory category,
        decimal pricePerDay)
    {
        var car = new Car(
            brand,
            model,
            vin,
            category,
            pricePerDay);

        _db.Cars.Add(car);
        await _db.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetById),
            new { id = car.Id },
            car);
    }

    [HttpPut("{id:int}/price")]
    public async Task<IActionResult> ChangePrice(
        int id,
        decimal pricePerDay)
    {
        var car = await _db.Cars.FindAsync(id);

        if (car is null)
            return NotFound();

        car.ChangePricePerDay(pricePerDay);

        await _db.SaveChangesAsync();

        return Ok(car);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var car = await _db.Cars.FindAsync(id);

        if (car is null)
            return NotFound();

        _db.Cars.Remove(car);
        await _db.SaveChangesAsync();

        return NoContent();
    }
}
