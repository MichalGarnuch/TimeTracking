using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeTrackingAPI.Models;
using TimeTrackingAPI.Models.Dtos;


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
        public async Task<ActionResult<IEnumerable<TaskTagDto>>> GetAll()
        {
            var list = await _context.TaskTags
                .Include(tt => tt.Task)
                .Include(tt => tt.Tag)
                .Select(tt => new TaskTagDto
                {
                    TaskID = tt.TaskID,
                    TaskName = tt.Task.TaskName,
                    TagID = tt.TagID,
                    TagName = tt.Tag.TagName
                })
                .ToListAsync();

            return list;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TaskTagCreateDto dto)
        {
            var exists = await _context.TaskTags
                .AnyAsync(tt => tt.TaskID == dto.TaskID && tt.TagID == dto.TagID);
            if (exists)
                return Conflict("Relation already exists");

            bool taskExists = await _context.Tasks.AnyAsync(t => t.TaskId == dto.TaskID);
            bool tagExists = await _context.Tags.AnyAsync(t => t.TagId == dto.TagID);
            if (!taskExists || !tagExists)
                return NotFound("Task or Tag not found");

            var taskTag = new TaskTagEntity
            {
                TaskID = dto.TaskID,
                TagID = dto.TagID
            };

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
