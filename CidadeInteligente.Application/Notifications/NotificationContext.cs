using CidadeInteligente.Application.Extensions;
using CidadeInteligente.Core.Notifications;
using Serilog;

namespace CidadeInteligente.Application.Notifications;

public sealed class NotificationContext : INotificationContext
{
    private readonly List<Notification> _notifications = [];
    private readonly List<Tuple<string, string>> _validations = [];

    public bool HasNotifications => _notifications.Count != 0;
    public bool HasValidations => _validations.Count != 0;

    public IReadOnlyCollection<Notification> Notifications => _notifications;

    public Dictionary<string, string[]> Validations => _validations
        .GroupBy(v => v.Item1)
        .ToDictionary(g => g.Key, g => g.Select(n => n.Item2).ToArray());

    public void AddNotification(NotificationType notificationType, IEnumerable<object>? @params = default)
    {
        Notification notification = new(notificationType, @params);
        Log.Warning(notification.Type.GetDescription(), @params);
        _notifications.Add(notification);
    }

    public void AddValidation(string key, string value) => _validations.Add(new(key, value));
}
