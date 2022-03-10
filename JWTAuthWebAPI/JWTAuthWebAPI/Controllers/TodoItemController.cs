using JWTAuth.Data.Entities;
using JWTAuthWebAPI.Models;
using JWTAuthWebAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JWTAuthWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TodoItemController : ControllerBase
    {
        private readonly ITodoItemService _todoItemService;

        public TodoItemController(ITodoItemService todoItemService)
        {
            _todoItemService = todoItemService;
        }
        // GET: api/TodoItem
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin,user")]
        public async Task<ActionResult<IEnumerable<TodoItemModel>>> GetTodoItems()
        {
            return Ok(await _todoItemService.GetAllItemsAsync());
        }

        // GET: api/TodoItem/5
        [HttpGet("{id}")]
        [Authorize(Roles = "admin,user")] // Accessible by both admin and user
        public async Task<ActionResult<TodoItemModel>> GetTodoItem(int id)
        {
            var todoItem = await _todoItemService.GetItemByIdAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return todoItem;
        }

        [HttpPost]
        [Authorize(Roles = "admin")] // Restricted to admin
        public async Task<ActionResult<TodoItemModel>> PostTodoItem([FromBody] TodoItemModel item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdItem = await _todoItemService.CreateItemAsync(item);
            return CreatedAtAction(nameof(GetTodoItem), new { id = createdItem.Id }, createdItem);
        }
    }
}
