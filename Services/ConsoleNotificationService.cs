using LabW08Delegate.EventArgs;

namespace LabW08Delegate.Services;

public sealed class ConsoleNotificationService
{
    public void HandleAlert(object? sender, DeviceAlertEventArgs e)
    {
        Console.WriteLine(
            $"[{e.Severity}] {e.DeviceId}: {e.Message} (lúc {e.OccurredAt:HH:mm:ss})");
    }
}
