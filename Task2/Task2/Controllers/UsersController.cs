using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task2.Context;
using Task2.Dtos;
using Task2.Models;

namespace Task2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _context.Users
                .Include(u => u.Tasks)
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    Name = u.Name,
                    Tasks = u.Tasks.Select(t => new TaskDto
                    {
                        Id = t.Id,
                        Title = t.Title,
                        Status = t.Status.ToString()
                    }).ToList()
                })
                .ToList();

            return Ok(users);
        }
    }
}
