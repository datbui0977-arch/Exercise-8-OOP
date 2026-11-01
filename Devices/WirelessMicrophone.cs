using LabW08Delegate.Enums;

namespace LabW08Delegate.Devices;

public sealed class WirelessMicrophone : TeachingDevice
{
    public int BatteryLevel { get; private set; }
    public int SignalQuality { get; private set; }

    private AlertSeverity? _batteryAlertLevel;
    private AlertSeverity? _signalAlertLevel;

    public WirelessMicrophone(
        string id,
        string name,
        int batteryLevel = 100,
        int signalQuality = 100)
        : base(id, name)
    {
        ValidateReading(batteryLevel);
        ValidateReading(signalQuality);

        BatteryLevel = batteryLevel;
        SignalQuality = signalQuality;
        _batteryAlertLevel = GetBatterySeverity(batteryLevel);
        _signalAlertLevel = GetSignalSeverity(signalQuality);
    }

    public void UpdateBatteryLevel(int newLevel)
    {
        ValidateReading(newLevel);

        AlertSeverity? newSeverity = GetBatterySeverity(newLevel);
        BatteryLevel = newLevel;

        if (newSeverity is not null && newSeverity != _batteryAlertLevel)
        {
            string message = newLevel == 0
                ? "Pin micro đã hết (0%)."
                : $"Pin micro xuống thấp: {newLevel}%.";

            RaiseAlert(message, newSeverity.Value);
        }

        _batteryAlertLevel = newSeverity;
    }

    public void UpdateSignalQuality(int newQuality)
    {
        ValidateReading(newQuality);

        AlertSeverity? newSeverity = GetSignalSeverity(newQuality);
        SignalQuality = newQuality;

        if (newSeverity is not null && newSeverity != _signalAlertLevel)
        {
            string message = newQuality == 0
                ? "Chất lượng tín hiệu micro bằng 0%."
                : $"Chất lượng tín hiệu micro thấp: {newQuality}%.";

            RaiseAlert(message, newSeverity.Value);
        }

        _signalAlertLevel = newSeverity;
    }

    private static AlertSeverity? GetBatterySeverity(int level)
    {
        if (level == 0) return AlertSeverity.Critical;
        if (level < 30) return AlertSeverity.Warning;
        return null;
    }

    private static AlertSeverity? GetSignalSeverity(int quality)
    {
        if (quality == 0) return AlertSeverity.Critical;
        if (quality < 40) return AlertSeverity.Warning;
        return null;
    }
}
