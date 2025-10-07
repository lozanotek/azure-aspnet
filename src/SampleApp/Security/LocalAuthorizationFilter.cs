using Hangfire.Dashboard;

namespace SampleApp.Security;

public class LocalAuthorizationFilter : IDashboardAuthorizationFilter
{
	public bool Authorize(DashboardContext context)
	{
		var httpContext = context.GetHttpContext();

		// Allow all authenticated users to see the Dashboard (potentially dangerous).
		return httpContext.User.Identity?.IsAuthenticated ?? false;
	}
}
