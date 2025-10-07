using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace SampleFunction;

public class WorkerFunction
{
    private readonly ILogger<WorkerFunction> _logger;

    public WorkerFunction(ILogger<WorkerFunction> logger)
    {
        _logger = logger;
    }

    [Function(nameof(WorkerFunction))]
    public async Task Run(
        [ServiceBusTrigger("DemoTopic", "azure-function", Connection = "DemoSB")]
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions)
    {
        _logger.LogInformation("Function | Message ID: {id}", message.MessageId);
        _logger.LogInformation("Function | Message Body: {body}", message.Body);
        _logger.LogInformation("Function | Message Content-Type: {contentType}", message.ContentType);

         // Complete the message
        await messageActions.CompleteMessageAsync(message);
    }
}
