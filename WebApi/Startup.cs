using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using WebApi.Helpers;
using WebApi.Interfaces;
using WebApi.Models.Settings;
using WebApi.Services;

namespace WebApi
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
            //Add Cors Policy ... very important Cross Origin Resource Sharing
            //This tells the App and WebApi that the requests going back and forth have the same origin .. let's call it a "Special Handshake"
            services.AddCors(); //
            services.AddControllers();

            //Configure your settings for each section of data in the appsettings.json file
            //Create a class for each section and add them here as show in the line of code below
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            // Configure your DI (Dependency Injection) for application services
            //Create a Service and a IService (Service Interface) for each Service Class you create
            services.AddScoped<IUsersService, UsersService>();

            //Adds OpenApi (Aka Swagger, Aka Swahbuckle)
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApi", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            // This is configuring the CORS policy we talked about above
            app.UseCors(x => x
                .AllowAnyOrigin() //Personally I would add my own CORS policy, but for education purposes we will leave it as it is
                .AllowAnyMethod()
                .AllowAnyHeader());

            // Our custon Jwt helper
            app.UseMiddleware<JwtHelper>();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
