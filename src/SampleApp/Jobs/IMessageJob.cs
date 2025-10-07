using Hangfire.Server;
using SampleApp.Models;
using System.ComponentModel;

namespace SampleApp.Jobs;

public interface IMessageJob
{
	[DisplayName("Message Processor")]
	Task<bool> ProcessMessage(PerformContext context, MessageInputModel inputModel);
}
