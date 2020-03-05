using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using mservicesample.Common.Services;
using mservicesample.Search.Api.DataAccess.ElasticSearch;
using mservicesample.Search.Api.DataAccess.Repositories;
using mservicesample.Search.Api.Helpers;

namespace mservicesample.Search.Api.Settings
{
     public static class ServiceRegistrations
    {
        public static IServiceCollection RegisterHealthCheck(this IServiceCollection services, string connectionstring, string apiurl)
        {
            //health ckeck configuration
            services.AddHealthChecks()
                .AddSqlServer(connectionString: connectionstring,
                    healthQuery: "SELECT 1;",
                    name: "Sql Server",
                    failureStatus: HealthStatus.Degraded)
                .AddUrlGroup(new Uri(apiurl + "api/Health/Check"),
                    name: "Health URL",
                    failureStatus: HealthStatus.Degraded);
            //add healthcheck ui
            services.AddHealthChecksUI(setupSettings: setup =>
            {
                setup.AddHealthCheckEndpoint("Basic healthcheck", apiurl + "alive");
            });
            return services;
        }

        public static IServiceCollection RegisterSwagger(this IServiceCollection services)
        {
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",new OpenApiInfo{ Title = "Search API", Version = "v1" });
                // Swagger 2.+ support
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
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

            return services;
        }

        public static IServiceCollection RegisterElasticSearch(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ElasticConnectionSettings>(configuration.GetSection(nameof(ElasticConnectionSettings)));
            services.AddSingleton(typeof(ElasticClientProvider));
            return services;
        }

        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            //services
            //    .AddScoped<IUserClaimsPrincipalFactory<IdentityUser>,
            //        UserClaimsPrincipalFactory<IdentityUser, IdentityRole>>();
            services.AddScoped(typeof(IArtistRepository), typeof(ArtistRepository));
            services.AddScoped(typeof(IReleaseRepository), typeof(ReleaseRepository));
            services.AddScoped(typeof(IReleaseByIdRepository), typeof(ReleaseByIdRepository));
            services.AddSingleton<JwtIssuerOptions>();
            services.AddSingleton<ILogger, Logger>();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IServiceId,ServiceId>();

            return services;
        }
    }
}
