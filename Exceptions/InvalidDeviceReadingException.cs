namespace LabW08Delegate.Exceptions;

public sealed class InvalidDeviceReadingException : Exception
{
    public string DeviceId { get; }
    public int InvalidValue { get; }

    public InvalidDeviceReadingException(
        string deviceId,
        int invalidValue)
        : base($"Giá trị {invalidValue} của thiết bị {deviceId} không nằm trong khoảng 0-100.")
    {
        DeviceId = deviceId;
        InvalidValue = invalidValue;
    }
}