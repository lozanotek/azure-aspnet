using SampleApp.Models;

namespace SampleApp.Jobs;

public interface IJobManager
{
	Task EnqueueMessageJob(MessageInputModel inputModel);
}
