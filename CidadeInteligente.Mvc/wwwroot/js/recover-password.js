(function ($) {
    "use strict";

    $(document).ready(() => {
        $(".btn-recover").click(async () => {
            formHasEmptyField($("form").serializeArray());

            sweetAlertAwait("Enviando email de recuperação");
            const { statusCode, notifications } = await restAPI.patchAsync("v1/auth/send-email-recover", { email: $("input[name=email]").val().trim() });
            toggleExitConfirmation(false);

            switch (statusCode) {
                case 204:
                    sweetAlert("success", "Se este e-mail pertencer à uma conta, será feito o envio das instruções para redefinição de senha!").then(({ value }) => value && $(location).attr("href", "/login"));
                    break;
                case 400:
                    handleBadRequest(notifications);
                    break;
                case 424:
                    sweetAlert("warning", notifications[0]);
                    break;
                default:
                    sweetAlert("error", notifications[0]);
                    break;
            }
        });
    });
})(jQuery);
