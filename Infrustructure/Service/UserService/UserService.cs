using Npgsql;
using System;
using System.Collections.Generic;
using Infrustructure.Common;
using Infrustructure.Models;
using Infrustructure.Service.UserService;

public class UserService(string connectionString) : IUserService
{
    public void AddUser(User user, string tableName)
    {
        using (var connection = NpgsqlHelper.CreateConnection(connectionString))
        {
            var command = new NpgsqlCommand($"INSERT INTO {tableName} (firstname, lastname, age) VALUES (@firstname, @lastname, @age)", connection);
            command.Parameters.AddWithValue("firstname", user.FirstName);
            command.Parameters.AddWithValue("lastname", user.LastName);
            command.Parameters.AddWithValue("age", user.Age);
            command.ExecuteNonQuery();
        }
    }

    public void DeleteUser(int id, string tableName)
    {
        using (var connection = NpgsqlHelper.CreateConnection(connectionString))
        {
            var command = new NpgsqlCommand($"DELETE FROM {tableName} WHERE id = @id", connection);
            command.Parameters.AddWithValue("id", id);
            command.ExecuteNonQuery();
        }
    }

    public List<User> GetUserById(int id, string tableName)
    {
        List<User> users = new List<User>();

        using (var connection = NpgsqlHelper.CreateConnection(connectionString))
        {
            var command = new NpgsqlCommand($"SELECT * FROM {tableName} WHERE id = @id", connection);
            command.Parameters.AddWithValue("id", id);

            using (var reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        User user = new User()
                        {
                            Id = reader.GetInt32(0),
                            FirstName = reader.GetString(1),
                            LastName = reader.GetString(2),
                            Age = reader.GetInt32(3)
                        };
                        users.Add(user);
                    }
                }
            }
        }

        return users;
    }

    public void UpdateUser(User user, string tableName)
    {
        using (var connection = NpgsqlHelper.CreateConnection(connectionString))
        {
            var command = new NpgsqlCommand
            {
                Connection = connection,
                CommandText = $"UPDATE {tableName} SET firstname = @firstname, lastname = @lastname, age = @age WHERE id = @id"
            };

            command.Parameters.AddWithValue("firstname", user.FirstName);
            command.Parameters.AddWithValue("lastname", user.LastName);
            command.Parameters.AddWithValue("age", user.Age);
            command.Parameters.AddWithValue("id", user.Id);

            command.ExecuteNonQuery();
        }
    }
}
