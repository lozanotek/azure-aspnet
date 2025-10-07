using Hangfire;
using SampleApp.Models;

namespace SampleApp.Jobs;

public class JobManager : IJobManager
{
	private readonly IBackgroundJobClient jobClient;

	public JobManager(IBackgroundJobClient jobClient)
	{
		this.jobClient = jobClient;
	}
	public Task EnqueueMessageJob(MessageInputModel inputModel)
	{
		// Enqueue a job to process the message
		jobClient.Enqueue<IMessageJob>(job => job.ProcessMessage(null!, inputModel));
		
		return Task.CompletedTask;
	}
}
