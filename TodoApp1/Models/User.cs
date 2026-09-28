using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApp1.Models
{
    internal class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<TodoItem> Todos { get; set; } = new List<TodoItem>();

        public override string ToString()
        {
            return $"ID: {Id}, Username: {Username}, Email: {Email}";
        }

    }
}
