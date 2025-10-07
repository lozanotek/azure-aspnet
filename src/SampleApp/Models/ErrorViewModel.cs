namespace SampleApp.Models;

public class MessageInputModel
{
    public string? Value { get; set; }
}

public class ErrorViewModel
{
    public string? RequestId { get; set; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}
