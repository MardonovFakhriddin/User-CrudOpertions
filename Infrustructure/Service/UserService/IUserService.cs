namespace Infrustructure.Service.UserService;
using Infrustructure.Models;
public interface IUserService
{
    List<User> GetUser(User user);
    User? GetStudentById(int id);
    bool UpdateUser(int id);
    bool CreateUser(User user);
    bool DeleteUser(int id);
}