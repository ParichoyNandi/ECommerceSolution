using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Serialization;
using ECommPortal.Services.Interface;
using ECommPortal.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Interfaces;
using DataAccess;
using ECommPortalDataAccess;

namespace ECommPortal
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
            services.AddControllersWithViews();

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSession(options => { options.IdleTimeout = TimeSpan.FromMinutes(30); });

            services.AddScoped<IAllDiscountService, AllDiscountServices>();
            services.AddScoped<IDiscountCourseCenterMappingService, DiscountCourseCenterMappingService>();
            services.AddScoped<ICouponService, CouponServices>();
            services.AddScoped<IPlanCouponMapService, PlanCouponMapService>();
            services.AddScoped<IProductsService, ProductsService>();
            services.AddScoped<IProductMapService, ProductMapService>();
            services.AddScoped<IProductConfigMappingService, ProductConfigMappingService>();
            services.AddScoped<IProductPublishDeactiveService, ProductPublishDeactiveService>();
            services.AddScoped<IPlanService, PlanService>();
            services.AddScoped<IPlanListingService, PlanListingService>();
            services.AddScoped<IPlanProductsConfigMappingService, PlanProductsConfigMappingService>();
            services.AddScoped<IPlanPublishDeactiveService,PlanPublishDeactiveService>();
            services.AddScoped<IDBAccess, DBAccess>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IProductListDashboardService, ProductListDashboardService >();
            services.AddScoped<IPlanListDashboardService,PlanListDashboardService >();
            services.AddScoped<IEcommPortalDBAccess, ECommPortalDBAccess>();




        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                //app.UseHsts();
            }
            //app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseSession();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Accounts}/{action=Index}/{id?}");
            });
        }
    }
}
