using API.Models;

namespace API.Interfaces
{
    public interface IUserRepo
    {
        Task<List<Users>> GetAllAsync();
        Task<Users?> GetByIdAsync(int id);
        Task<Users> CreateAsync(Users userModel);
        Task<Users?> UpdateAsync(int id, Users userModel);
        Task<Users?> DeleteAsync(int id);
    }
}
