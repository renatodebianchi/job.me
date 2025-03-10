using Api.Hubs; 
using Microsoft.AspNetCore.Builder; 
using Microsoft.AspNetCore.Hosting; 
using Microsoft.Extensions.Configuration; 
using Microsoft.Extensions.DependencyInjection; 
using Microsoft.Extensions.Hosting; 
using Microsoft.OpenApi.Models; 
using Infra.IoC; 
using System; 
using System.IO; 
using System.Reflection; 
namespace JobMe.Api 
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
            services.AddInfrastructure(Configuration); 
            services.AddControllers(); 
            services.AddSignalR() 
                    .AddJsonProtocol(options => 
                    { 
                       options.PayloadSerializerOptions.PropertyNamingPolicy = null; 
                    }); 
			 services.AddCors(options => options.AddPolicy("CorsPolicy", 
            builder => 
            { 
                builder.AllowAnyHeader() 
                        .AllowAnyMethod() 
                        .SetIsOriginAllowed((host) => true) 
                        .AllowCredentials(); 
            })); 
            services.AddSwaggerGen(c => 
            { 
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "JobMe.Api", Version = "v1" }); 
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"; 
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile); 
                c.IncludeXmlComments(xmlPath); 
            }); 
        } 
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline. 
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) 
        { 
            if (env.IsDevelopment()) 
            { 
                app.UseDeveloperExceptionPage(); 
                app.UseSwagger(); 
                app.UseSwaggerUI(c => { 
                    c.RoutePrefix = string.Empty; 
                    string swaggerPath = string.IsNullOrWhiteSpace(c.RoutePrefix) ? "." : ".."; 
                    c.SwaggerEndpoint($"{swaggerPath}/swagger/v1/swagger.json", "JobMe.Api v1"); 
                }); 
            } 
            app.UseHttpsRedirection(); 
            app.UseRouting(); 
            app.UseCors("CorsPolicy"); 
            app.UseAuthorization(); 
            app.UseEndpoints(endpoints => 
            { 
                endpoints.MapControllers(); 
                endpoints.MapHub<WebSocketHub>("/Hub"); 
            }); 
        } 
    } 
} 
