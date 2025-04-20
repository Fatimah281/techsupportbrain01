
using System.Threading.Tasks;
using TechSupportBrain.Models;

namespace TechSupportBrain.Interfaces
{
    public interface IUserService
    {
        Task<bool> Register(User user);
        Task<User> Login(string email, string password);
    }
}