using SharedProject.DbModels;
using Domain.RepositoryInterfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Repositories
{
    public class UserRepo : IUserRepo
    {
        private readonly DatabaseContext dbcontext;
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;

        public UserRepo(DatabaseContext dbcontext, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            this.dbcontext = dbcontext;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        
        public async Task<IdentityResult> Create(string username, string password, string email)
        {
            User newUser = new User()
            {
                UserName = username,
                Email = email,
            };
            var res = await userManager.CreateAsync(newUser, password);
            await dbcontext.SaveChangesAsync();
            return res;
        }

        public async Task<User> Login(string username, string password)
        {
            /*var res = await Create("BlazorTeszt", "Blazor123", "totokilenc2@gmail.com");
            foreach (var e in res.Errors)
            {
                Console.WriteLine(e.Description);
            }*/
            var user = dbcontext.Users.FirstOrDefault(x => x.UserName == username);
            if (user == null)
                return null;
            var result = await signInManager.CheckPasswordSignInAsync(user, password, false);
            if (result.Succeeded)
                return user;
            return null;
        }

        public async Task<User> GetUserById(string id)
        {
            return dbcontext.Users.FirstOrDefault(x => x.Id == id);
        }

        public async Task<User> UpdateUser(string userId, string firstname, string lastname, string oldpassword, string password)
        {
            var user = dbcontext.Users.FirstOrDefault(x => x.Id == userId);
            if (password != "")
            {
                var asd = await userManager.ChangePasswordAsync(user, oldpassword, password);
            }
            user.FirstName = firstname;
            user.LastName = lastname;
            dbcontext.SaveChanges();

            return user;
        }

        public async Task<ICollection<User>> GetAll()
        {
            return await dbcontext.Users.ToListAsync();
        }
    }
}
