using Application.Common.Interfaces;
using Application.Common.Providers;
using AutoMapper;
using Azure.Identity;
using Domain.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Persistance;
using Infrastructure.Providers;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Azure.KeyVault;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using XbrlPlus.Application.Common.Mappings;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            services.AddBaseServices();
#if !DEBUG
			services.AddAzureKeyVault(configuration);
#endif
			
            services.AddApplicationServices(configuration);          
            services.AddIdentity(configuration);
            services.AddSwagger();

            return services;
        }

        private static IServiceCollection AddBaseServices(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddAutoMapper();

            return services;
        }
        
        private static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDatabaseContext(configuration);
            services.AddProviders();

            services.AddTransient<IPermissionValidator, PermissionValidator>();
            services.AddTransient<IImageService, ImageService>();

            return services;
        }

        private static IServiceCollection AddAutoMapper(this IServiceCollection services)
        {
            var mapperConfig = new MapperConfiguration(config =>
            {
                config.AddProfile(new MappingProfile());
            });
            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            return services;
        }

        private static IServiceCollection AddDatabaseContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseLazyLoadingProxies().UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName));
            });
            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>()!);

            return services;
        }

        private static IServiceCollection AddProviders(this IServiceCollection services)
        {
            services.AddTransient<IPermissionsProvider, PermissionsProvider>();
            services.AddTransient<IBasicRolesProvider, BasicRolesProvider>();

            return services;
        }

        private static IServiceCollection AddIdentity(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddTransient<IIdentityService, IdentityService>();
            services.AddTransient<ITokenService, TokenService>();
            services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredUniqueChars = 8;
                options.Password.RequiredLength = 8;
                options.User.RequireUniqueEmail = false;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            var jwtConfig = configuration.GetSection("JWT");
            var secretKey = jwtConfig["Secret"];
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateLifetime = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = jwtConfig["ValidIssuer"],
                    ValidAudience = jwtConfig["ValidAudience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                };
            });

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    policy =>
                    {
                        policy
                        .WithOrigins(@"https://localhost:7243",
                                     @"https://www.team-it.online")
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                    });
            });

            return services;
        }

		private static IServiceCollection AddAzureKeyVault(this IServiceCollection services, IConfiguration configuration)
		{
			var keyVaultClient = new KeyVaultClient(async (_, _, _) =>
			{
				var credential = new DefaultAzureCredential(false);
				var token = credential.GetToken(
					new Azure.Core.TokenRequestContext(
						new[] { "https://vault.azure.net/.default" }));
				return token.Token;
			});
			var vaultUri = configuration.GetSection("TeamItSecretVault:VaultUri").Value!;
			((ConfigurationManager)configuration).AddAzureKeyVault(vaultUri, keyVaultClient, new DefaultKeyVaultSecretManager());
			return services;
		}

		private static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "TeamIt API",
                    Version = "v1",
                    Description = "API of project managment system with teams and messenger functionality"
                }); ;
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Description = "Please insert JWT with Bearer into field",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });
            return services;
        }
    }
}