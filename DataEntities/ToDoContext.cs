using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoTnet.DataEntities
{

    public class ToDoContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<ToDo> ToDos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(Program.connString);
        }
    }

    public class User
    {
        public User()
        {
            UserID = Guid.NewGuid();

        }
        public Guid UserID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<ToDo> ToDos { get; set; }
    }

    public class ToDo
    {
        public ToDo()
        {
            ToDoID = Guid.NewGuid();

        }
        public Guid ToDoID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string Product { get; set; }

        public Guid UserID { get; set; }
        public User User { get; set; }
    }



}
