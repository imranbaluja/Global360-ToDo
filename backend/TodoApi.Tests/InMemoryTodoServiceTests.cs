using System.Collections.Generic;
using Xunit;
using TodoApi.Services;
using TodoApi.Models;

namespace TodoApi.Tests
{
    public class InMemoryTodoServiceTests
    {
        [Fact]
        public void Add_ShouldAddItem()
        {
            var service = new InMemoryTodoService();
            var todo = service.Add("Test");
            var todos = service.GetAll();
            Assert.NotNull(todo);
            Assert.Contains(todos, t => t.Id == todo.Id && t.Text == "Test");
        }

        [Fact]
        public void GetAll_ShouldReturnAllItems()
        {
            var service = new InMemoryTodoService();
            var todo1 = service.Add("A");
            var todo2 = service.Add("B");
            var todos = service.GetAll().ToList();
            Assert.Equal(2, todos.Count);
            Assert.Contains(todos, t => t.Text == "A");
            Assert.Contains(todos, t => t.Text == "B");
        }

        [Fact]
        public void Delete_ShouldRemoveItem()
        {
            var service = new InMemoryTodoService();
            var todo = service.Add("Test");
            Assert.NotNull(todo);
            var deleted = service.Delete(todo.Id);
            var todos = service.GetAll();
            Assert.True(deleted);
            Assert.DoesNotContain(todos, t => t.Id == todo.Id);
        }
        [Fact]
        public void Update_ShouldModifyItem()
        {
            var service = new InMemoryTodoService();
            var todo = service.Add("Original");
            Assert.NotNull(todo);
            var updated = service.Update(todo.Id, "Updated");
            Assert.NotNull(updated);
            Assert.Equal(todo.Id, updated.Id);
            Assert.Equal("Updated", updated.Text);
            var todos = service.GetAll();
            Assert.Contains(todos, t => t.Id == todo.Id && t.Text == "Updated");
        }
    }
}
