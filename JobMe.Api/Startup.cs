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
    /// <summary>
    /// The startup class for configuring services and the app's request pipeline.
    /// </summary>
    public class Startup 
    { 
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public Startup(IConfiguration configuration) 
        { 
            Configuration = configuration; 
        } 

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        public IConfiguration Configuration { get; } 
        
        /// <summary>
        /// Configures the services for the application.
        /// </summary>
        /// <param name="services">The service collection to add services to.</param>
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
        
        /// <summary>
        /// Configures the HTTP request pipeline.
        /// </summary>
        /// <param name="app">The application builder.</param>
        /// <param name="env">The web host environment.</param>
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
                    c.InjectJavascript("/js/websocket-button.js");
                }); 
            } 
            app.UseHttpsRedirection(); 
            app.UseRouting(); 
            app.UseCors("CorsPolicy"); 
            app.UseStaticFiles(); 
            app.UseRouting(); 
            app.UseEndpoints(endpoints => 
            { 
                endpoints.MapControllers(); 
                endpoints.MapHub<WebSocketHub>("/Hub"); 
            }); 
        } 
    } 
} 
