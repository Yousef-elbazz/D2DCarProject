using DALProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALProject.Data.Seeding
{
    public static class CarAppDbContextSeed
    {
        public static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }
            if (!await roleManager.RoleExistsAsync("Customer"))
            {
                await roleManager.CreateAsync(new IdentityRole("Customer"));
            }
            if (!await roleManager.RoleExistsAsync("Technician"))
            {
                await roleManager.CreateAsync(new IdentityRole("Technician"));
            }
            if (!await roleManager.RoleExistsAsync("Driver"))
            {
                await roleManager.CreateAsync(new IdentityRole("Driver"));
            }


        }

        public static async Task SeedUser(UserManager<AppUser> userManager)
        {
            if(!userManager.Users.Any()) 
            {
                var admin = new AppUser { 
                
                  UserName = "admin",
                  Email = "Yousef@gmail.com",
                  Name = "Yousef"
                
                
                
                
                };
               
              var result =  await userManager.CreateAsync(admin,"111AAAaaa@");
                if (result.Succeeded)
                {
                   await  userManager.AddToRoleAsync(admin, "Admin");
                
                
                }
            
            }
            else if (userManager.Users.Any(e=>e.Email == "Yousef@gmail.com")) {
                var admin = userManager.Users.Where(e => e.Email == "Yousef@gmail.com").FirstOrDefault();
                if (admin != null)
                {
					var token = await userManager.GeneratePasswordResetTokenAsync(admin);
					await userManager.ResetPasswordAsync(admin, token, "111AAAaaa##");
				}
            
            }

        }

    }
}
