namespace Task2.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<TaskDto> Tasks { get; set; }
    }
}
