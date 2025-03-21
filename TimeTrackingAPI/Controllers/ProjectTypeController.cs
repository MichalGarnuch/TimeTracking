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
    public class ProjectTypeController : ControllerBase
    {
        private readonly TimeTrackingDBContext _context;

        public ProjectTypeController(TimeTrackingDBContext context)
        {
            _context = context;
        }

        // GET: api/ProjectType
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectType>>> GetProjectTypes()
        {
          if (_context.ProjectTypes == null)
          {
              return NotFound();
          }
            return await _context.ProjectTypes.ToListAsync();
        }

        // GET: api/ProjectType/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectType>> GetProjectType(int id)
        {
          if (_context.ProjectTypes == null)
          {
              return NotFound();
          }
            var projectType = await _context.ProjectTypes.FindAsync(id);

            if (projectType == null)
            {
                return NotFound();
            }

            return projectType;
        }

        // PUT: api/ProjectType/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProjectType(int id, ProjectType projectType)
        {
            if (id != projectType.ProjectTypeId)
            {
                return BadRequest();
            }

            _context.Entry(projectType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectTypeExists(id))
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

        // POST: api/ProjectType
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ProjectType>> PostProjectType(ProjectType projectType)
        {
          if (_context.ProjectTypes == null)
          {
              return Problem("Entity set 'TimeTrackingDBContext.ProjectTypes'  is null.");
          }
            _context.ProjectTypes.Add(projectType);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProjectType", new { id = projectType.ProjectTypeId }, projectType);
        }

        // DELETE: api/ProjectType/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProjectType(int id)
        {
            if (_context.ProjectTypes == null)
            {
                return NotFound();
            }
            var projectType = await _context.ProjectTypes.FindAsync(id);
            if (projectType == null)
            {
                return NotFound();
            }

            _context.ProjectTypes.Remove(projectType);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProjectTypeExists(int id)
        {
            return (_context.ProjectTypes?.Any(e => e.ProjectTypeId == id)).GetValueOrDefault();
        }
    }
}
