using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Entities;
using WebApplication1.Interfaces;

namespace WebApplication1.Controllers
{
    public class TodoItemsController : ControllerBase
    {
        private readonly ITodoService _todoService;

        public TodoItemsController(ITodoService todoService)
        {
            _todoService = todoService;
        }

        [HttpGet("ToDoList")]
        public async Task<ActionResult<IEnumerable<ToDoItemDetails>>> GetTodoItems()
        {
            var todoItems = await _todoService.GetTodoItemsAsync();
            var todoItemDetails = todoItems.Select(ti => new ToDoItemDetails()
            {
                Id = ti.Id,
                Name = ti.Name,
                CreatedDate = ti.CreatedDate,
                IsDone = ti.IsDone
            });

            return Ok(todoItemDetails);
        }

        [HttpGet ("ToDoList/{id}")]
        public async Task<ActionResult<TodoItem>> GetTodoItem(int id)
        {
            var todoItem = await _todoService.GetTodoItemAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return Ok(todoItem);
        }
        
        [HttpPost("ToDoList")]
        public async Task<IActionResult> CreateTodoItem(TodoItem todoItem)
        {
            if (ModelState.IsValid)
            {
                var result = await _todoService.CreateTodoItemAsync(todoItem);
                return Ok(result);
            }

            return BadRequest(ModelState);
        }
        
        [HttpPut("ToDoList/{id}")]
        public async Task<IActionResult> PutTodoItem(int id, TodoItem todoItem)
        {
            if (id != todoItem.Id)
            {
                return BadRequest();
            }

            var editedItem = await _todoService.UpdateTodoItemAsync(id, todoItem);
            
            if (editedItem is null)
            {
                return NotFound();
            }

            return Ok(editedItem);
        }
        
        [HttpPut("ToDoList/IsDone/{id}")]
        public async Task<ActionResult<TodoItem>> UpdateTodoItemIsDone(int id, TodoItem todoItem)
        {
            var updatedTodoItem = await _todoService.UpdateTodoItemIsDoneAsync(id, todoItem);
            if (updatedTodoItem == null)
            {
                return NotFound();
            }
            return updatedTodoItem;
        }

        [HttpDelete ("ToDoList/{id}")]
        public async Task<IActionResult> DeleteTodoItem(int id)
        {
            var rowsAffected = await _todoService.DeleteTodoItemAsync(id);

            if (rowsAffected == 0)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}