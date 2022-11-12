using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Superwish_FSD04_AppDevII_ASP.NET_Project.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Superwish_FSD04_AppDevII_ASP.NET_Project.Data;

namespace Superwish_FSD04_AppDevII_ASP.NET_Project.Data
{
    public class SeedUsersAndRoles
    {
        public static async Task Initialize
        (ToysDbContext context,
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager)
        {

            string[] rolesNameList = new string[] { "User", "Admin" };
            foreach (string roleName in rolesNameList)
            {
                if (!roleManager.RoleExistsAsync(roleName).Result)
                {
                    IdentityRole role = new IdentityRole();
                    role.Name = roleName;
                    IdentityResult result = roleManager.CreateAsync(role).Result;
                }
            }
            string adminEmail = "admin@admin.com";
            string adminPass = "Admin123!";
            if (userManager.FindByNameAsync(adminEmail).Result == null)
            {
                IdentityUser user = new IdentityUser();
                user.UserName = adminEmail;
                user.EmailConfirmed = true;
                IdentityResult result = userManager.CreateAsync(user, adminPass).Result;

                if (result.Succeeded)
                {
                  IdentityResult result2 = userManager.AddToRoleAsync(user, "Admin").Result;
                  if (!result2.Succeeded){

                  }else{
                    
                  }
                }
            }
        }
    }
}

