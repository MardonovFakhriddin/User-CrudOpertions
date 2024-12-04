using System.Data;
namespace Infrustructure.Service;
using Npgsql;
using Infrustructure.Common;
public static class NpgsqlService
{
    public static bool CreateDatabase(string databaseName)
    {
        try
        {
            using NpgsqlConnection connection = NpgsqlHelper.CreateConnection(SqlCommands.ConnectionString);
            NpgsqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = $"Create database {databaseName};";
            // NpgsqlParameter parameter = new NpgsqlParameter()
            // {
            //     ParameterName = "databaseName",
            //     Value = databaseName,
            //     DbType = DbType.String,
            //     Size = 20,
            //     IsNullable = false,
            // };
            //
            // cmd.Parameters.Add(parameter);
            // cmd.CommandText = SqlCommands.CreateDatabase;
            // cmd.CommandTimeout = 40;
            // cmd.CommandType = CommandType.Text;
            int res = cmd.ExecuteNonQuery();

            return res != 0 ? true : false;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }
    
    public static bool DropDatabase(string databaseName)
    {
        try
        {
            using NpgsqlConnection connection = NpgsqlHelper.CreateConnection(SqlCommands.ConnectionString);
            NpgsqlCommand cmd = connection.CreateCommand();
            NpgsqlParameter parameter = new NpgsqlParameter()
            {
                ParameterName = "databaseName",
                Value = databaseName,
                DbType = DbType.String,
                Size = 20,
                IsNullable = false,
            };

            cmd.Parameters.Add(parameter);
            cmd.CommandText = SqlCommands.DropDatabase;
            cmd.CommandTimeout = 40;
            cmd.CommandType = CommandType.Text;

            int res = cmd.ExecuteNonQuery();

            return res != 0 ? true : false;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    public static bool CreateTable(string databaseName, string sqlCommand)
    {
        try
        {
            string connectionString =
                $"Server = localhost; Port = 5432; Database = {databaseName}; username = postgres; password=LMard1909;";
            using NpgsqlConnection connection = NpgsqlHelper.CreateConnection(connectionString);
            NpgsqlCommand command = connection.CreateCommand();
            command.CommandText = sqlCommand;
            int res = command.ExecuteNonQuery();
            if (res != 0) return true;
            return false;
        }
        catch (Exception e)
        {
            Console.WriteLine("This exception " + e.Message);
            throw;
        }
    }
    
    public static bool DropTable(string databaseName, string tableName)
    {
        try
        {
            string connectionString =
                $"Server = localhost; Port = 5432; Database = {databaseName}; username = postgres; password=LMard1909;";
            using NpgsqlConnection connection = NpgsqlHelper.CreateConnection(connectionString);
            NpgsqlCommand command = connection.CreateCommand();
            command.Parameters.AddWithValue("tableName", tableName);
            command.CommandText = SqlCommands.DropTable;
            int res = command.ExecuteNonQuery();
            if (res != 0) return true;
            return false;
        }
        catch (Exception e)
        {
            Console.WriteLine("This exception " + e.Message);
            throw;
        }
    }
}

file static class SqlCommands
{
    public const string ConnectionString =
        "Server = localhost; Port = 5432; Database = postgres; username = postgres; password=LMard1909;";
    // public const string CreateDatabase = $"Create database {databaseName};";
    public const string DropDatabase = "Drop database @databaseName with(force);";
    public const string DropTable = "Drop table @tableName;";
}