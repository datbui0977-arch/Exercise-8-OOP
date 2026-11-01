using LabW08Delegate.Enums;
using LabW08Delegate.EventArgs;

namespace LabW08Delegate.Devices;

public abstract class TeachingDevice
{
    public string Id { get; }
    public string Name { get; }
    public DeviceStatus Status { get; private set; }

    public event EventHandler<DeviceStatusChangedEventArgs>? StatusChanged;
    public event EventHandler<DeviceAlertEventArgs>? AlertRaised;

    protected TeachingDevice(string id, string name)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentException("Mã thiết bị không được để trống.", nameof(id));

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Tên thiết bị không được để trống.", nameof(name));

        Id = id;
        Name = name;
        Status = DeviceStatus.Available;
    }

    public void ChangeStatus(DeviceStatus newStatus)
    {
        if (newStatus == Status)
            return;

        DeviceStatus oldStatus = Status;
        Status = newStatus;

        OnStatusChanged(new DeviceStatusChangedEventArgs(
            Id, oldStatus, newStatus, DateTime.Now));
    }

    protected void RaiseAlert(string message, AlertSeverity severity)
    {
        OnAlertRaised(new DeviceAlertEventArgs(
            Id, message, severity, DateTime.Now));
    }

    protected virtual void OnStatusChanged(DeviceStatusChangedEventArgs e)
    {
        StatusChanged?.Invoke(this, e);
    }

    protected virtual void OnAlertRaised(DeviceAlertEventArgs e)
    {
        AlertRaised?.Invoke(this, e);
    }

    protected void ValidateReading(int value)
    {
        if (value < 0 || value > 100)
            throw new LabW08Delegate.Exceptions.InvalidDeviceReadingException(Id, value);
    }
}
