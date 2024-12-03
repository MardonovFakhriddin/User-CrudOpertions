namespace Infrustructure.Service.UserService;
using Infrustructure.Models;
public interface IUserService
{
    void AddUser(User user, string tableName);
    void DeleteUser(int id, string tableName);
    List<User> GetUserById(int id, string tableName);
    void UpdateUser(User user, string tableName);
}