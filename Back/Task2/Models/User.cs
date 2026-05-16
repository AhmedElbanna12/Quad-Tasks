namespace Task2.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<TaskItem> Tasks { get; set; }
    }
}
