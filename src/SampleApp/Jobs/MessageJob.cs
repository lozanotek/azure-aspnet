using Hangfire.Console;
using Hangfire.Server;
using SampleApp.Models;

namespace SampleApp.Jobs;

public class MessageJob : IMessageJob
{
	private readonly ILogger<MessageJob> logger;
	public MessageJob(ILogger<MessageJob> logger)
	{
		this.logger = logger;
	}
	public async Task<bool> ProcessMessage(PerformContext context, MessageInputModel inputModel)
	{
		context.WriteLine("Job | Processing message: {0}", inputModel.Value);
		logger.LogInformation("Job | Processing message: {Message}", inputModel.Value);

		// Simulate some processing work
		await Task.Delay(2000);

		logger.LogInformation("Job | Message processed: {Message}", inputModel.Value);
		context.WriteLine("Job | Message processed: {0}", inputModel.Value);

		return true;
	}
}
