(function ($) {
    "use strict";

    $(document).ready(() => {
        screenExitTargetBlocker.onClickBlockingTargetAndLeavingFromScreen($(".btn-changePassword"), async () => {
            if (hasEmptyField($("form"))) return;

            if ($("input[type=password]:eq(0)").val() != $("input[type=password]:eq(1)").val()) {
                sweetAlertUtils.sweetAlertAsync("warning", "As senhas não conferem");
                return;
            }

            sweetAlertUtils.sweetAlertBlockingScreen("Enviando email de recuperação");
            const { statusCode, notifications } = await restApi.patchAsync("v1/auth/change-password", {
                newPassword: $("input[type=password]:eq(0)").val(),
                confirmNewPassword: $("input[type=password]:eq(1)").val(),
                token: $("input[name=token]").val()
            });
            if (statusCode === null) {
                sweetAlertUtils.sweetAlertAsync("error", "Ocorreu um erro durante a atualização da senha!");
                return;
            }

            if (statusCode !== 204) {
                showNotifications(notifications, statusCode);
                return;
            }

            toggleAskBeforeExit(false);
            await sweetAlertUtils.sweetAlertAsync("success", "Senha redefinida com sucesso!");
            $(location).attr("href", "/");
        });
    });
})(jQuery);
