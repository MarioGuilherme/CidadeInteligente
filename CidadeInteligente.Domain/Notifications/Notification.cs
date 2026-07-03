namespace CidadeInteligente.Domain.Notifications;

public record Notification(NotificationType Type, IEnumerable<object>? @Params = default);
