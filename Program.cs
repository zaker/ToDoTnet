using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace ToDoTnet
{
    public class Program
    {
        public static string connString = "Filename=./todo.db";
        public static void Main(string[] args)
        {
            //connString = File.ReadAllText("conString.txt");
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
