using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;
using ToDoTnet.Models;
using System.Threading;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ToDoTnet.DataEntities;


namespace ToDoTnet.Logic
{
    public class ToDoUserStore<TUser> : IUserStore<TUser>, IUserPasswordStore<TUser>
        where TUser : ToDoUser, new()
    {
        private ToDoContext _ctx;
        public ToDoUserStore(ToDoContext ctx)
        {
            _ctx = ctx;
        }
        public Task<IdentityResult> CreateAsync(TUser user, CancellationToken cancellationToken)
        {

            var dbUser = new User
            {
                Name = user.UserName,
                Email = user.Email,
                Password = user.Password
            };

            _ctx.Users.Add(dbUser);
            _ctx.SaveChanges();

            return Task.FromResult(IdentityResult.Success);
        }


        public Task<IdentityResult> DeleteAsync(TUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<TUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            // if your users set name is Users
            var dbUser = await (from u in _ctx.Users
                                where u.UserID.ToString() == userId
                                select u).FirstOrDefaultAsync();
            if (dbUser == null) return null;
            TUser user = new TUser();
            user.AttachDbEntity(dbUser);
            return user;

        }

        public async Task<TUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {

            // if your users set name is Users
            var dbUser = await (from u in _ctx.Users
                                where u.Name.ToUpper() == normalizedUserName.ToUpper()
                                select u).FirstOrDefaultAsync();
            if (dbUser == null) return null;
            TUser user = new TUser();
            user.AttachDbEntity(dbUser);
            return user;

        }

        public Task<string> GetNormalizedUserNameAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        public Task<string> GetUserIdAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id.ToString());
        }

        public Task<string> GetUserNameAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        public Task SetNormalizedUserNameAsync(TUser userIn, string normalizedName, CancellationToken cancellationToken)
        {
            userIn.NormalizedUserName = normalizedName;

            return Task.CompletedTask;
        }

        public Task SetUserNameAsync(TUser userIn, string userName, CancellationToken cancellationToken)
        {
            // if your users set name is Users
            var dbUser = _ctx.Users.First(u => u.UserID == Guid.Parse(userIn.Id)).Name = userName;

            return Task.FromResult(_ctx.SaveChanges());

        }

        public Task<IdentityResult> UpdateAsync(TUser userIn, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _ctx.SaveChanges();
                }

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            Dispose(true);

        }

        public async Task SetPasswordHashAsync(TUser userIn, string passwordHash, CancellationToken cancellationToken)
        {
            
            var dbUser = await FindByNameAsync(userIn.UserName, cancellationToken);

            if (dbUser == null)
            {
                return;
            }
            
            dbUser.PasswordHash = passwordHash;


            await _ctx.SaveChangesAsync();



            return;
        }

        public Task<string> GetPasswordHashAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Password);
        }

        public Task<bool> HasPasswordAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Password.Length > 0);
        }
        #endregion
    }
}
