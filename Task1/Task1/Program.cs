namespace Task1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<TaskItem> tasks = new List<TaskItem>();
            int idCounter = 1;




            while (true)
            {
                Console.WriteLine("\n1. Add Task");
                Console.WriteLine("2. Update Task");
                Console.WriteLine("3. Delete Task");
                Console.WriteLine("4. List Tasks");
                Console.WriteLine("5. Exit");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddTask();
                        break;
                    case "2":
                        UpdateTask();
                        break;
                    case "3":
                        DeleteTask();
                        break;
                    case "4":
                        ListTasks();
                        break;
                    case "5":
                        return;
                }
            }


            void AddTask()
            {
                Console.Write("Enter title: ");
                var title = Console.ReadLine();

                tasks.Add(new TaskItem
                {
                    Id = idCounter++,
                    Title = title,
                    IsCompleted = false
                });

                Console.WriteLine("Task Added!");
            }


            void UpdateTask()
            {
                Console.Write("Enter task id: ");
                int id = int.Parse(Console.ReadLine());

                var task = tasks.FirstOrDefault(t => t.Id == id);

                if (task == null)
                {
                    Console.WriteLine("Task not found");
                    return;
                }

                Console.Write("New title: ");
                task.Title = Console.ReadLine();

                Console.Write("Is completed (true/false): ");
                task.IsCompleted = bool.Parse(Console.ReadLine());

                Console.WriteLine("Updated!");
            }


            void DeleteTask()
            {
                Console.Write("Enter task id: ");
                int id = int.Parse(Console.ReadLine());

                tasks.RemoveAll(t => t.Id == id);

                Console.WriteLine("Deleted!");
            }


            void ListTasks()
            {
                foreach (var task in tasks)
                {
                    Console.WriteLine($"{task.Id} - {task.Title} - {(task.IsCompleted ? "Done" : "Pending")}");
                }
            }


        }
    }
}
