using Idea.Data;
using Idea.Data.Models.Locations;
using Idea.Service;
using Idea.Service.Models;
using Idea.Web.Models.Core;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Idea.Web
{
    public class Program
    {
        // TODO: Think of input of seed
        // TODO: Think of lazy
        private static int GetCurrentSeed()
        {
            int seed = new Random().Next();

            Console.WriteLine("============================");
            Console.WriteLine("Current seed: " + seed);
            Console.WriteLine("============================");
            Console.WriteLine();

            return seed;
        }

        private static void SeedDatabase(IServiceProvider serviceCollection)
        {
            using(var serviceScope = serviceCollection.CreateScope())
            {
                using(var ideaDbContext = serviceScope.ServiceProvider.GetService<IdeaDbContext>())
                {
                    ideaDbContext.LocationTypes.Add(new LocationType { Name = "StarSystem" });
                    ideaDbContext.LocationTypes.Add(new LocationType { Name = "EmptySpace" });
                    ideaDbContext.LocationTypes.Add(new LocationType { Name = "Nebula" });
                    ideaDbContext.LocationTypes.Add(new LocationType { Name = "AsteroidField" });
                    ideaDbContext.LocationTypes.Add(new LocationType { Name = "CelestialObject" });

                    ideaDbContext.SaveChanges();
                }
            }
        }

        private static void ConfigureServices(WebApplicationBuilder builder)
        {
            // Add DbContext
            builder.Services.AddDbContext<IdeaDbContext>(options =>
                 options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            
            // Add Custom Services
            builder.Services.AddSingleton<IRandomService>(new RandomService(GetCurrentSeed()));
            builder.Services.AddTransient<IUserService, UserService>();
            builder.Services.AddTransient<IShipService, ShipService>();

            // Add JWT Auth
            // Configure strongly typed settings objects
            var jwtSettingsSection = builder.Configuration.GetSection("JwtSettings");
            builder.Services.Configure<AppJwtSettings>(jwtSettingsSection);

            // Configure jwt authentication
            var jwtSettings = jwtSettingsSection.Get<AppJwtSettings>();
            var key = Encoding.ASCII.GetBytes(jwtSettings.Secret);
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        IUserService userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();
                        string userId = context.Principal.Identity.Name;
                        IdeaUserServiceModel user = userService.GetByIdAsync(userId).GetAwaiter().GetResult();
                        if (user == null)
                        {
                            // return unauthorized if user no longer exists
                            context.Fail("Unauthorized");
                        }
                        context.HttpContext.Items["User"] = user;
                        return Task.CompletedTask;
                    }
                };
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            // Add services to the container.
            builder.Services.AddControllers();
            
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("MyPolicy", policyBuilder =>
                {
                    policyBuilder.WithOrigins("http://localhost:63342")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });
        }

        private static void ConfigureApp(WebApplication app)
        {
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            SeedDatabase(app.Services);

            app.UseCors("MyPolicy");
             
            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            ConfigureServices(builder);

            var app = builder.Build();
            ConfigureApp(app);
        }
    }
}