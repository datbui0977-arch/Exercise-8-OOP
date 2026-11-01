using LabW08Delegate.Enums;

namespace LabW08Delegate.EventArgs;

public sealed class DeviceStatusChangedEventArgs : System.EventArgs
{
    public string DeviceId { get; }
    public DeviceStatus OldStatus { get; }
    public DeviceStatus NewStatus { get; }
    public DateTime ChangedAt { get; }

    public DeviceStatusChangedEventArgs(
        string deviceId,
        DeviceStatus oldStatus,
        DeviceStatus newStatus,
        DateTime changedAt)
    {
        DeviceId = deviceId;
        OldStatus = oldStatus;
        NewStatus = newStatus;
        ChangedAt = changedAt;
    }
}