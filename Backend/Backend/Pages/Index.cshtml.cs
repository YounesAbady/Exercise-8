using FluentMigrator.Runner;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Reflection;
using YumCiity.DatabaseSpecific;
using YumCiity.Linq;

namespace Backend.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IConfiguration _configuration;
        public int count;
        public IndexModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void OnGet()
        {
            //var serviceProvider = CreateServices();

            //// Put the database update into a scope to ensure
            //// that all resources will be disposed.
            //using (var scope = serviceProvider.CreateScope())
            //{
            //    UpdateDatabase(scope.ServiceProvider);
            //}
            using (var adapter = new DataAccessAdapter())
            {
                var metaData = new LinqMetaData(adapter);
                count = metaData.Category.Count();
            }

        }
        private static IServiceProvider CreateServices()
        {
            IConfiguration config = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .AddEnvironmentVariables()
                    .Build();
            return new ServiceCollection()
                // Add common FluentMigrator services
                .AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                // Add SQLite support to FluentMigrator
                .AddPostgres11_0()
                // Set the connection string
                .WithGlobalConnectionString(config.GetConnectionString("YumCityDb"))
                // Define the assembly containing the migrations
                .ScanIn(Assembly.GetExecutingAssembly()).For.All())
                // Enable logging to console in the FluentMigrator way
                .AddLogging(lb => lb.AddFluentMigratorConsole())
                // Build the service provider
                .BuildServiceProvider(false);
        }
        private static void UpdateDatabase(IServiceProvider serviceProvider)
        {
            // Instantiate the runner
            var runner = serviceProvider.GetRequiredService<IMigrationRunner>();

            // Execute the migrations
            runner.MigrateUp();
        }
    }

}