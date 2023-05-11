using SharedProject.DbModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.RepositoryInterfaces
{
    public interface IUserRepo
    {
        public Task<IdentityResult> Create(string username, string password, string email);
        public Task<User> Login(string username, string password);
        public Task<User> GetUserById(string id);
        public Task<User> UpdateUser(string userId, string firstname, string lastname, string oldpassword, string password);
        public Task<ICollection<User>> GetAll();
    }
}
