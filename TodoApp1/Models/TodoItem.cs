using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp1.Enums;

namespace TodoApp1.Models
{
    internal class TodoItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DueDate { get; set; }
        public TodoStatus Status { get; set; } = TodoStatus.Todo;
        public Priority Priority { get; set; } = Priority.Medium;
        public int? UserId { get; set; }   
        public User? User { get; set; }

        public override string ToString()
        {
            var due = DueDate.HasValue ? DueDate.Value.ToString("s") : "no due";
            return $"ID: {Id}, Title: {Title}, Status: {Status}, Priority: {Priority}, Due: {due}";
        }

    }
}
