using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using ServerService.Abstracts;
using ServerService.Domain;
using ServerService.Services;


namespace ServerService
{
    public class Startup
    {

        public IConfiguration Configuration { get; }
        private IConfigurationRoot _api_dadata { get; }


        public Startup(IConfiguration configuration, IHostEnvironment env)
        {

            Configuration = configuration;

            _api_dadata = new ConfigurationBuilder()
               .SetBasePath(env.ContentRootPath)
               .AddJsonFile("appsettings.json")
               .Build();
        }

       
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddTransient<ITreeService, TreeService>();

            services.AddScoped<TreeRepository>();
            services.AddScoped<TreeService>();
            
            services.AddMvc(op=> op.EnableEndpointRouting = false);
  
            services.AddEntityFrameworkNpgsql().AddDbContext<TreeContext>(opt =>
              opt.UseNpgsql(_api_dadata.GetConnectionString("TreeConnection")));
        }

       
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
               // app.UseDeveloperExceptionPage();
         
            }

             app.UseHttpsRedirection();

             app.UseRouting();

            //app.UseAuthorization();
           // app.UseMvcWithDefaultRoute();
            app.UseEndpoints(endpoints =>
            {
                endpoints
                .MapControllerRoute("controller",
                "{controller=Server}/{action=Interection}/{id?}");
                 
            });
             
        }
    }
}
