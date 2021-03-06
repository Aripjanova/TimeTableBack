using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;


namespace WebApplication
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
            services.AddControllers();
            
           /* services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigin", builder => builder.AllowAnyOrigin()
                    .WithMethods("DELETE")
                    .WithMethods("PUT")
                    .WithMethods("GET")
                    .WithMethods("POST")
                );*/
                services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                }));
          //  });
            services.AddTransient<AppDb>(_ => new AppDb(Configuration["ConnectionStrings:DefaultConnection"]));        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors();
            app.UseCors(options => options.AllowAnyOrigin());
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}