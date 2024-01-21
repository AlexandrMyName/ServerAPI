using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace WebServiceForClient
{

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
 

        public void ConfigureServices(IServiceCollection services)
        {
            
            services.AddMvc(op => op.EnableEndpointRouting = false);
        }

      
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
     
            if (env.IsDevelopment())
            {
                 
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
              
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.UseMvc();
            app.UseEndpoints(endpoints =>
            {
                endpoints
                .MapControllerRoute("default",
                "{controller=WebService}/{action=MainView}/{id?}");

            });
        }
    }
}
