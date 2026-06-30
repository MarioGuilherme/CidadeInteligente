namespace CidadeInteligente.Mvc.Requests.v1;

public record ChangePasswordRequest(string NewPassword, string ConfirmNewPassword, string Token);
