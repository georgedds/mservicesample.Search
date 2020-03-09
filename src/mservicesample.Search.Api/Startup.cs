using System;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using mservicesample.Search.Api.Middleware;
using mservicesample.Search.Api.Settings;
using MediatR;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using mservicesample.Common.Infrastructure.Consul;

namespace mservicesample.Search.Api
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
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            var appSettings = appSettingsSection.Get<AppSettings>();
            var secretkey = Encoding.ASCII.GetBytes(appSettings.Secret);
            var signingKey = new SymmetricSecurityKey(secretkey);

            //Configure Consul
            services.AddConsul();
            
            services.AddCors();

            // jwt wire up
            // Get options from app settings
            var jwtAppSettingOptions = Configuration.GetSection(nameof(JwtIssuerOptions));

            // Configure JwtIssuerOptions
            services.Configure<JwtIssuerOptions>(options =>
            {
                options.Issuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                options.Audience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)];
                options.Expiresinminutes = Convert.ToInt32( jwtAppSettingOptions[nameof(JwtIssuerOptions.Expiresinminutes)]);
                options.SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256Signature);
            });

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)],
                ValidAudience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)],
                IssuerSigningKey = new SymmetricSecurityKey(secretkey)
            };

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(configureOptions =>
            {
                configureOptions.ClaimsIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                configureOptions.TokenValidationParameters = tokenValidationParameters;
                configureOptions.SaveToken = true;

                configureOptions.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("Token-Expired", "true");
                        }
                        return Task.CompletedTask;
                    }
                };
            });

            // api user claim policy
            services.AddAuthorization(options =>
            {
                options.AddPolicy("ApiUser", policy => policy.RequireClaim(Helpers.Constants.Strings.JwtClaimIdentifiers.Rol, Helpers.Constants.Strings.JwtClaims.ApiAccess));
            });

          
            services.AddMvc(x => x.EnableEndpointRouting = false); //.SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddAutoMapper(typeof(Startup));

            services.AddMediatR(typeof(Startup));

            //add healthcheck
            services.RegisterHealthCheck(Configuration.GetConnectionString("Default"), jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)]);


            //Add swagger
            services.RegisterSwagger();
           
            //Add elastic
            services.RegisterElasticSearch(Configuration);
            //register our services
            services.RegisterServices();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Search API V1");
            });
            

            //app.UseCors(x => x
            //    .AllowAnyOrigin()
            //    .AllowAnyMethod()
            //    .AllowAnyHeader());
            app.UseCors();


            //add error handling middleware always before mvc
            app.UseMiddleware(typeof(ExceptionHandlingMiddleware));
            app.UseMvc();

            var consulServiceId = app.UseConsul();
            //applicationLifetime.ApplicationStopped.Register(() =>
            //{
            //    client.Agent.ServiceDeregister(consulServiceId);
            //});

            //add healthcheck options
            app.UseHealthChecks("/alive", new HealthCheckOptions
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
            app.UseHealthChecksUI(); //http://localhost:5000/healthchecks-ui#/healthchecks
        }
    }
}
