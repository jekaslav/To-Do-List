using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication1.Entities;
using WebApplication1.Interfaces;
using Microsoft.Data.SqlClient;
using Dapper;


namespace WebApplication1.Services
{
    public class TodoService : ITodoService
{
    private readonly DbContext _context;

    public TodoService(DbContext context)
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
        
        var query = "SELECT * FROM TodoItems WHERE Id = @Id";
            
        var parameters = new { Id = id };

        var todoItem = await connection.QuerySingleOrDefaultAsync<TodoItem>(query, parameters);

        return todoItem;
    }

    public async Task<TodoItem> CreateTodoItemAsync(TodoItem todoItem)
    {
        var query =
            "INSERT INTO TodoItems (Name, Description, CreatedDate, IsDone) VALUES (@Name, @Description, @CreatedDate, @IsDone);";
        
        var parameters = new DynamicParameters();
        
        parameters.Add("Name", todoItem.Name);
        parameters.Add("Description", todoItem.Description);
        parameters.Add("CreatedDate", DateTime.Now.Date);
        parameters.Add("IsDone", todoItem.IsDone);

        using var connection = _context.CreateConnection();
        var id = await connection.ExecuteAsync(query, parameters);
        
        var createdItem = new TodoItem
        {
            Name = todoItem.Name,
            Description = todoItem.Description,
            CreatedDate = DateTime.Now.Date,
            IsDone = todoItem.IsDone
        };
        return createdItem;
    }

    public async Task<TodoItem> UpdateTodoItemAsync(int id, TodoItem todoItem)
    {
        var query =
            "UPDATE TodoItems SET Name = @Name, Description = @Description, IsDone = @IsDone WHERE Id = @Id";
        
        var parameters = new DynamicParameters();
        
        parameters.Add("Name", todoItem.Name);
        parameters.Add("Description", todoItem.Description);
        
        using var connection = _context.CreateConnection();

        await connection.ExecuteAsync(query, todoItem);
        var editedItem = new TodoItem
        {
            Name = todoItem.Name,
            Description = todoItem.Description,
        };

        return editedItem;
    }

    public async Task<TodoItem> UpdateTodoItemIsDoneAsync(int id, TodoItem todoItem)
    {
        using var connection = _context.CreateConnection();
        
        var query =
            "UPDATE TodoItems SET IsDone = @IsDone WHERE Id = @Id";
        
        var parameters = new DynamicParameters();
        
        parameters.Add("IsDone", todoItem.IsDone);
        
        await connection.ExecuteAsync(query, parameters);
        
        return await GetTodoItemAsync(id);

    }

    public async Task<int> DeleteTodoItemAsync(int id)
    {
        using var connection = _context.CreateConnection();
        
        var query = "DELETE FROM TodoItems WHERE Id = @Id";
        
        var parameters = new { Id = id };

        var rowsAffected = await connection.ExecuteAsync(query, parameters);

        return rowsAffected;
    }
    
}
}