using LabW08Delegate.Enums;
using LabW08Delegate.EventArgs;

namespace LabW08Delegate.Services;

public sealed class MaintenanceService
{
    private readonly List<string> _requests = new();

    public IReadOnlyList<string> Requests => _requests.AsReadOnly();

    public void HandleCriticalAlert(object? sender, DeviceAlertEventArgs e)
    {
        if (e.Severity != AlertSeverity.Critical)
            return;

        _requests.Add(
            $"Maintenance request for {e.DeviceId}: {e.Message}");
    }
}
