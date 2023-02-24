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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<TodoItemModel> GetItemByIdAsync(int id)
        {
            var item = await _context.TodoItems.Include(r=>r.User).Where(r=>r.Id == id).FirstOrDefaultAsync();    
            if (item == null) throw new Exception("Item not found.");


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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
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
            model.Id = await _context.SaveChangesAsync();  
            
             
            model.User = new User
            {
                UserId = user.Id,
                FullName = $"{user.FirstName} {user.LastName}"
            };

            return model;
        }
    }

}
