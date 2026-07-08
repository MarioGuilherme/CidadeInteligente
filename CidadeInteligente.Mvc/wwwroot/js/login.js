(function ($) {
    "use strict";

    $(document).ready(() => {
        screenExitTargetBlocker.onClickBlockingTargetAndLeavingFromScreen($(".btn-login"), async () => {
            if (hasEmptyField($("form"))) return;

            sweetAlertUtils.sweetAlertBlockingScreen("Fazendo login");
            const { statusCode, notifications } = await restApi.postAsync("v1/auth/login", {
                email: $("input[name=email]").val().trim() || null,
                password: $("input[name=password]").val().trim() || null
            });
            if (statusCode === null) {
                sweetAlertUtils.sweetAlertAsync("error", "Ocorreu um erro durante o login!");
                return;
            }

            if (statusCode !== 201) {
                showNotifications(notifications, statusCode);
                return;
            }

            toggleAskBeforeExit(false);
            $(location).attr("href", "/");
        });
    });
})(jQuery);
