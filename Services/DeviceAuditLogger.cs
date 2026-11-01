using LabW08Delegate.EventArgs;

namespace LabW08Delegate.Services;

public sealed class DeviceAuditLogger
{
    private readonly List<string> _entries = new();

    public IReadOnlyList<string> Entries => _entries.AsReadOnly();

    public void HandleStatusChanged(
        object? sender,
        DeviceStatusChangedEventArgs e)
    {
        _entries.Add(
            $"{e.ChangedAt:yyyy-MM-dd HH:mm:ss}: {e.DeviceId}, " +
            $"{e.OldStatus} -> {e.NewStatus}");
    }
}
