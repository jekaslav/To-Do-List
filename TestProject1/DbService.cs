using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using WebApplication1;
using WebApplication1.Entities;
using WebApplication1.Interfaces;

namespace TestProject1
{
    public class DbService : ITodoService
{
    private readonly DbContext _context;

    public DbService(DbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TodoItem>> GetTodoItemsAsync()
    {
        using var connection = _context.CreateConnection();
        
        var todoItems = await connection.QueryAsync<TodoItem>("SELECT * FROM TodoItems");
        
        return todoItems;
    }

    public async Task<TodoItem> GetTodoItemAsync(int id)
    {
        using var connection = _context.CreateConnection();
        
        var todoItem = await connection.QuerySingleOrDefaultAsync<TodoItem>("SELECT  FROM TodoItems WHERE Id = @Id", new { Id = id });
        
        return todoItem;
    }

    public async Task<TodoItem> CreateTodoItemAsync(TodoItem todoItem)
    {
        using var connection = _context.CreateConnection();
        
        var result = await connection.QuerySingleOrDefaultAsync<int>("INSERT INTO TodoItems (Name, Description, IsDone) VALUES (@Name, @Description, @IsDone); SELECT CAST(SCOPE_IDENTITY() as int)", todoItem);
        
        todoItem.Id = result;
        
        return todoItem;
    }

    public async Task<TodoItem> UpdateTodoItemAsync(int id, TodoItem todoItem)
    {
        using var connection = _context.CreateConnection();
        
        await connection.ExecuteAsync("UPDATE TodoItems SET Name = @Name, Description = @Description, IsDone = @IsDone WHERE Id = @Id", new { Id = id, Name = todoItem.Name, Description = todoItem.Description, IsDone = todoItem.IsDone });
        
        var updatedTodoItem = await GetTodoItemAsync(id);
        
        return updatedTodoItem;
    }

    public async Task<TodoItem> UpdateTodoItemIsDoneAsync(int id, TodoItem todoItem)
    {
        using var connection = _context.CreateConnection();
        
        await connection.ExecuteAsync("UPDATE TodoItems SET IsDone = @IsDone WHERE Id = @Id", new { Id = id, IsDone = todoItem.IsDone });
        
        var updatedTodoItem = await GetTodoItemAsync(id);
        
        return updatedTodoItem;
    }

    public async Task<int> DeleteTodoItemAsync(int id)
    {
        using (var connection = _context.CreateConnection())
        {
            var rowsAffected = await connection.ExecuteAsync("DELETE FROM TodoItems WHERE Id = @Id", new { Id = id });
            
            return rowsAffected;
        }
    }
}

}