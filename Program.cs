using System.IO;
using Microsoft.AspNetCore.Hosting;
using ToDoTnet.DataEntities;
using System;
using System.Linq;

namespace ToDoTnet
{
    public class Program
    {
        private static void createAdmin()
        {
            using (var db = new ToDoContext())
            {
                if (db.Users.FirstOrDefault(u => u.Name == "Admin") != null) return;
                
                var admin = new User { Name = "Admin", Email = "gm@il.io", Password = "Secret1" };
                db.Users.Add(admin);

                var count = db.SaveChanges();
                var firstTask = new ToDo
                {
                    Title = "Created Admin",
                    Description = "Created admin \n Only to populate database with something",
                    UserID = admin.UserID,
                    Product = "ToDoTnet",
                    Type = "System Task"
                };
                
                db.ToDos.Add(firstTask);


                count += db.SaveChanges();
                Console.WriteLine("{0} records saved to database", count);

                Console.WriteLine();
                Console.WriteLine("All users in database:");
                foreach (var u in db.Users)
                {
                    Console.WriteLine("{0}({2}) - {1}", u.Name, u.Email, u.UserID);
                }
            }
        }
        public static string connString = "Filename=./todo.db";
        public static void Main(string[] args)
        {
            //connString = File.ReadAllText("conString.txt");
            createAdmin();
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                .UseUrls("http://*:5000")
                .Build();

            host.Run();
        }
    }
}
