using System;
using TodoApp1.Data;
using TodoApp1.Menus;
using TodoApp1.Services;

namespace TodoApp1
{
    internal static class Program
    {
        private static readonly UserService _userService = new UserService();
        private static readonly TodoMenu _todoMenu = new TodoMenu();
        private static readonly UserMenu _userMenu = new UserMenu();
        private static readonly DataContext DC = new DataContext();

        private static void Main()
        {
            while (true)
            {
                try
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("=== Todo App ===");
                    Console.ResetColor();

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("1. Register");
                    Console.ResetColor();

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("2. Login");
                    Console.ResetColor();

                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("3. Logout");
                    Console.ResetColor();

                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine("4. Users (list/update/delete/search)");
                    Console.ResetColor();

                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("5. Todo Menu");
                    Console.ResetColor();

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("6. Exit");
                    Console.ResetColor();

                    Console.Write("\nSelect an option: ");
                    if (!int.TryParse(Console.ReadLine(), out int choice))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid input. Please enter a number.");
                        Console.ResetColor();
                        Console.WriteLine("Press Enter to continue...");
                        Console.ReadLine();
                        continue;
                    }

                    switch (choice)
                    {
                        case 1:
                            _user_service_register();
                            break;
                        case 2:
                            _userService.Login();
                            break;
                        case 3:
                            _userService.Logout();
                            break;
                        case 4:
                            _userMenu.ShowUserMenu();
                            break;
                        case 5:
                            _todoMenu.ShowTodoMenu();
                            break;
                        case 6:
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("Goodbye!");
                            Console.ResetColor();
                            return;
                        default:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Invalid option. Please try again.");
                            Console.ResetColor();
                            break;
                    }

                    Console.WriteLine("Press Enter to continue...");
                    Console.ReadLine();
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Error: {ex.Message}");
                    Console.ResetColor();
                    Console.WriteLine("Press Enter to continue...");
                    Console.ReadLine();
                }
            }
        }

        private static void _user_service_register()
        {
            try
            {
                _userService.RegisterUser();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error: {ex.Message}");
                Console.ResetColor();
            }
        }
    }
}