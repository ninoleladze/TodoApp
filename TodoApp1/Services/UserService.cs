using TodoApp1.Data;
using TodoApp1.Models;
using System;
using System.Linq;

namespace TodoApp1.Services
{
    internal class UserService
    {
        private DataContext DC = new DataContext();

        public static User? loggedInUser;

        public void RegisterUser()
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("Enter username:");
            string username = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(username)) throw new Exception("Username cannot be empty.");

            if (DC.Users.Any(u => u.Username == username))
                throw new Exception("Username already exists.");

            Console.WriteLine("Enter email:");
            string email = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(email) || !email.Contains('@')) throw new Exception("Invalid email.");

            Console.WriteLine("Enter password (minimum 4 characters):");
            string password = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(password) || password.Length < 4) throw new Exception("Password too short.");

            var user = new User
            {
                Username = username,
                Email = email,
                Password = password
            };

            DC.Users.Add(user);
            DC.SaveChanges();

            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("User registered successfully.");
            Console.ResetColor();
        }

        public void Login()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Enter username:");
            string username = Console.ReadLine();
            Console.WriteLine("Enter password:");
            string password = Console.ReadLine();

            var user = DC.Users.FirstOrDefault(u => u.Username == username && u.Password == password);
            if (user == null) throw new Exception("Invalid username or password.");

            loggedInUser = user;
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Logged in as {user.Username}.");
            Console.ResetColor();
        }

        public void Logout()
        {
            CheckLoggedIn();
            loggedInUser = null;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Logged out.");
            Console.ResetColor();
        }

        public void GetAllUsers()
        {
            CheckLoggedIn();
            var users = DC.Users.ToList();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Users:");
            foreach (var user in users)
            {
                Console.WriteLine(user);
            }
            Console.ResetColor();
        }

        public void UpdateUser()
        {
            CheckLoggedIn();
            GetAllUsers();
            Console.WriteLine("Enter the ID of the user to update:");
            if (!int.TryParse(Console.ReadLine(), out int userId)) throw new Exception("Invalid ID.");

            var user = DC.Users.Find(userId);
            if (user == null) throw new Exception("User not found.");

            if (loggedInUser!.Id != userId) throw new Exception("You can only update your own account.");

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("Enter new username (leave blank to keep current):");
            string username = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(username))
            {
                if (DC.Users.Any(u => u.Username == username && u.Id != userId))
                    throw new Exception("Username already taken.");
                user.Username = username;
            }

            Console.WriteLine("Enter new email (leave blank to keep current):");
            string email = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(email))
            {
                if (!email.Contains('@')) throw new Exception("Invalid email.");
                user.Email = email;
            }

            Console.WriteLine("Enter new password (leave blank to keep current):");
            string password = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(password))
            {
                if (password.Length < 4) throw new Exception("Password too short.");
                user.Password = password;
            }

            DC.Users.Update(user);
            DC.SaveChanges();

            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("User updated successfully.");
            Console.ResetColor();
        }

        public void DeleteUser()
        {
            CheckLoggedIn();
            GetAllUsers();
            Console.WriteLine("Enter the ID of the user to delete:");
            if (!int.TryParse(Console.ReadLine(), out int userId)) throw new Exception("Invalid ID.");

            var user = DC.Users.Find(userId);
            if (user == null) throw new Exception("User not found.");

            if (loggedInUser!.Id != userId) throw new Exception("You can only delete your own account.");

            DC.Users.Remove(user);
            DC.SaveChanges();

            loggedInUser = null;

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("User deleted successfully.");
            Console.ResetColor();
        }

        public void SearchUsers()
        {
            CheckLoggedIn();
            Console.WriteLine("Enter username or email to search:");
            string search = (Console.ReadLine() ?? string.Empty).ToLower();

            var users = DC.Users
                .Where(u => u.Username.ToLower().Contains(search) || u.Email.ToLower().Contains(search))
                .ToList();

            Console.ForegroundColor = ConsoleColor.Cyan;
            if (!users.Any())
            {
                Console.WriteLine("No users found.");
            }
            else
            {
                foreach (var user in users)
                {
                    Console.WriteLine(user);
                }
            }
            Console.ResetColor();
        }

        private void CheckLoggedIn()
        {
            if (loggedInUser == null) throw new Exception("You must be logged in to perform this action.");
        }
    }
}