using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;
using TodoApi.Services;
using TodoApi.DTOs;

namespace TodoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodosController : ControllerBase
    {
        private readonly InMemoryTodoService _svc;
        public TodosController(InMemoryTodoService svc) => _svc = svc;

        [HttpGet]
        public ActionResult<IEnumerable<TodoItem>> Get() => Ok(_svc.GetAll());


        [HttpPost]
        public ActionResult<TodoItem> Post([FromBody] CreateTodoDto dto)
        {

            if (!ModelState.IsValid)
                return BadRequest(new { message = ModelState.Values.SelectMany(v => v.Errors).FirstOrDefault()?.ErrorMessage });

            var created = _svc.Add(dto.Text.Trim());
            if (created == null)
                return Conflict(new { message = "Duplicate todo not allowed." });

            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var removed = _svc.Delete(id);
            return removed ? NoContent() : NotFound();
        }

        [HttpPut]
        public ActionResult<TodoItem> Put([FromBody] UpdateTodoDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = ModelState.Values.SelectMany(v => v.Errors).FirstOrDefault()?.ErrorMessage });

            var updated = _svc.Update(dto.Id, dto.Text.Trim());
            if (updated == null)
                return NotFound(new { message = $"Todo with id {dto.Id} not found." });

            return Ok(updated);
        }
    }
}
