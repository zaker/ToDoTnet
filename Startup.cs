using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using ToDoTnet.DataEntities;
using Microsoft.AspNetCore.Mvc.Authorization;
using ToDoTnet.Logic;
using ToDoTnet.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;

namespace ToDoTnet
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.


            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdministratorOnly", policy => policy.RequireRole("Administrator"));
            });
            services.AddMvc(config =>
            {
                var policy = new AuthorizationPolicyBuilder()
                                 .RequireAuthenticatedUser()
                                 .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            });

            services.AddDbContext<ToDoContext>(options => options.UseSqlite(Program.connString));


            services.AddIdentity<ToDoUser, IdentityRole>()
                .AddEntityFrameworkStores<ToDoContext>()
                .AddUserStore<ToDoUserStore<ToDoUser>>();

            services.AddScoped<SignInManager<ToDoUser>, ToDoSignInManager>();
            services.AddScoped<UserManager<ToDoUser>, ToDoUserManager>();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationScheme = "Cookie",
                LoginPath = new PathString("/Account/Login/"),
                AccessDeniedPath = new PathString("/Account/Forbidden/"),
                AutomaticAuthenticate = true,
                AutomaticChallenge = true

            });

            app.UseStaticFiles();

            app.UseIdentity();
            app.UseClaimsTransformation(new ClaimsTransformationOptions
            {
                Transformer = new ClaimsTransformer()
            });
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
