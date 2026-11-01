using LabW08Delegate.Devices;
using LabW08Delegate.Services;

namespace LabW08Delegate.Coordinators;

public sealed class DeviceMonitoringCoordinator : IDisposable
{
    private readonly List<TeachingDevice> _devices;
    private readonly ConsoleNotificationService _notificationService;
    private readonly DeviceAuditLogger _auditLogger;
    private readonly MaintenanceService _maintenanceService;

    private bool _isSubscribed;
    private bool _isDisposed;

    public DeviceMonitoringCoordinator(
        IEnumerable<TeachingDevice> devices,
        ConsoleNotificationService notificationService,
        DeviceAuditLogger auditLogger,
        MaintenanceService maintenanceService)
    {
        _devices = devices?.ToList()
            ?? throw new ArgumentNullException(nameof(devices));

        _notificationService = notificationService
            ?? throw new ArgumentNullException(nameof(notificationService));

        _auditLogger = auditLogger
            ?? throw new ArgumentNullException(nameof(auditLogger));

        _maintenanceService = maintenanceService
            ?? throw new ArgumentNullException(nameof(maintenanceService));
    }

    public void Subscribe()
    {
        ThrowIfDisposed();

        if (_isSubscribed)
            return;

        foreach (TeachingDevice device in _devices)
        {
            device.AlertRaised += _notificationService.HandleAlert;
            device.AlertRaised += _maintenanceService.HandleCriticalAlert;
            device.StatusChanged += _auditLogger.HandleStatusChanged;
        }

        _isSubscribed = true;
    }

    public void Unsubscribe()
    {
        if (!_isSubscribed)
            return;

        foreach (TeachingDevice device in _devices)
        {
            device.AlertRaised -= _notificationService.HandleAlert;
            device.AlertRaised -= _maintenanceService.HandleCriticalAlert;
            device.StatusChanged -= _auditLogger.HandleStatusChanged;
        }

        _isSubscribed = false;
    }

    public void Dispose()
    {
        if (_isDisposed)
            return;

        Unsubscribe();
        _isDisposed = true;
    }

    private void ThrowIfDisposed()
    {
        if (_isDisposed)
            throw new ObjectDisposedException(nameof(DeviceMonitoringCoordinator));
    }
}
