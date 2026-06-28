using CidadeInteligente.Application.Extensions;
using CidadeInteligente.Core.Notifications;

namespace CidadeInteligente.Mvc.Extensions;

public static partial class NotificationContextExtensions
{
    extension(INotificationContext notificationContext)
    {
        public IEnumerable<string> AsListString
            => notificationContext.Notifications.Select(n => string.Format(n.Type.GetDescription(), args: n?.Params?.ToArray() ?? []));
    }
}
