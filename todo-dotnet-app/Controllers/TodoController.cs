using Microsoft.AspNetCore.Mvc;
using TodoApp.Models;
using TodoApp.Services;

namespace TodoApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly TodoService _todoService;

        public TodoController(TodoService todoService)
        {
            _todoService = todoService;
        }

        // GET: api/Todo
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var items = await _todoService.GetAllItemsAsync();
                var itemList = new List<TodoItem>();

                // Iterate over the AsyncPageable and collect all the items
                await foreach (var item in items)
                {
                    itemList.Add(item);
                }

                return Ok(itemList); // Return all items in the response
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/Todo/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                var item = await _todoService.GetItemAsync(id);

                if (item == null)
                {
                    return NotFound(); // Item not found
                }

                return Ok(item); // Return the item
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/Todo
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TodoItem newItem)
        {
            if (newItem == null)
            {
                return BadRequest("TodoItem is null");
            }

            try
            {
                await _todoService.AddItemAsync(newItem);
                return CreatedAtAction(nameof(GetById), new { id = newItem.RowKey }, newItem); // Return created item with location header
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: api/Todo/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] TodoItem updatedItem)
        {
            if (updatedItem == null)
            {
                return BadRequest("TodoItem is null");
            }

            try
            {
                var existingItem = await _todoService.GetItemAsync(id);
                if (existingItem == null)
                {
                    return NotFound(); // Item not found
                }

                updatedItem.RowKey = existingItem.RowKey; // Keep the same RowKey
                await _todoService.UpdateItemAsync(updatedItem);
                return NoContent(); // Return 204 No Content for successful update
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE: api/Todo/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var existingItem = await _todoService.GetItemAsync(id);
                if (existingItem == null)
                {
                    return NotFound(); // Item not found
                }

                await _todoService.DeleteItemAsync(id);
                return NoContent(); // Return 204 No Content for successful delete
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
