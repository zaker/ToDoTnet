using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using static ToDoTnet.DataEntities.ToDoContext;
using ToDoTnet.DataEntities;

namespace ToDoTnet.Models
{
    public class ToDoUser : IdentityUser , IAttachDbEntity<User>
    {
        private User _dbUser;

        public ToDoUser()
        {
            _dbUser = null;
        }
        public ToDoUser(User dbUser)
        {
            _dbUser = dbUser;
        }
        public void AttachDbEntity(User ent)
        {



            if (_dbUser != null)
                return;

            _dbUser = ent;
        }


        public override string UserName
        {
            get
            {
                return base.UserName;
            }

            set
            {
                base.UserName = value;
                base.NormalizedUserName = value.ToUpper();
            }
        }

        
        public string Password
        {
            set
            {
                PasswordHasher<ToDoUser> hasher = new PasswordHasher<ToDoUser>();
                this.PasswordHash = hasher.HashPassword(this, value);
                
            }
            get
            {
                return _dbUser.Password;
            }
        }

        public override string Id
        {
            get
            {
                return _dbUser.UserID.ToString();
            }

            set
            {
                return;
            }
        }

        public override string PasswordHash
        {
            get
            {
                return "DDD9";
            }

            set
            {
                base.PasswordHash = value;
            }
        }


    }
}