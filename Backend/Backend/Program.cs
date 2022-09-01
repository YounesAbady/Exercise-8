using FluentMigrator.Runner;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
//builder.Services.AddFluentMigratorCore().ConfigureRunner(config => config
//    .AddPostgres11_0()
//    .WithGlobalConnectionString(builder.Configuration.GetSection("ConnectionStrings:YumCityDb").Value)
//    .ScanIn(Assembly.GetExecutingAssembly()).For.All());
builder.Configuration.AddXmlFile("D:/SIlverKey/Exercise-8/Backend/Backend/App.config");
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();
//using var scope = app.Services.CreateScope();
//var migrator = scope.ServiceProvider.GetService<IMigrationRunner>();
//migrator.MigrateUp();
app.Run();
