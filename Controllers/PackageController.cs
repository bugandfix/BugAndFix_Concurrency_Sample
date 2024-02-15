using BugAndFix_Concurrency_Sample.Data;
using BugAndFix_Concurrency_Sample.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BugAndFix_Concurrency_Sample.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PackageController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public PackageController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/Package
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Package>>> GetPackages()
    {
        if (_context.Packages == null)
        {
            return NotFound();
        }
        return await _context.Packages.ToListAsync();
    }

    // GET: api/Package/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Package>> GetPackage(int id)
    {
        if (_context.Packages == null)
        {
            return NotFound();
        }
        var package = await _context.Packages.FindAsync(id);

        if (package == null)
        {
            return NotFound();
        }

        return package;
    }

    // PUT: api/Package/5
    
    [HttpPut("{id}")]
    public async Task<IActionResult> PutPackage(int id, Package package)
    {
        if (id != package.PackageID)
        {
            return BadRequest();
        }

        _context.Entry(package).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!PackageExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // POST: api/Package    
    [HttpPost]
    public async Task<ActionResult<Package>> PostPackage(Package package)
    {
        if (_context.Packages == null)
        {
            return Problem("Entity set 'ApplicationDbContext.Packages'  is null.");
        }
        _context.Packages.Add(package);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetPackage", new { id = package.PackageID }, package);
    }

    // DELETE: api/Package/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePackage(int id, Package package)
    {
        if (_context.Packages == null)
        {
            return NotFound();
        }
        if (package == null)
        {
            return NotFound();
        }

        _context.Packages.Remove(package);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!PackageExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    private bool PackageExists(int id)
    {
        return (_context.Packages?.Any(e => e.PackageID == id)).GetValueOrDefault();
    }
}
