using LabW08Delegate.Coordinators;
using LabW08Delegate.Delegates;
using LabW08Delegate.Devices;
using LabW08Delegate.Enums;
using LabW08Delegate.Exceptions;
using LabW08Delegate.Services;

namespace LabW08Delegate;

internal static class Program
{
    private static void Main()
    {
        // 1. Khởi tạo các thiết bị.
        var projector = new ProjectorRemote("P001", "May chieu lop A", 80);
        var airConditioner = new AirConditionerRemote(
            "AC001", "Dieu hoa", "A101");
        var microphone = new WirelessMicrophone(
            "M001", "Micro khong day", 90, 85);

        List<TeachingDevice> devices =
        [
            projector,
            airConditioner,
            microphone
        ];

        // 2. Khởi tạo các subscriber.
        var notificationService = new ConsoleNotificationService();
        var auditLogger = new DeviceAuditLogger();
        var maintenanceService = new MaintenanceService();

        // 3. Điều phối đăng ký event.
        using var coordinator = new DeviceMonitoringCoordinator(
            devices,
            notificationService,
            auditLogger,
            maintenanceService);

        coordinator.Subscribe();

        // 4. Delegate lọc riêng các cảnh báo Critical.
        var criticalOnly = new FilteredAlertSubscriber(
            alert => alert.Severity == AlertSeverity.Critical,
            alert => Console.WriteLine(
                $"[CRITICAL FILTER] {alert.DeviceId} - {alert.Message}"));

        projector.AlertRaised += criticalOnly.HandleAlert;

        Console.WriteLine("===== DEMO DEVICE EVENTS =====");

        // Thay đổi trạng thái: DeviceAuditLogger nhận được event.
        projector.ChangeStatus(DeviceStatus.Borrowed);
        projector.ChangeStatus(DeviceStatus.UnderMaintenance);

        // Cập nhật pin: Warning rồi Critical.
        projector.UpdateBatteryLevel(15);
        projector.UpdateBatteryLevel(10); // Cùng vùng Warning: không cảnh báo lặp.
        projector.UpdateBatteryLevel(0);   // Chuyển sang Critical.

        // Điều hòa mất phản hồi: chỉ cảnh báo một lần.
        airConditioner.UpdateResponseState(false);
        airConditioner.UpdateResponseState(false);

        // Micro: pin thấp và tín hiệu thấp.
        microphone.UpdateBatteryLevel(20);
        microphone.UpdateSignalQuality(30);
        microphone.UpdateSignalQuality(0);

        // 5. Minh họa ngoại lệ dữ liệu không hợp lệ.
        try
        {
            projector.UpdateBatteryLevel(120);
        }
        catch (InvalidDeviceReadingException ex)
        {
            Console.WriteLine($"[INPUT ERROR] {ex.Message}");
        }

        // 6. In lịch sử và yêu cầu bảo trì.
        Console.WriteLine("\n===== AUDIT LOG =====");
        foreach (string entry in auditLogger.Entries)
            Console.WriteLine(entry);

        Console.WriteLine("\n===== MAINTENANCE REQUESTS =====");
        foreach (string request in maintenanceService.Requests)
            Console.WriteLine(request);

        // Hủy đăng ký handler lọc riêng.
        projector.AlertRaised -= criticalOnly.HandleAlert;

        // coordinator.Dispose() sẽ tự gọi Unsubscribe().
        Console.WriteLine("\n===== END PROGRAM =====");
    }
}
