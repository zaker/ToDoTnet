using System.IO;
using Microsoft.AspNetCore.Hosting;
using ToDoTnet.DataEntities;
using System;

namespace ToDoTnet
{
    public class Program
    {
        private static void preLoad()
        {
            using (var db = new ToDoContext())
            {
                db.Users.Add(new User { Name = "Admin", Email = "gm@il.io",Password = "Foo"});
                var count = db.SaveChanges();
                Console.WriteLine("{0} records saved to database", count);

                Console.WriteLine();
                Console.WriteLine("All users in database:");
                foreach (var u in db.Users)
                {
                    Console.WriteLine("{0}({2}) - {1}", u.Name,u.Email,u.UserID);
                }
            }
        }
        public static string connString = "Filename=./todo.db";
        public static void Main(string[] args)
        {
            //connString = File.ReadAllText("conString.txt");
            preLoad();
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
