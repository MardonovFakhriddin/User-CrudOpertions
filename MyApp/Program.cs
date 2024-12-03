using System;
using System.Collections.Generic;
using Infrustructure.Models;
using Infrustructure.Service;
using Infrustructure.Service.UserService;
using Npgsql;

class Program
{
    static void Main(string[] args)
    {
        string ConnectionString = "Server=localhost;Username=postgres;Password=LMard1909;Database=postgres";
        Console.Write("Введите имя базы данных: ");
        string databaseName = Console.ReadLine();

        NpgsqlService.CreateDatabase( databaseName);

         ConnectionString = $"Server=localhost;Username=postgres;Password=LMard1909;Database={databaseName}";

        NpgsqlService.CreateTable(databaseName, "create table users " +
                                                "(int id serial primary key," +
                                                " string firstname not null," +
                                                " string lastname not null," +
                                                "int age not null)");

        IUserService userService = new UserService(ConnectionString);

        while (true)
        {
            Console.Clear();
            Console.WriteLine("Операции CRUD с пользователем");
            Console.WriteLine("1. Добавить пользователя");
            Console.WriteLine("2. Получить пользователя по ID");
            Console.WriteLine("3. Обновить пользователя");
            Console.WriteLine("4. Удалить пользователя");
            Console.WriteLine("5. Выход");
            Console.Write("Выберите опцию (1-5): ");
            
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AddUser(userService);
                    break;
                case "2":
                    GetUserById(userService);
                    break;
                case "3":
                    UpdateUser(userService);
                    break;
                case "4":
                    DeleteUser(userService);
                    break;
                case "5":
                    return;
                default:
                    Console.WriteLine("Неверный выбор. Пожалуйста, выберите правильную опцию.");
                    break;
            }

            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }
    }

    static void AddUser(IUserService userService)
    {
        Console.Clear();
        Console.WriteLine("Добавить нового пользователя");

        Console.Write("Введите имя: ");
        string firstName = Console.ReadLine();

        Console.Write("Введите фамилию: ");
        string lastName = Console.ReadLine();

        Console.Write("Введите возраст: ");
        int age = int.Parse(Console.ReadLine());

        User newUser = new User { FirstName = firstName, LastName = lastName, Age = age };

        userService.AddUser(newUser, "users");
        Console.WriteLine("Пользователь успешно добавлен.");
    }

    static void GetUserById(IUserService userService)
    {
        Console.Clear();
        Console.WriteLine("Получить пользователя по ID");

        Console.Write("Введите ID пользователя: ");
        int userId = int.Parse(Console.ReadLine());

        List<User> users = userService.GetUserById(userId, "users");

        if (users.Count > 0)
        {
            Console.WriteLine("Пользователь найден:");
            foreach (var user in users)
            {
                Console.WriteLine($"ID: {user.Id}, Имя: {user.FirstName} {user.LastName}, Возраст: {user.Age}");
            }
        }
        else
        {
            Console.WriteLine("Пользователь не найден.");
        }
    }

    static void UpdateUser(IUserService userService)
    {
        Console.Clear();
        Console.WriteLine("Обновить пользователя");

        Console.Write("Введите ID пользователя для обновления: ");
        int userId = int.Parse(Console.ReadLine());

        List<User> users = userService.GetUserById(userId, "users");

        if (users.Count > 0)
        {
            User user = users[0]; 

            Console.WriteLine($"Текущее имя: {user.FirstName} {user.LastName}, Возраст: {user.Age}");

            Console.Write("Введите новое имя (оставьте пустым, чтобы оставить текущим): ");
            string firstName = Console.ReadLine();
            if (string.IsNullOrEmpty(firstName)) firstName = user.FirstName;

            Console.Write("Введите новую фамилию (оставьте пустым, чтобы оставить текущей): ");
            string lastName = Console.ReadLine();
            if (string.IsNullOrEmpty(lastName)) lastName = user.LastName;

            Console.Write("Введите новый возраст (оставьте пустым, чтобы оставить текущим): ");
            string ageInput = Console.ReadLine();
            int age = string.IsNullOrEmpty(ageInput) ? user.Age : int.Parse(ageInput);

            user.FirstName = firstName;
            user.LastName = lastName;
            user.Age = age;

            userService.UpdateUser(user, "users");
            Console.WriteLine("Пользователь успешно обновлён.");
        }
        else
        {
            Console.WriteLine("Пользователь не найден.");
        }
    }

    static void DeleteUser(IUserService userService)
    {
        Console.Clear();
        Console.WriteLine("Удалить пользователя");

        Console.Write("Введите ID пользователя для удаления: ");
        int userId = int.Parse(Console.ReadLine());

        List<User> users = userService.GetUserById(userId, "users");

        if (users.Count > 0)
        {
            userService.DeleteUser(userId, "users");
            Console.WriteLine("Пользователь успешно удалён.");
        }
        else
        {
            Console.WriteLine("Пользователь не найден.");
        }
    }
}
