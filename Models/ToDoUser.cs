﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using ToDoTnet.DataEntities;

namespace ToDoTnet.Models
{
    public class ToDoUser : IdentityUser , IAttachDbEntity<User>
    {
        private User _dbUser;

        public ToDoUser()
        {
            _dbUser = new User();
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
                return _dbUser.Name;
            }

            set
            {
                _dbUser.Name = value;
                base.NormalizedUserName = value.ToUpper();
            }
        }

        
        public string Password
        {
            set
            {
                PasswordHasher<ToDoUser> hasher = new PasswordHasher<ToDoUser>();
                _dbUser.Password = hasher.HashPassword(this, value);
                
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


        }

        //public override string PasswordHash
        //{
        //    get
        //    {
        //        return _dbUser.Password;
        //    }

        //    set
        //    {
        //        base.PasswordHash = value;
        //    }
        //}


    }
}