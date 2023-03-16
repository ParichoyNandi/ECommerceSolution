using DataAccess;
using ECommAPI.Middleware;
using ECommManagement;
using ECommPortal.Models;
using ECommPortalDataAccess;
using Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Entities;

namespace ECommAPI
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

            services.AddDbContext<DbContext>(options=> options.UseSqlServer("Server=192.168.1.77;Database=SMS_Intern;User ID=ParichayIntern;Password=Parichay!Intern"));

                services.AddControllers();
            services.AddAutoMapper(typeof(Startup));

            services.AddScoped<IDBAccess, DBAccess>();
            services.AddScoped<IPaymentManagement, PaymentManagement>();
            services.AddScoped<IProductManagement, ProductManagement>();
            services.AddScoped<IEcommPortalDBAccess, ECommPortalDBAccess>();


            //services.AddHttpContextAccessor();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ECommAPI", Version = "v1" });
                c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme()
                {
                    Type = SecuritySchemeType.ApiKey,
                    In = ParameterLocation.Header,
                    Name = "X-API-KEY",
                    Description = "Authorization Key Required to use APIs",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "ApiKey" }
                        },
                        new string[] { }
                    }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                
            }

            
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
            
              c.SwaggerEndpoint("/swagger/v1/swagger.json", "ECommAPI v1");
             
            });

            //app.UseStaticFiles();
            app.UseHttpsRedirection();

            app.UseMiddleware<APIAccessHandlerMiddleware>();

            app.UseRouting();

            app.UseAuthorization();

            //app.Use(async (httpContext, next) =>
            //{
            //    var req = httpContext.Request.Body;


            //    await next.Invoke();
            //});

            //app.UseRequestResponseLogging();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
