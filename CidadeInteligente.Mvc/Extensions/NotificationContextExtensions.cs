using CidadeInteligente.Application.Extensions;
using CidadeInteligente.Domain.Notifications;

namespace CidadeInteligente.Mvc.Extensions;

public static partial class NotificationContextExtensions
{
    extension(INotificationContext notificationContext)
    {
        public IEnumerable<string> NotificationsToListString
            => notificationContext.Notifications.Select(n => string.Format(n.Type.GetDescription(), args: n?.Params?.ToArray() ?? []));

        public IEnumerable<string> ValidationsToListString
            => notificationContext.Validations.Values.SelectMany(v => v);
    }
}
