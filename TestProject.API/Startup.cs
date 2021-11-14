using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TestProject.API.Utilities;
using TestProject.DAL.Context;
using TestProject.Infrastructure.Infrastructures;
using TestProject.Repository.GenericRepo;
using TestProject.Services.UserServices;

namespace TestProject.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

        }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null); ;
            services.AddDbContext<TestProjectContext>(ServiceLifetime.Transient);
            //services.AddDbContext<TestProjectContext>(options => options.UseSqlServer(Configuration.GetConnectionString("TestProjectDB")));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IDatabaseFactory, DatabaseFactory>();
            services.AddScoped<UnitOfWork>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<UserManager>();

            //services.AddScoped<IUserRoleService, UserRoleService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();

            app.UseStaticFiles();

            app.UseCookiePolicy();

            app.UseStaticHttpContext();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
