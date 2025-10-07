using Hangfire;
using Hangfire.AspNetCore;
using Hangfire.Console;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Logging.ApplicationInsights;
using SampleApp.Jobs;
using SampleApp.Security;
using SampleApp.Service;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var config = builder.Configuration;

services.AddAuthentication().AddCookie(options =>
{
	options.Cookie.Name = ".azure-aspnet";

	options.LoginPath = "/Account/Login";
	options.LogoutPath = "/Account/Logout";
});

// Add services to the container.
services.AddControllersWithViews();

services.AddTransient<IMessageService, MessageService>();
services.AddTransient<IJobManager, JobManager>();
services.AddTransient<IMessageJob, MessageJob>();

services.AddAzureClients(builder =>
{
	// This will register the ServiceBusClient using an Azure Identity credential.
	var connString = config.GetConnectionString("DemoSB");
	builder.AddServiceBusClient(connString);
});

services.AddApplicationInsightsTelemetry(new ApplicationInsightsServiceOptions
{
	ConnectionString = config["APPLICATIONINSIGHTS_CONNECTION_STRING"],
	DependencyCollectionOptions = { EnableLegacyCorrelationHeadersInjection = true }
});

services.AddLogging(builder =>
{
	builder.AddApplicationInsights(options =>
	{
		options.FlushOnDispose = true;
	});

	builder.AddFilter<ApplicationInsightsLoggerProvider>("", LogLevel.Information); // Capture Information and above
	builder.AddFilter<ApplicationInsightsLoggerProvider>("Microsoft", LogLevel.Warning); // Specific filter for Microsoft categories
});

services.AddHangfire((serviceProvider, globalConfig) =>
{
	globalConfig.UseSqlServerStorage(config.GetConnectionString("HangfireConnection"));

	globalConfig.SetDataCompatibilityLevel(CompatibilityLevel.Version_180);
	globalConfig.UseRecommendedSerializerSettings();
	globalConfig.UseColouredConsoleLogProvider();
	globalConfig.UseConsole();

	globalConfig.UseActivator(new AspNetCoreJobActivator(serviceProvider.GetRequiredService<IServiceScopeFactory>()));
});

services.AddHangfireServer((serviceProvider, options) =>
{
	options.ServerName = "Sample App";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
	Authorization = [new LocalAuthorizationFilter()],
});


app.MapStaticAssets();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}")
	.WithStaticAssets();


app.Run();
