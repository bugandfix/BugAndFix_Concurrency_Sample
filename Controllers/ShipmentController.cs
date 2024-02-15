using BugAndFix_Concurrency_Sample.Data;
using BugAndFix_Concurrency_Sample.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BugAndFix_Concurrency_Sample.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ShipmentController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ShipmentController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/Shipments
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Shipment>>> GetShipments()
    {
        if (_context.Shipments == null)
        {
            return NotFound();
        }
        return await _context.Shipments.ToListAsync();
    }

    // GET: api/Shipment/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Shipment>> GetShipment(int id)
    {
        if (_context.Shipments == null)
        {
            return NotFound();
        }
        var shipment = await _context.Shipments.FindAsync(id);

        if (shipment == null)
        {
            return NotFound();
        }

        return shipment;
    }

    // PUT: api/Shipment/5    
    [HttpPut("{id}")]
    public async Task<IActionResult> PutShipment(int id, Shipment shipment)
    {
        if (id != shipment.ShipmentID)
        {
            return BadRequest();
        }

        _context.Entry(shipment).State = EntityState.Modified;

        try
        {
            shipment.RecordVersion = Guid.NewGuid();
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ShipmentExists(id))
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

    // POST: api/Shipment    
    [HttpPost]
    public async Task<ActionResult<Shipment>> PostShipment(Shipment shipment)
    {
        if (_context.Shipments == null)
        {
            return Problem("Entity set 'ApplicationDbContext.Shipment'  is null.");
        }
        shipment.RecordVersion = Guid.NewGuid();
        _context.Shipments.Add(shipment);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetShipment", new { id = shipment.ShipmentID }, shipment);
    }

    // DELETE: api/Shipment/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteShipment(int id, Shipment shipment)
    {
        if (_context.Shipments == null)
        {
            return NotFound();
        }
        //var shipment = await _context.Shipments.FindAsync(id);
        if (shipment == null)
        {
            return NotFound();
        }
        _context.Shipments.Remove(shipment);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ShipmentExists(id))
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

    private bool ShipmentExists(int id)
    {
        return (_context.Shipments?.Any(e => e.ShipmentID == id)).GetValueOrDefault();
    }
}
