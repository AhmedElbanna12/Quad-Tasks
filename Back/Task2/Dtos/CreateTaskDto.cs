using Task2.Models;

namespace Task2.Dtos
{
    public class CreateTaskDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public TaskItemStatus Status { get; set; } 
        public int UserId { get; set; }
    }
}
