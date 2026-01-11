using TodoApi.Controllers;
using TodoApi.Services;
using TodoApi.Models;
using Microsoft.AspNetCore.Mvc;
using TodoApi.DTOs;
using Xunit;

namespace TodoApi.Tests
{

    public class TodoControllerTests
    {

        [Fact]
        public void Put_NonExistingItem_ReturnsNotFound()
        {
            var svc = new InMemoryTodoService();
            var controller = new TodosController(svc);
            var updateDto = new UpdateTodoDto { Id = 999, Text = "updated" };
            // Manually trigger model validation
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(updateDto);
            var validationResults = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            System.ComponentModel.DataAnnotations.Validator.TryValidateObject(updateDto, validationContext, validationResults, true);
            foreach (var validationResult in validationResults)
            {
                controller.ModelState.AddModelError("Text", validationResult.ErrorMessage ?? "Validation error");
            }
            var putResult = controller.Put(updateDto);
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(putResult.Result);
            var errorObj = notFoundResult.Value;
            Assert.NotNull(errorObj);
            var messageProp = errorObj.GetType().GetProperty("message");
            var message = messageProp?.GetValue(errorObj)?.ToString();
            Assert.Equal("Todo with id 999 not found.", message);
        }

        [Fact]
        public void Post_NonAlphanumericText_ReturnsBadRequest()
        {
            var svc = new InMemoryTodoService();
            var controller = new TodosController(svc);
            var dto = new CreateTodoDto { Text = "invalid!@#" };
            // Manually trigger model validation
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(dto);
            var validationResults = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            System.ComponentModel.DataAnnotations.Validator.TryValidateObject(dto, validationContext, validationResults, true);
            foreach (var validationResult in validationResults)
            {
                controller.ModelState.AddModelError("Text", validationResult.ErrorMessage ?? "Validation error");
            }
            var result = controller.Post(dto);
            var badRequest = Assert.IsType<BadRequestObjectResult>(result.Result);
            var errorObj = badRequest.Value;
            Assert.NotNull(errorObj);
            var messageProp = errorObj.GetType().GetProperty("message");
            var message = messageProp?.GetValue(errorObj)?.ToString();
            Assert.Equal("Text can only contain letters, numbers, and spaces.", message);
        }
        [Fact]
        public void Post_AddsItem_AndGetReturnsIt()
        {
            var svc = new InMemoryTodoService();
            var controller = new TodosController(svc);

            var dto = new CreateTodoDto { Text = "test item" };
            var postResult = controller.Post(dto);
            var getResult = controller.Get();

            var postActionResult = Assert.IsType<CreatedAtActionResult>(postResult.Result);
            var createdItem = Assert.IsType<TodoItem>(postActionResult.Value);
            Assert.Equal("test item", createdItem.Text);

            var getActionResult = Assert.IsType<OkObjectResult>(getResult.Result);
            var items = Assert.IsAssignableFrom<IEnumerable<TodoItem>>(getActionResult.Value);
            Assert.Contains(items, i => i.Text == "test item");
        }

        [Fact]
        public void Post_EmptyText_ReturnsBadRequest()
        {
            var svc = new InMemoryTodoService();
            var controller = new TodosController(svc);
            var dto = new CreateTodoDto { Text = "" };
            // Manually trigger model validation
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(dto);
            var validationResults = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            System.ComponentModel.DataAnnotations.Validator.TryValidateObject(dto, validationContext, validationResults, true);
            foreach (var validationResult in validationResults)
            {
                controller.ModelState.AddModelError("Text", validationResult.ErrorMessage ?? "Validation error");
            }
            var result = controller.Post(dto);
            var badRequest = Assert.IsType<BadRequestObjectResult>(result.Result);
            var errorObj = badRequest.Value;
            Assert.NotNull(errorObj);
            var messageProp = errorObj.GetType().GetProperty("message");
            var message = messageProp?.GetValue(errorObj)?.ToString();
            Assert.Equal("Text is required.", message);
        }

        [Fact]
        public void Post_DuplicateTodo_ReturnsConflict()
        {
            var svc = new InMemoryTodoService();
            var controller = new TodosController(svc);
            var dto1 = new CreateTodoDto { Text = "duplicate" };
            var dto2 = new CreateTodoDto { Text = "duplicate" };
            controller.Post(dto1);
            var result = controller.Post(dto2);
            var conflict = Assert.IsType<ConflictObjectResult>(result.Result);
            var errorObj = conflict.Value;
            Assert.NotNull(errorObj);
            var messageProp = errorObj.GetType().GetProperty("message");
            var message = messageProp?.GetValue(errorObj)?.ToString();
            Assert.Equal("Duplicate todo not allowed.", message);
        }

        [Fact]
        public void Delete_ExistingItem_RemovesItem()
        {
            var svc = new InMemoryTodoService();
            var controller = new TodosController(svc);
            var dto = new CreateTodoDto { Text = "to delete" };
            var postResult = controller.Post(dto);
            var created = Assert.IsType<CreatedAtActionResult>(postResult.Result).Value as TodoItem;
            Assert.NotNull(created);
            var deleteResult = controller.Delete(created.Id);
            Assert.IsType<NoContentResult>(deleteResult);
            var getResult = controller.Get();
            var okResult = Assert.IsType<OkObjectResult>(getResult.Result);
            Assert.NotNull(okResult.Value);
            var items = Assert.IsAssignableFrom<IEnumerable<TodoItem>>(okResult.Value);
            Assert.DoesNotContain(items, i => i.Id == created.Id);
        }

        [Fact]
        public void Delete_NonExistingItem_ReturnsNotFound()
        {
            var svc = new InMemoryTodoService();
            var controller = new TodosController(svc);
            var result = controller.Delete(999);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Get_ReturnsAllItems()
        {
            var svc = new InMemoryTodoService();
            var controller = new TodosController(svc);
            controller.Post(new CreateTodoDto { Text = "item 1" });
            controller.Post(new CreateTodoDto { Text = "item 2" });
            var result = controller.Get();
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var items = Assert.IsAssignableFrom<IEnumerable<TodoItem>>(okResult.Value);
            Assert.Contains(items, i => i.Text == "item 1");
            Assert.Contains(items, i => i.Text == "item 2");
        }
        [Fact]
        public void Put_ExistingItem_UpdatesText()
        {
            var svc = new InMemoryTodoService();
            var controller = new TodosController(svc);
            var createDto = new CreateTodoDto { Text = "original" };
            var postResult = controller.Post(createDto);
            var created = Assert.IsType<CreatedAtActionResult>(postResult.Result).Value as TodoItem;
            Assert.NotNull(created);

            var updateDto = new UpdateTodoDto { Id = created.Id, Text = "updated" };
            // Manually trigger model validation
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(updateDto);
            var validationResults = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            System.ComponentModel.DataAnnotations.Validator.TryValidateObject(updateDto, validationContext, validationResults, true);
            foreach (var validationResult in validationResults)
            {
                controller.ModelState.AddModelError("Text", validationResult.ErrorMessage ?? "Validation error");
            }
            var putResult = controller.Put(updateDto);
            var okResult = Assert.IsType<OkObjectResult>(putResult.Result);
            var updatedItem = Assert.IsType<TodoItem>(okResult.Value);
            Assert.Equal("updated", updatedItem.Text);
        }

    }
}
