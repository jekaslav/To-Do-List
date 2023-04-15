using System;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using WebApplication1;
using WebApplication1.Entities;
using WebApplication1.Services;

namespace TestProject1
{
    public class Tests
{
    private DbContext _context;
    private DbService _dbService;
    
    [SetUp]
    public void Setup()
    {
        _context = new DbContext(_context);
        
        _dbService = new DbService(_context);
    }

   [Test]
    public async Task GetTodoItemsAsync_ReturnsTodoItems()
    {
        var todoItems = await _dbService.GetTodoItemsAsync();

        Assert.That(todoItems, Is.Not.Null);
        
        Assert.That(todoItems.Count(), Is.GreaterThan(0));
    }

    [Test]
    public async Task GetTodoItemAsync_ReturnsTodoItem()
    {
        var todoItem = await _dbService.GetTodoItemAsync(1);

        Assert.That(todoItem, Is.Not.Null);
        
        Assert.That(todoItem.Id, Is.EqualTo(1));
    }

    [Test]
    public async Task CreateTodoItemAsync_CreatesNewTodoItem()
    {
        var newTodoItem = new TodoItem
        {
            Name = "New task",
            Description = "New task description",
            IsDone = false
        };

        var createdTodoItem = await _dbService.CreateTodoItemAsync(newTodoItem);

        Assert.That(createdTodoItem, Is.Not.Null);
        Assert.That(createdTodoItem.Id, Is.GreaterThan(0));
        Assert.That(createdTodoItem.Name, Is.EqualTo(newTodoItem.Name));
        Assert.That(createdTodoItem.Description, Is.EqualTo(newTodoItem.Description));
        Assert.That(createdTodoItem.IsDone, Is.EqualTo(newTodoItem.IsDone));
    }

    [Test]
    public async Task UpdateTodoItemAsync_UpdatesTodoItem()
    {
        var todoItem = await _dbService.GetTodoItemAsync(1);

        todoItem.Name = "Updated task";
        todoItem.Description = "Updated task description";
        todoItem.IsDone = true;

        var updatedTodoItem = await _dbService.UpdateTodoItemAsync(1, todoItem);

        Assert.That(updatedTodoItem, Is.Not.Null);
        Assert.That(updatedTodoItem.Id, Is.EqualTo(todoItem.Id));
        Assert.That(updatedTodoItem.Name, Is.EqualTo(todoItem.Name));
        Assert.That(updatedTodoItem.Description, Is.EqualTo(todoItem.Description));
        Assert.That(updatedTodoItem.IsDone, Is.EqualTo(todoItem.IsDone));
    }

    [Test]
    public async Task UpdateTodoItemIsDoneAsync_UpdatesIsDoneField()
    {
        var todoItem = await _dbService.GetTodoItemAsync(1);

        todoItem.IsDone = true;

        var updatedTodoItem = await _dbService.UpdateTodoItemIsDoneAsync(1, todoItem);

        Assert.That(updatedTodoItem, Is.Not.Null);
        Assert.That(updatedTodoItem.Id, Is.EqualTo(todoItem.Id));
        Assert.That(updatedTodoItem.IsDone, Is.EqualTo(todoItem.IsDone));
    }

    [Test]
    public async Task DeleteTodoItemAsync_DeletesTodoItem()
    {
        var rowsAffected = await _dbService.DeleteTodoItemAsync(1);

        Assert.That(rowsAffected, Is.EqualTo(1));
    }
}
}
