using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using ITOne_AspnetCore.Api.User.Database;
using Lazarus.Common.DI;
using Lazarus.Common.Nexus.Database;
using Lazarus.Common.Utilities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace ITOne_AspnetCore
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        #region Implement
        public static ILifetimeScope AutofacContainer { get; set; }
        #endregion
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
          
            services.AddControllers();

            #region Implement
            services.AddSingleton<Microsoft.AspNetCore.Http.IHttpContextAccessor, Microsoft.AspNetCore.Http.HttpContextAccessor>();
            services.AddHttpContextAccessor();
            services.AddDbContext<DbDataContext>(options =>
    options.UseSqlServer(Configuration.GetConnectionString("CustomerDatabase")).UseLazyLoadingProxies());

            services.AddDbContext<NexusDataContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("NexusDatabase")));
            #endregion

            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "ASPNETCore API",
                    Version = "v3",
                    Description = "Full documentation to ASPNETCore public API",
                    Contact = new OpenApiContact
                    {
                        Name = "Zoccarato Davide",
                        Email = "davide@davidezoccarato.cloud",
                        Url = new Uri("https://www.davidezoccarato.cloud/")
                    },
                });

                c.IgnoreObsoleteProperties();

                // ref: https://stackoverflow.com/questions/56234504/migrating-to-swashbuckle-aspnetcore-version-5
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                });

            });


        }

        #region Implement
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new MediatorModule());
            builder.RegisterModule(new ProcessingModule());
            builder.RegisterModule(new RegisterServiceModule());
            builder.RegisterModule(new RegisterEventModule());
            builder.RegisterModule(new SharedModule());
        }
        #endregion

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var pathBase = Configuration["API_PATH_BASE"]; // <---

  

            if (!string.IsNullOrWhiteSpace(pathBase))
            {
                app.UsePathBase($"/{pathBase.TrimStart('/')}");
            }

            app.UseStaticFiles();
            #region Implement
            AppConfigUtilities._configuration = Configuration;

            AutofacContainer = app.ApplicationServices.GetAutofacRoot();
            DomainEvents._Container = AutofacContainer.BeginLifetimeScope();
            #endregion


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseSwagger();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
            app.UseCors("MyPolicy");
            #region Implement
            DependencyConfig.RegisterEvent();
            #endregion
        }
    }
}
