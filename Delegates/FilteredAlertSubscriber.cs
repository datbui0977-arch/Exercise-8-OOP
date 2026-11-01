using LabW08Delegate.EventArgs;

namespace LabW08Delegate.Delegates;

public sealed class FilteredAlertSubscriber
{
    private readonly AlertFilter _filter;
    private readonly Action<DeviceAlertEventArgs> _action;

    public FilteredAlertSubscriber(
        AlertFilter filter,
        Action<DeviceAlertEventArgs> action)
    {
        _filter = filter ?? throw new ArgumentNullException(nameof(filter));
        _action = action ?? throw new ArgumentNullException(nameof(action));
    }

    public void HandleAlert(object? sender, DeviceAlertEventArgs e)
    {
        if (_filter(e))
            _action(e);
    }
}
