using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeTrackingAPI.Models;

namespace TimeTrackingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskTagController : ControllerBase
    {
        private readonly TimeTrackingDBContext _context;

        public TaskTagController(TimeTrackingDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskTagEntity>>> GetAll()
        {
            return await _context.TaskTags.ToListAsync();
        }

        [HttpPost]
        public async Task<IActionResult> Create(TaskTagEntity taskTag)
        {
            _context.TaskTags.Add(taskTag);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{taskId}/{tagId}")]
        public async Task<IActionResult> Delete(int taskId, int tagId)
        {
            var entity = await _context.TaskTags
                .FirstOrDefaultAsync(tt => tt.TaskID == taskId && tt.TagID == tagId);

            if (entity == null)
                return NotFound();

            _context.TaskTags.Remove(entity);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
