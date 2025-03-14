using Domain.Entities;
using Infra.Data.Contexts; 
using MediatR; 
using Microsoft.Extensions.Configuration; 
using Microsoft.Extensions.DependencyInjection; 
using Serilog; 
namespace Infra.IoC 
{ 
    public static class DependencyInjectionExtension 
    { 
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration) 
        { 
            services.AddDbContext<ApplicationDbContext>(); 
            services.AddMediatR(AppDomain.CurrentDomain.Load("JobMe.Application")); 
            services.AddAutoMapper(AppDomain.CurrentDomain.Load("JobMe.Application")); 

            //services.AddScoped<Domain.Interfaces.Repositories.IGenericRepository<User>, Infra.Data.Repositories.EntityFramework.UserRepository>();
            services.AddScoped<Domain.Interfaces.Repositories.IGenericRepository<User>, Infra.Data.Repositories.EntityFramework.BaseRepositoryEntityFramework<User>>();
            services.AddScoped<Domain.Interfaces.Repositories.IGenericRepository<Character>, Infra.Data.Repositories.EntityFramework.BaseRepositoryEntityFramework<Character>>();

            Log.Logger = new LoggerConfiguration() 
               .WriteTo.Console() 
               .WriteTo.File("./logs/servicelog-.log", rollingInterval: RollingInterval.Day) 
               .CreateLogger(); 
            return services; 
        } 
    } 
} 
