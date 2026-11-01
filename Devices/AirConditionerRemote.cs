using LabW08Delegate.Enums;

namespace LabW08Delegate.Devices;

public sealed class AirConditionerRemote : TeachingDevice
{
    public string AssignedRoom { get; }
    public bool IsResponsive { get; private set; }

    public AirConditionerRemote(string id, string name, string assignedRoom)
        : base(id, name)
    {
        if (string.IsNullOrWhiteSpace(assignedRoom))
            throw new ArgumentException("Phòng được phân công không được để trống.", nameof(assignedRoom));

        AssignedRoom = assignedRoom;
        IsResponsive = true;
    }

    public void UpdateResponseState(bool isResponsive)
    {
        bool wasResponsive = IsResponsive;
        IsResponsive = isResponsive;

        // Chỉ cảnh báo tại thời điểm chuyển từ có phản hồi sang mất phản hồi.
        if (wasResponsive && !isResponsive)
        {
            RaiseAlert(
                $"Điều hòa tại phòng {AssignedRoom} không phản hồi.",
                AlertSeverity.Critical);
        }
    }
}
