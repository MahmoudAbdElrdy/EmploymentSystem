using EmploymentSystem.CQRS.Commands;
using EmploymentSystem.Data;
using EmploymentSystem.Service;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();

        // Configure DbContext
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                Configuration.GetConnectionString("DefaultConnection")));

        // Register application services
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<UserService>();
        services.AddScoped<VacancyService>();

        // Register MediatR
        //  services.AddMediatR(typeof(Startup).Assembly);
        services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        //  services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMediatR(typeof(CreateVacancyCommandHandler).Assembly);
        services.AddMediatR(typeof(ApplyForVacancyCommand).Assembly);
        services.AddMediatR(typeof(CreateUserCommand).Assembly);
        services.AddMediatR(typeof(DeactivateVacancyCommand).Assembly);
        services.AddMediatR(typeof(DeleteVacancyCommand).Assembly);
        services.AddMediatR(typeof(PostVacancyCommand).Assembly);
        services.AddMediatR(typeof(UpdateVacancyCommand).Assembly);
        services.AddMediatR(Assembly.GetExecutingAssembly());
      //  services.AddMediatR(typeof(Startup).Assembly);  // Register handlers from the current assembly

        // Configure JWT authentication
        var key = Encoding.ASCII.GetBytes(Configuration["Jwt:Key"]);
        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(x =>
        {
            x.RequireHttpsMetadata = false;
            x.SaveToken = true;
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = Configuration["Jwt:Issuer"],
                ValidAudience = Configuration["Jwt:Audience"]
            };
        });

        // Add caching
        services.AddMemoryCache();

        // Add logging
        services.AddLogging();

        // Register the Swagger generator, defining 1 or more Swagger documents
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
            {
                Title = "EmploymentSystem API",
                Version = "v1"
            });
        });
    }


    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        // Enable middleware to serve generated Swagger as a JSON endpoint
     

        if (env.IsDevelopment() || env.IsProduction())
        {

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("./v1/swagger.json", "EmploymentSystem"));
        }
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }

}
