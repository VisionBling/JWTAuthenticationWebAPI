using JWTAuth.Data.Entities;
using JWTAuthWebAPI.Models;

namespace JWTAuthWebAPI.Services.Interfaces
{
    public interface ITodoItemService
    {
        Task<IEnumerable<TodoItemModel>> GetAllItemsAsync();
        Task<TodoItemModel> GetItemByIdAsync(int id);
        Task<TodoItemModel> CreateItemAsync(TodoItemModel item);
    }
}
