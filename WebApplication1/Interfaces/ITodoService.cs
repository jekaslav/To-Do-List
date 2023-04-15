using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication1.Entities;

namespace WebApplication1.Interfaces
{
    public interface ITodoService
    {
        Task<IEnumerable<TodoItem>> GetTodoItemsAsync();
        Task<TodoItem> GetTodoItemAsync(int id);
        Task<TodoItem> CreateTodoItemAsync(TodoItem todoItem);
        Task<TodoItem> UpdateTodoItemAsync(int id, TodoItem todoItem);
        Task<TodoItem> UpdateTodoItemIsDoneAsync(int id, TodoItem todoItem);
        Task<int> DeleteTodoItemAsync(int id);
    }
}