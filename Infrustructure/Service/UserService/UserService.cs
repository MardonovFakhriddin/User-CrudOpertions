using Npgsql;
using System;
using System.Collections.Generic;
using Infrustructure.Common;
using Infrustructure.Models;
using Infrustructure.Service.UserService;

public class UserService : IUserService
{
    private readonly string _connectionString;

    public UserService(string connectionString)
    {
        _connectionString = connectionString;
    }

    public void AddUser(User user, string tableName)
    {
        using (var connection = NpgsqlHelper.CreateConnection(_connectionString))
        {
            var command = new NpgsqlCommand($"INSERT INTO {tableName} (first_name, last_name, age) VALUES (@first_name, @last_name, @age)", connection);
            command.Parameters.AddWithValue("first_name", user.FirstName);
            command.Parameters.AddWithValue("last_name", user.LastName);
            command.Parameters.AddWithValue("age", user.Age);
            command.ExecuteNonQuery();
        }
    }

    public void DeleteUser(int id, string tableName)
    {
        using (var connection = NpgsqlHelper.CreateConnection(_connectionString))
        {
            var command = new NpgsqlCommand($"DELETE FROM {tableName} WHERE id = @id", connection);
            command.Parameters.AddWithValue("id", id);
            command.ExecuteNonQuery();
        }
    }

    public List<User> GetUserById(int id, string tableName)
    {
        List<User> users = new List<User>();

        using (var connection = NpgsqlHelper.CreateConnection(_connectionString))
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
        using (var connection = NpgsqlHelper.CreateConnection(_connectionString))
        {
            var command = new NpgsqlCommand
            {
                Connection = connection,
                CommandText = $"UPDATE {tableName} SET first_name = @first_name, last_name = @last_name, age = @age WHERE id = @id"
            };

            command.Parameters.AddWithValue("first_name", user.FirstName);
            command.Parameters.AddWithValue("last_name", user.LastName);
            command.Parameters.AddWithValue("age", user.Age);
            command.Parameters.AddWithValue("id", user.Id);

            command.ExecuteNonQuery();
        }
    }
}
