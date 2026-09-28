using System;
using System.Linq;
using TodoApp1.Data;
using TodoApp1.Enums;
using TodoApp1.Models;

namespace TodoApp1.Services
{
    internal class TodoService
    {
        private DataContext DC = new DataContext();

        public void CreateTodo()
        {
            CheckLoggedIn();

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("Enter title:");
            string title = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(title)) throw new Exception("Title cannot be empty.");

            Console.WriteLine("Enter description (optional):");
            string? description = Console.ReadLine();

            Console.WriteLine("Enter due date (yyyy-MM-dd) or leave blank for none:");
            string dueInput = Console.ReadLine();
            DateTime? dueDate = null;
            if (!string.IsNullOrWhiteSpace(dueInput))
            {
                if (!DateTime.TryParse(dueInput, out DateTime parsed)) throw new Exception("Invalid date format.");
                dueDate = parsed;
            }

            Console.WriteLine("Select priority: 0=Low, 1=Medium, 2=High, 3=Critical (press Enter for Medium):");
            string priorityInput = Console.ReadLine();
            Priority priority = Priority.Medium;
            if (!string.IsNullOrWhiteSpace(priorityInput))
            {
                if (!int.TryParse(priorityInput, out int p) || p < 0 || p > 3) throw new Exception("Invalid priority selection.");
                priority = (Priority)p;
            }

            var todo = new TodoItem
            {
                Title = title,
                Description = string.IsNullOrWhiteSpace(description) ? null : description,
                DueDate = dueDate,
                Priority = priority,
                CreatedAt = DateTime.UtcNow,
                UserId = UserService.loggedInUser!.Id
            };

            DC.TodoItems.Add(todo);
            DC.SaveChanges();

            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Todo created successfully.");
            Console.ResetColor();
        }

        public void GetAllTodos()
        {
            CheckLoggedIn();
            var todos = DC.TodoItems
                .Select(t => new
                {
                    t.Id,
                    t.Title,
                    t.Status,
                    t.Priority,
                    t.DueDate,
                    t.UserId
                })
                .ToList();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Todos:");
            foreach (var t in todos)
            {
                var due = t.DueDate.HasValue ? t.DueDate.Value.ToString("s") : "no due";
                Console.WriteLine($"ID: {t.Id}, Title: {t.Title}, Status: {t.Status}, Priority: {t.Priority}, Due: {due}, UserId: {t.UserId}");
            }
            Console.ResetColor();
        }

        public void GetMyTodos()
        {
            CheckLoggedIn();
            var userId = UserService.loggedInUser!.Id;
            var todos = DC.TodoItems.Where(t => t.UserId == userId).ToList();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"Todos for {UserService.loggedInUser!.Username}:");
            foreach (var t in todos)
            {
                Console.WriteLine(t);
            }
            Console.ResetColor();
        }

        public void UpdateTodo()
        {
            CheckLoggedIn();
            GetAllTodos();
            Console.WriteLine("Enter the ID of the todo to update:");
            if (!int.TryParse(Console.ReadLine(), out int todoId)) throw new Exception("Invalid ID.");

            var todo = DC.TodoItems.Find(todoId);
            if (todo == null) throw new Exception("Todo not found.");

            if (todo.UserId != UserService.loggedInUser!.Id) throw new Exception("You can only update your own todos.");

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("Enter new title (leave blank to keep current):");
            string title = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(title)) todo.Title = title;

            Console.WriteLine("Enter new description (leave blank to keep current):");
            string description = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(description)) todo.Description = description;

            Console.WriteLine("Enter new due date (yyyy-MM-dd) (leave blank to keep current):");
            string dueInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(dueInput))
            {
                if (!DateTime.TryParse(dueInput, out DateTime parsed)) throw new Exception("Invalid date format.");
                todo.DueDate = parsed;
            }

            Console.WriteLine("Select new priority: 0=Low, 1=Medium, 2=High, 3=Critical (leave blank to keep current):");
            string priorityInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(priorityInput))
            {
                if (!int.TryParse(priorityInput, out int p) || p < 0 || p > 3) throw new Exception("Invalid priority selection.");
                todo.Priority = (Priority)p;
            }

            DC.TodoItems.Update(todo);
            DC.SaveChanges();

            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Todo updated successfully.");
            Console.ResetColor();
        }

        public void DeleteTodo()
        {
            CheckLoggedIn();
            GetAllTodos();
            Console.WriteLine("Enter the ID of the todo to delete:");
            if (!int.TryParse(Console.ReadLine(), out int todoId)) throw new Exception("Invalid ID.");

            var todo = DC.TodoItems.Find(todoId);
            if (todo == null) throw new Exception("Todo not found.");

            if (todo.UserId != UserService.loggedInUser!.Id) throw new Exception("You can only delete your own todos.");

            DC.TodoItems.Remove(todo);
            DC.SaveChanges();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Todo deleted successfully.");
            Console.ResetColor();
        }

        public void ChangeStatus()
        {
            CheckLoggedIn();
            GetAllTodos();
            Console.WriteLine("Enter the ID of the todo to change status:");
            if (!int.TryParse(Console.ReadLine(), out int todoId)) throw new Exception("Invalid ID.");

            var todo = DC.TodoItems.Find(todoId);
            if (todo == null) throw new Exception("Todo not found.");

            if (todo.UserId != UserService.loggedInUser!.Id) throw new Exception("You can only change status of your own todos.");

            Console.WriteLine("Select status: 0=Todo, 1=InProgress, 2=Done, 3=Archived");
            if (!int.TryParse(Console.ReadLine(), out int s) || s < 0 || s > 3) throw new Exception("Invalid status selection.");

            todo.Status = (TodoStatus)s;
            DC.TodoItems.Update(todo);
            DC.SaveChanges();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Status updated successfully.");
            Console.ResetColor();
        }

        public void SearchTodos()
        {
            CheckLoggedIn();
            Console.WriteLine("Enter search term (title or description):");
            string search = (Console.ReadLine() ?? string.Empty).ToLower();

            var todos = DC.TodoItems
                .Where(t => t.Title.ToLower().Contains(search) || (t.Description ?? string.Empty).ToLower().Contains(search))
                .ToList();

            Console.ForegroundColor = ConsoleColor.Cyan;
            if (!todos.Any())
            {
                Console.WriteLine("No todos found.");
            }
            else
            {
                foreach (var t in todos)
                {
                    Console.WriteLine(t);
                }
            }
            Console.ResetColor();
        }

        private void CheckLoggedIn()
        {
            if (UserService.loggedInUser == null) throw new Exception("You must be logged in to perform this action.");
        }
    }
}
