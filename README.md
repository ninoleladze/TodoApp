# TodoApp

A multi-user console to-do manager built with **C#, .NET 8 and EF Core (SQL Server)**. Each user registers, logs in and manages their own tasks with a priority and a status.

## Structure

- Models/ — User, TodoItem
- Enums/ — Priority, TodoStatus
- Services/ — UserService, TodoService
- Menus/ — user menu and to-do menu
- Data/DataContext.cs — EF Core DbContext

## Run

```bash
dotnet ef database update --project TodoApp1
dotnet run --project TodoApp1
```
