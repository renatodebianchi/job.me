using Microsoft.EntityFrameworkCore; 
using Polly; 

/// <summary>
/// Provides an extension method to migrate the database context.
/// </summary>
public static class MigrateDbContextExtensionClass 
{ 
    /// <summary>
    /// Migrates the database context.
    /// </summary>
    /// <typeparam name="TContext">The type of the database context.</typeparam>
    /// <param name="host">The host instance.</param>
    /// <returns>The host instance.</returns>
    public static IHost MigrateDbContext<TContext>(this IHost host) 
        where TContext : DbContext 
    { 
        using (var scope = host.Services.CreateScope()) 
        { 
            var services = scope.ServiceProvider; 
            var logger = services.GetRequiredService<ILogger<TContext>>(); 
            var context = services.GetService<TContext>(); 
            try 
            { 
                logger.LogInformation($"Migrating database associated with context {typeof(TContext).Name}"); 
                var retry = Policy.Handle<Exception>().WaitAndRetry(new[] 
                { 
                    TimeSpan.FromSeconds(5), 
                    TimeSpan.FromSeconds(10), 
                    TimeSpan.FromSeconds(15), 
                }); 
                retry.Execute(() => 
                { 
                        context?.Database.Migrate(); 
                }); 
                logger.LogInformation($"Migrated database associated with context {typeof(TContext).Name}"); 
            } 
            catch (Exception ex) 
            { 
                logger.LogError(ex, $"An error occurred while migrating the database used on context {typeof(TContext).Name}"); 
            } 
        } 
        return host; 
    } 
} 
