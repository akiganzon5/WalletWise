namespace FinanceAPI.Services
{
    using FinanceAPI.Data;
    using global::FinanceAPI.Model;
    using Microsoft.AspNetCore.Identity;

    namespace FinanceAPI.Data
    {
        public static class UserSeeder
        {
            public static async Task SeedUsersAsync(IServiceProvider serviceProvider)
            {
                var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();

                await CreateUser(userManager, "Tester1", "admin@gmail.com", "Tester1!");
                await CreateUser(userManager, "Tester2", "employee@gmail.com", "Tester12");
            }

            private static async Task CreateUser(
                UserManager<AppUser> userManager,
                string username,
                string email,
                string password
            )
            {
                if (await userManager.FindByEmailAsync(email) == null)
                {
                    var user = new AppUser
                    {
                        Email = email,
                        EmailConfirmed = true,
                        UserName = username
                    };

                    var result = await userManager.CreateAsync(user, password);

                    if (!result.Succeeded)
                    {
                        throw new Exception($"Failed to create user with email {email}. Errors: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                    }
                }
            }
        }
    }

}
