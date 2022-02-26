using JWTAuth.Data.Entities;
using JWTAuthWebAPI.Data;
using JWTAuthWebAPI.Models;
using JWTAuthWebAPI.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace JWTAuthWebAPI.Services
{
    public class TodoItemService : ITodoItemService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public TodoItemService(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IEnumerable<TodoItemModel>> GetAllItemsAsync()
        {
            return await _context.TodoItems.Include(r=>r.User)
                .Select(item => new TodoItemModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    IsComplete = item.IsComplete,
                    User = new User
                    {
                        UserId = item.UserId,
                        FullName = $"{item.User.FirstName} {item.User.LastName}"
                    }
                })
                .ToListAsync();
        }

        public async Task<TodoItemModel> GetItemByIdAsync(int id)
        {
            var item = await _context.TodoItems.FindAsync(id);
            if (item == null) return null;

            return new TodoItemModel
            {
                Id = item.Id,
                Name = item.Name,
                IsComplete = item.IsComplete,
                User = new User
                {
                    UserId = item.UserId,
                    FullName = $"{item.User.FirstName} {item.User.LastName}"
                }               
            };
        }

        public async Task<TodoItemModel> CreateItemAsync(TodoItemModel model)
        {
            var user = await _userManager.FindByIdAsync(model.User.UserId);
            if (user == null) throw new Exception("User not found.");

            var item = new TodoItem
            {
                Name = model.Name,
                IsComplete = model.IsComplete,
                UserId = user.Id 
            };

            _context.TodoItems.Add(item);
            await _context.SaveChangesAsync();

            // Map back to model including User details
            model.Id = item.Id;
            model.User = new User
            {
                UserId = user.Id,
                FullName = $"{user.FirstName} {user.LastName}"
            };

            return model;
        }
    }

}
