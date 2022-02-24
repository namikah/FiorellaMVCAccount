using FirstFiorellaMVC.DataAccessLayer;
using FirstFiorellaMVC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace FirstFiorellaMVC.Data
{
    public class DataInitializer
    {
        public readonly AppDbContext _dbContext;
        public readonly RoleManager<IdentityRole> _roleManager;
        public readonly UserManager<User> _userManager;

        public DataInitializer(AppDbContext dbContext, RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            _dbContext = dbContext;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task SeedDataAsync()
        {
            await _dbContext.Database.MigrateAsync();

            //await MyCreateRoleListFileAsync();

            var readedJson = File.ReadAllText(@$"{Constants.SeedDataPath}\MyDefaultRoles.json");
            var JsonRoleList = JsonConvert.DeserializeObject<List<string>>(readedJson);

            foreach (var role in JsonRoleList)
            {
                if (await _roleManager.RoleExistsAsync(role))
                    continue;

                await _roleManager.CreateAsync(new IdentityRole(role));
            }

            var user = new User()
            {
                FullName = "Admin",
                UserName = "Admin",
            };

            if (_userManager.FindByNameAsync(user.UserName) != null)
                return;

            await _userManager.CreateAsync(user, "Admin@123");
        }

        //public async Task MyCreateRoleListFileAsync()
        //{
        //    var roles = new List<string>()
        //    {
        //        RoleConstants.AdminRole,
        //        RoleConstants.ModeratorRole,
        //        RoleConstants.UserRole
        //    };

        //    var roleJson = JsonConvert.SerializeObject(roles);
        //    await File.WriteAllTextAsync(@$"{Constants.SeedDataPath}\MyDefaultRoles.json", roleJson);
        //}
    }
}
