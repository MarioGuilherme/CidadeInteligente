(function ($) {
    "use strict";

    $(document).ready(() => {
        screenExitTargetBlocker.onClickBlockingTargetAndLeavingFromScreen($(".btn-recover"), async () => {
            if (hasEmptyField($("form"))) return;

            sweetAlertUtils.sweetAlertBlockingScreen("Enviando email de recuperação");
            const { statusCode, notifications } = await restApi.patchAsync("v1/auth/send-email-recover", { email: $("input[name=email]").val().trim() });
            if (statusCode === null) {
                sweetAlertUtils.sweetAlertAsync("error", "Ocorreu um erro durante o envio de e-mail de recuperação de senha!");
                return;
            }

            if (statusCode !== 204) {
                showNotifications(notifications, statusCode);
                return;
            }

            await sweetAlertUtils.sweetAlertAsync("success", "Se este e-mail pertencer à uma conta, será feito o envio das instruções para redefinição de senha!");
            toggleAskBeforeExit(false);
            $(location).attr("href", "/login");
        });
    });
})(jQuery);
