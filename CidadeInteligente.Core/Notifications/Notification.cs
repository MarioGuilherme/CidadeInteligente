namespace CidadeInteligente.Core.Notifications;

public record Notification(NotificationType Type, IEnumerable<object>? @Params = default);
