using Infra.Data.Contexts; 
using JobMe.Api; 
var builder = WebApplication.CreateBuilder(args); 
var startup = new Startup(builder.Configuration); 
startup.ConfigureServices(builder.Services); 
var app = builder.Build(); 
try 
{  
    app.MigrateDbContext<ApplicationDbContext>();  
} catch (Exception ex)  
{  
    Console.WriteLine(ex.Message);  
} 
startup.Configure(app, app.Environment); 
app.Run(); 
