using LabW08Delegate.Enums;

namespace LabW08Delegate.Devices;

public sealed class ProjectorRemote : TeachingDevice
{
    public int BatteryLevel { get; private set; }

    public ProjectorRemote(string id, string name, int batteryLevel = 100)
        : base(id, name)
    {
        ValidateReading(batteryLevel);
        BatteryLevel = batteryLevel;
    }

    public void UpdateBatteryLevel(int newLevel)
    {
        ValidateReading(newLevel);

        AlertSeverity? oldSeverity = GetBatterySeverity(BatteryLevel);
        AlertSeverity? newSeverity = GetBatterySeverity(newLevel);

        BatteryLevel = newLevel;

        // Chỉ cảnh báo khi thiết bị chuyển sang một mức cảnh báo khác.
        if (newSeverity is not null && newSeverity != oldSeverity)
        {
            string message = newLevel == 0
                ? "Pin máy chiếu đã hết (0%)."
                : $"Pin máy chiếu xuống thấp: {newLevel}%.";

            RaiseAlert(message, newSeverity.Value);
        }
    }

    private static AlertSeverity? GetBatterySeverity(int level)
    {
        if (level == 0) return AlertSeverity.Critical;
        if (level < 20) return AlertSeverity.Warning;
        return null;
    }
}
