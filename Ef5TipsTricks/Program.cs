using Ef5TipsTricks;
using Ef5TipsTricks.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(ConfigureServices)
    .Build();

var dataContext = host.Services.GetService<DataContext>();
await dataContext.Database.EnsureCreatedAsync();

var application = host.Services.GetService<Application>();
await application.RunAsync();

void ConfigureServices(HostBuilderContext hostingContext, IServiceCollection services)
{
    services.AddDbContext<DataContext>(options =>
    {
        var sqlConnection = hostingContext.Configuration.GetConnectionString("SqlConnection");
        options.UseSqlServer(sqlConnection,
            options =>
            {
                options.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
            });
    });

    services.AddSingleton<Application>();
}
