using Azure.Messaging.ServiceBus;
using SampleApp.Models;

namespace SampleApp.Service;

public class MessageService : IMessageService
{
	private readonly ServiceBusClient serviceBusClient;
	private readonly ILogger<MessageService> logger;

	public MessageService(ServiceBusClient serviceBusClient, ILogger<MessageService> logger)
	{
		this.serviceBusClient = serviceBusClient;
		this.logger = logger;
	}

	public async Task PublishAsync(MessageInputModel inputModel)
	{
		logger.LogInformation("Service | Publishing message: {Message}", inputModel.Value);
		var sender = serviceBusClient.CreateSender("DemoTopic");
		await sender.SendMessageAsync(new ServiceBusMessage(inputModel.Value ?? string.Empty));

		logger.LogInformation("Service | Message published: {Message}", inputModel.Value);
	}
}

public interface IMessageService
{
	Task PublishAsync(MessageInputModel inputModel);
}
