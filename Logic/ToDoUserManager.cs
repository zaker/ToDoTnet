using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;

using System.Linq;
using Microsoft.EntityFrameworkCore;
using ToDoTnet.DataEntities;

namespace ToDoTnet.Models
{
    public class ToDoUserManager : UserManager<ToDoUser>

    {
        private readonly ToDoContext db;

        public ToDoUserManager(IUserStore<ToDoUser> store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<ToDoUser> passwordHasher,
            IEnumerable<IUserValidator<ToDoUser>> userValidators,
            IEnumerable<IPasswordValidator<ToDoUser>> passwordValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            IServiceProvider services,
            ILogger<UserManager<ToDoUser>> logger,
            ToDoContext db) :
            base(store,
                optionsAccessor,
                passwordHasher,
                userValidators,
                passwordValidators,
                keyNormalizer,
                errors,
                services,
                logger)
        {
            this.db = db;
        }


        public override async Task<ToDoUser> FindByNameAsync(string userName)
        {
            using (db) // use your DbConext
            {
                // Fetch - again - your user from the DB with the Id.
                var user = await (from u in db.Users
                                  where u.Name == userName
                                  select u).FirstOrDefaultAsync();

                if (user == null) return null;
                return new ToDoUser(user);
            }




        }

        public override async Task<ToDoUser> FindByIdAsync(string userIdStr)
        {
            Guid userId;
            if (!Guid.TryParse(userIdStr, out userId))
            {
                return null;
            }
            using (db ) // use your DbConext
            {
                // Fetch - again - your user from the DB with the Id.
                var user = await (from u in db.Users
                                  where u.UserID == userId
                                  select new ToDoUser(u)).FirstOrDefaultAsync();
                return user;
            }
        }


    }
}