using LabW08Delegate.EventArgs;

namespace LabW08Delegate.Delegates;

// Delegate đại diện cho hàm nhận cảnh báo và trả về true/false.
public delegate bool AlertFilter(DeviceAlertEventArgs alert);
