using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TimeTrackingAPI.Models;

namespace TimeTrackingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimeRecordController : ControllerBase
    {
        private readonly TimeTrackingDBContext _context;

        public TimeRecordController(TimeTrackingDBContext context)
        {
            _context = context;
        }

        // GET: api/TimeRecord
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TimeRecord>>> GetTimeRecords()
        {
          if (_context.TimeRecords == null)
          {
              return NotFound();
          }
            return await _context.TimeRecords.ToListAsync();
        }

        // GET: api/TimeRecord/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TimeRecord>> GetTimeRecord(int id)
        {
          if (_context.TimeRecords == null)
          {
              return NotFound();
          }
            var timeRecord = await _context.TimeRecords.FindAsync(id);

            if (timeRecord == null)
            {
                return NotFound();
            }

            return timeRecord;
        }

        // PUT: api/TimeRecord/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTimeRecord(int id, TimeRecord timeRecord)
        {
            if (id != timeRecord.TimeRecordId)
            {
                return BadRequest();
            }

            _context.Entry(timeRecord).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TimeRecordExists(id))
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

        // POST: api/TimeRecord
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TimeRecord>> PostTimeRecord(TimeRecord timeRecord)
        {
          if (_context.TimeRecords == null)
          {
              return Problem("Entity set 'TimeTrackingDBContext.TimeRecords'  is null.");
          }
            _context.TimeRecords.Add(timeRecord);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTimeRecord", new { id = timeRecord.TimeRecordId }, timeRecord);
        }

        // DELETE: api/TimeRecord/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTimeRecord(int id)
        {
            if (_context.TimeRecords == null)
            {
                return NotFound();
            }
            var timeRecord = await _context.TimeRecords.FindAsync(id);
            if (timeRecord == null)
            {
                return NotFound();
            }

            _context.TimeRecords.Remove(timeRecord);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TimeRecordExists(int id)
        {
            return (_context.TimeRecords?.Any(e => e.TimeRecordId == id)).GetValueOrDefault();
        }
    }
}
