using System.Text.Json;
using auth_service.Models;

namespace auth_service.Data;

public class Seed
{
    /// <summary>
    /// Seed the database with examples models in the json files if the database is empty.
    /// </summary>
    /// <param name="context">Database Context </param>
    public static void SeedData(DataContext context)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        CallEachSeeder(context, options);
    }
    
    /// <summary>
    /// Centralize the call to each seeder method
    /// </summary>
    /// <param name="context">Database context</param>
    /// <param name="options">Options to deserialize json</param>
    public static void CallEachSeeder(DataContext context, JsonSerializerOptions options)
    {
        SeedFirstOrderTables(context, options);
        SeedSecondOrderTables(context, options);
    }
    
    /// <summary>
    /// Seed the database with the tables that don't depend on other tables.
    /// </summary>
    /// <param name="context">Database context</param>
    /// <param name="options">Options to deserialize json</param>
    private static void SeedFirstOrderTables(DataContext context, JsonSerializerOptions options)
    {
        SeedRoles(context, options);
    }
    
    /// <summary>
    /// Seed the database with the tables that depend on first-order tables.
    /// </summary>
    /// <param name="context">Database context</param>
    /// <param name="options">Options to deserialize JSON</param>
    private static void SeedSecondOrderTables(DataContext context, JsonSerializerOptions options)
    {
        SeedUsers(context, options);
    }
    
    /// <summary>
    /// Seed the database with the roles in the json file and save changes if the database is empty.
    /// </summary>
    /// <param name="context">Database context</param>
    /// <param name="options">Options to deserialize json</param>
    private static void SeedRoles(DataContext context, JsonSerializerOptions options)
    {
        var result = context.Roles?.Any();
        if (result is true or null) return;

        var path = "Src/Data/DataSeeders/RolesData.json";
        var rolesData = File.ReadAllText(path);
        var rolesList = JsonSerializer.Deserialize<List<Role>>(rolesData, options) ??
                        throw new Exception("RolesData.json is empty");

        context.Roles?.AddRange(rolesList);
        context.SaveChanges();
    }
    
    
    /// <summary>
    /// Seed the database with users from the JSON file.
    /// </summary>
    /// <param name="context">Database context</param>
    /// <param name="options">Options to deserialize JSON</param>
    private static void SeedUsers(DataContext context, JsonSerializerOptions options)
    {
        var result = context.Users?.Any();
        if (result is true or null) return;

        var path = "Src/Data/DataSeeders/UsersData.json";
        var usersData = File.ReadAllText(path);
        var usersList = JsonSerializer.Deserialize<List<User>>(usersData, options) ??
                        throw new Exception("UsersData.json is empty");
        
        // Hashear la contraseña de cada usuario antes de guardarlo
        foreach (var user in usersList)
        {
            user.HashedPassword = BCrypt.Net.BCrypt.HashPassword(user.HashedPassword);
        }

        context.Users?.AddRange(usersList);
        context.SaveChanges();
    }
}