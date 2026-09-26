using CarRental.Users.Data;
using CarRental.Users.Entities;
using CarRental.Users.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Users.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly UsersDbContext _db;

    public UsersController(UsersDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<object>>> GetAll()
    {
        var users = await _db.Users
            .Include(u => u.RoleAssignments)
            .ToListAsync();

        return Ok(users.Select(ToResponse));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<object>> GetById(int id)
    {
        var user = await _db.Users
            .Include(u => u.RoleAssignments)
            .FirstOrDefaultAsync(u => u.Id == id);

        if (user is null)
            return NotFound();

        return Ok(ToResponse(user));
    }

    [HttpPost]
    public async Task<ActionResult<object>> Create(
        string username,
        string passwordHash,
        DateOnly birthDate,
        DateOnly? driverLicenseIssueDate,
        UserRole role)
    {
        var user = new User(
            username,
            passwordHash,
            birthDate,
            driverLicenseIssueDate,
            new[] { role });

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetById),
            new { id = user.Id },
            ToResponse(user));
    }

    [HttpPut("{id:int}/password")]
    public async Task<IActionResult> ChangePassword(
        int id,
        string passwordHash)
    {
        var user = await _db.Users
            .Include(u => u.RoleAssignments)
            .FirstOrDefaultAsync(u => u.Id == id);

        if (user is null)
            return NotFound();

        user.ChangePasswordHash(passwordHash);

        await _db.SaveChangesAsync();

        return Ok(ToResponse(user));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var user = await _db.Users
            .Include(u => u.RoleAssignments)
            .FirstOrDefaultAsync(u => u.Id == id);

        if (user is null)
            return NotFound();

        _db.Users.Remove(user);
        await _db.SaveChangesAsync();

        return NoContent();
    }

    private static object ToResponse(User user)
    {
        return new
        {
            user.Id,
            user.Username,
            user.BirthDate,
            user.DriverLicenseIssueDate,
            Roles = user.Roles
        };
    }
}
