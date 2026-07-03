(function ($) {
    "use strict";

    $(document).ready(() => {
        $(".btn-changePassword").click(async () => {
            formHasEmptyField($("form").serializeArray());

            if ($("input[type=password]:eq(0)").val() != $("input[type=password]:eq(1)").val()) {
                sweetAlert("error", "As senhas não conferem");
                return;
            }

            sweetAlertAwait("Enviando email de recuperação");
            const { statusCode, notifications } = await restAPI.patch("auth/changePassword", {
                newPassword: $("input[type=password]:eq(0)").val(),
                confirmNewPassword: $("input[type=password]:eq(1)").val(),
                token: $("input[name=token]").val()
            });
            toggleExitConfirmation(false);

            switch (statusCode) {
                case 204:
                    await sweetAlert("success", "Senha redefinida com sucesso!");
                    $(location).attr("href", "/");
                    break;
                case 400:
                    handleBadRequest(notifications);
                    break;
                case 404:
                case 410:
                    sweetAlert("warning", notifications[0]);
                    break;
                default:
                    sweetAlert("error", notifications[0]);
                    break;
            }
        });
    });
})(jQuery);
