using Hotel.BLogicLayer;
using Hotel.BLogicLayer.Interfaces;
using Hotel.BLogicLayer.Services;
using Hotel.DAL.Interfaces;
using Hotel.DAL.Repositories;
using Hotel.DAL.Сontexts;
using Hotel.PRLAYER.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace Hotel.PRLAYER
{
    public class Startup
    {

        public Startup(IConfiguration config)
        {
            Configuration = config;
        }

        private IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //DB DEPENDENCY REGISTRATIONS
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<HotelContext>(options => options.UseSqlServer(connectionString));
            services.AddScoped<IUnitOfWork, EfUnitOfWork>();
            
            services.AddTransient<IRoomService, RoomService>();
            services.AddTransient<IMapperItem, ContainedMapper>();
            services.AddTransient<ICategoryService, СategoryService>();
            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<IGuestService, GuestService>();
            services.AddTransient<ICategoryDateService, CategoryDateService>();
            services.AddTransient<IReportInfoService, ReportService>();
            services.AddTransient<IStayService, StayService>();
            services.AddTransient<IBookingAndCheckInService, BookinAndCheckInService>();
            
            
            //MVC OPTIONS
            services.AddControllersWithViews(opts =>
            {
               
            });
            
            //AUTHENTICATION
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = AuthOptions.ISSUER,

                        ValidateAudience = true,
                        ValidAudience = AuthOptions.AUDIENCE,
                        ValidateLifetime = true,

                        IssuerSigningKey = AuthOptions.SymmetricSecurityKey,
                        ValidateIssuerSigningKey = true,
                    };
                });


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();


            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(opt =>
            {
                opt.MapControllers();

            });
        }
    }
}