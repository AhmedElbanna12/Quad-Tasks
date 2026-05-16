using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task2.Context;
using Task2.Dtos;
using Task2.Models;

namespace Task2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TasksController(AppDbContext context)
        {
            _context = context;
        }

        // ================= GET ALL =================
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var tasks = await _context.Tasks
                .Include(t => t.User)
                .Select(t => new TaskResponseDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    UserName = t.User.Name,
                    Status = (int)t.Status
                })
                .ToListAsync();

            return Ok(tasks);
        }

        // ================= GET BY ID =================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var task = await _context.Tasks
                .Include(t => t.User)
                .Where(t => t.Id == id)
                .Select(t => new TaskResponseDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    UserName = t.User.Name,
                    Status = (int)t.Status
                })
                .FirstOrDefaultAsync();

            if (task == null)
                return NotFound("Task not found");

            return Ok(task);
        }

        // ================= GET BY USER =================
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUser(int userId)
        {
            var tasks = await _context.Tasks
                .Where(t => t.UserId == userId)
                .Include(t => t.User)
                .Select(t => new TaskResponseDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    UserName = t.User.Name,
                    Status = (int)t.Status
                })
                .ToListAsync();

            return Ok(tasks);
        }

        // ================= CREATE =================
        [HttpPost]
        public async Task<IActionResult> CreateTask(CreateTaskDto dto)
        {
            var task = new TaskItem
            {
                Title = dto.Title,
                Description = dto.Description,
                Status = (TaskItemStatus)dto.Status,
                UserId = dto.UserId
            };

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            var result = await _context.Tasks
                .Include(t => t.User)
                .Where(t => t.Id == task.Id)
                .Select(t => new TaskResponseDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    UserName = t.User.Name,
                    Status = (int)t.Status
                })
                .FirstOrDefaultAsync();

            return Ok(result);
        }

        // ================= UPDATE =================
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, UpdateTaskDto dto)
        {
            var task = await _context.Tasks.FindAsync(id);

            if (task == null)
                return NotFound("Task not found");

            task.Title = dto.Title;
            task.Description = dto.Description;
            task.Status = (TaskItemStatus)dto.Status;
            task.UserId = dto.UserId;

            await _context.SaveChangesAsync();

            var result = await _context.Tasks
                .Include(t => t.User)
                .Where(t => t.Id == id)
                .Select(t => new TaskResponseDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    UserName = t.User.Name,
                    Status = (int)t.Status
                })
                .FirstOrDefaultAsync();

            return Ok(result);
        }

        // ================= DELETE =================
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await _context.Tasks.FindAsync(id);

            if (task == null)
                return NotFound("Task not found");

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}