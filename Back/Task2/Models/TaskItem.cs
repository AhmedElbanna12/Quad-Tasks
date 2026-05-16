using System.ComponentModel.DataAnnotations;

namespace Task2.Models
{
    public class TaskItem
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        public string Description { get; set; }


        public TaskItemStatus Status { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }

    public enum TaskItemStatus
    {
        Pending = 0,
        InProgress = 1,
        Completed = 2
    }
}
