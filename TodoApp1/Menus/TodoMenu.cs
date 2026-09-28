using System;
using TodoApp1.Services;

namespace TodoApp1.Menus
{
    internal class TodoMenu
    {
        private TodoService _todoService = new TodoService();

        public void ShowTodoMenu()
        {
            while (true)
            {
                try
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("=== Todo Menu ===");
                    Console.ResetColor();

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("1. Add Todo");
                    Console.ResetColor();

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("2. View All Todos");
                    Console.ResetColor();

                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("3. View My Todos");
                    Console.ResetColor();

                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine("4. Update Todo");
                    Console.ResetColor();

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("5. Delete Todo");
                    Console.ResetColor();

                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.WriteLine("6. Change Status");
                    Console.ResetColor();

                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("7. Search Todos");
                    Console.ResetColor();

                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("8. Back to Main Menu");
                    Console.ResetColor();

                    Console.Write("\nSelect an option: ");

                    if (!int.TryParse(Console.ReadLine(), out int choice))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid input. Please enter a number.");
                        Console.ResetColor();
                        continue;
                    }

                    switch (choice)
                    {
                        case 1:
                            _todoService.CreateTodo();
                            break;
                        case 2:
                            _todoService.GetAllTodos();
                            break;
                        case 3:
                            _todoService.GetMyTodos();
                            break;
                        case 4:
                            _todoService.UpdateTodo();
                            break;
                        case 5:
                            _todoService.DeleteTodo();
                            break;
                        case 6:
                            _todoService.ChangeStatus();
                            break;
                        case 7:
                            _todoService.SearchTodos();
                            break;
                        case 8:
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("Returning to main menu...");
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
    }
}