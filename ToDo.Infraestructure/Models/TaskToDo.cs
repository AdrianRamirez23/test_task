namespace ToDo.Infraestructure.Models
{
    public class TaskToDo
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
