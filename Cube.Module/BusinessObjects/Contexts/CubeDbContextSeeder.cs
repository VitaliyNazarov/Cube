using Cube.Model.Helpers;
using Cube.Model.Security;

namespace Cube.Model.Contexts
{
    internal static class CubeDbContextSeeder
    {
        public static void Seed(CubeDbContext context)
        {
            var password = "Cube";
            var systemPassword = "Fag13720";

            var adminRole = context.Roles.Add(new Role
            {
                Name = "Админитратор",
                Type = RoleType.Administrator
            });
            context.Roles.Add(new Role
            {
                Name = "Дизайнер",
                Type = RoleType.Designer
            });
            var admin = context.Users.Add(new User
            {
                Name = "Администратор",
                Login = "Admin",
                IsArchive = false,
                Password = HashBuilder.CreateSha256(password.EncodeToUtf8Bytes()).EncodeBase64()
            });
            var system = context.Users.Add(new User
            {
                Name = "System",
                Login = "System",
                IsArchive = false,
                Password = HashBuilder.CreateSha256(systemPassword.EncodeToUtf8Bytes()).EncodeBase64()
            });
            adminRole.Users.Add(admin);
            adminRole.Users.Add(system);

            context.PriceLists.Add(new PriceList {Name = "Основной прайс-лист"});

            context.SaveChanges();
        }
    }
}