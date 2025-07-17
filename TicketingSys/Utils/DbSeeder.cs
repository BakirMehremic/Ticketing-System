using TicketingSys.Models;
using TicketingSys.Settings;

namespace TicketingSys.Utils;

public class DbSeeder
{
    public static void Seed(ApplicationDbContext db, IConfiguration config, ILogger logger)
    {
        string userId = config["SEED_ADMIN_USERID"] ?? "NO ID IN ENV";
        string adminEmail = config["SEED_ADMIN_EMAIL"] ?? "ERROR";
        string adminFirstName = config["SEED_ADMIN_FIRSTNAME"] ?? "ERROR ";
        string adminLastName = config["SEED_ADMIN_LASTNAME"] ?? "User ";

        logger.LogInformation("Seeding admin user");

        List<User> existing = db.Users.ToList();
        if (existing.Any())
        {
            logger.LogWarning("REMOVING EXISTING USERS");
            db.Users.RemoveRange(existing);
            db.SaveChanges();
        }

        if (!db.Users.Any())
        {
            User adminUser = new()
            {
                Id = userId,
                FirstName = adminFirstName,
                LastName = adminLastName,
                Email = adminEmail,
                IsAdmin = true
            };

            db.Users.Add(adminUser);
            db.SaveChanges();
        }
    }
}