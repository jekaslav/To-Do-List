using System;

namespace WebApplication1.Entities
{
    public class ToDoItemDetails
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDone { get; set; }
    }
}