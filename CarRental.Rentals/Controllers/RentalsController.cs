using CarRental.Rentals.Data;
using CarRental.Rentals.Entities;
using CarRental.Rentals.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Rentals.Controllers;

[ApiController]
[Route("api/rentals")]
public class RentalsController : ControllerBase
{
    private readonly RentalsDbContext _db;

    public RentalsController(RentalsDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RentalRequest>>> GetAll()
    {
        return Ok(await _db.RentalRequests.ToListAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<RentalRequest>> GetById(int id)
    {
        var request = await _db.RentalRequests.FindAsync(id);

        if (request is null)
            return NotFound();

        return Ok(request);
    }

    [HttpPost]
    public async Task<ActionResult<RentalRequest>> CreateRequest(
        int clientId,
        int carId,
        DateOnly startDate,
        DateOnly endDate)
    {
        var request = new RentalRequest(
            clientId,
            carId,
            startDate,
            endDate);

        _db.RentalRequests.Add(request);
        await _db.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetById),
            new { id = request.Id },
            request);
    }

    [HttpPut("{id:int}/approve")]
    public async Task<IActionResult> Approve(int id)
    {
        var request = await _db.RentalRequests.FindAsync(id);

        if (request is null)
            return NotFound();

        request.Approve();
        await _db.SaveChangesAsync();

        return Ok(request);
    }

    [HttpPut("{id:int}/reject")]
    public async Task<IActionResult> Reject(int id)
    {
        var request = await _db.RentalRequests.FindAsync(id);

        if (request is null)
            return NotFound();

        request.Reject();
        await _db.SaveChangesAsync();

        return Ok(request);
    }

    [HttpPut("{id:int}/cancel")]
    public async Task<IActionResult> Cancel(int id)
    {
        var request = await _db.RentalRequests.FindAsync(id);

        if (request is null)
            return NotFound();

        request.Cancel();
        await _db.SaveChangesAsync();

        return Ok(request);
    }

    [HttpPost("{id:int}/agreement")]
    public async Task<ActionResult<RentalAgreement>> CreateAgreement(
        int id,
        decimal totalPrice)
    {
        var request = await _db.RentalRequests.FindAsync(id);

        if (request is null)
            return NotFound();

        if (request.Status != RentalRequestStatus.Approved)
            return BadRequest(
                "Rental request must be approved first.");

        var agreement = new RentalAgreement(
            request.Id,
            totalPrice);

        _db.RentalAgreements.Add(agreement);
        await _db.SaveChangesAsync();

        return Ok(agreement);
    }

    [HttpGet("agreements")]
    public async Task<ActionResult<IEnumerable<RentalAgreement>>> GetAgreements()
    {
        return Ok(await _db.RentalAgreements.ToListAsync());
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var request = await _db.RentalRequests.FindAsync(id);

        if (request is null)
            return NotFound();

        var agreements = await _db.RentalAgreements
            .Where(a => a.RentalRequestId == id)
            .ToListAsync();

        _db.RentalAgreements.RemoveRange(agreements);
        _db.RentalRequests.Remove(request);

        await _db.SaveChangesAsync();

        return NoContent();
    }
}
