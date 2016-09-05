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

            _ctx.Users.Add(new User
            {
                Name = user.UserName,
                Email = user.Email,
                Password = user.PasswordHash
            });
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
            TUser user = new TUser();
            user.AttachDbEntity(dbUser);
            return user;

        }

        public async Task<TUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {

            // if your users set name is Users
            var dbUser = await (from u in _ctx.Users
                                where u.Name.ToUpper() == normalizedUserName
                                select u).FirstOrDefaultAsync();
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

        public Task SetNormalizedUserNameAsync(TUser user, string normalizedName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
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
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~UnaUserStore() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }

        public async Task SetPasswordHashAsync(TUser userIn, string passwordHash, CancellationToken cancellationToken)
        {

            // if your users set name is Users
            var dbUser = await FindByIdAsync(userIn.Id, cancellationToken);

            if (dbUser == null)
            {
                await CreateAsync(dbUser, cancellationToken);
            }
            dbUser.Password = passwordHash;


            await _ctx.SaveChangesAsync();



            return;
        }

        public Task<string> GetPasswordHashAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash.Length > 0);
        }
        #endregion
    }
}
