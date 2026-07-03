(function ($) {
    "use strict";

    $(document).ready(() => {
        $(".btn-login").click(async () => {
            formHasEmptyField($("form").serializeArray());

            sweetAlertAwait("Fazendo login...");
            const { statusCode, notifications } = await restAPI.postAsync("v1/auth/login", {
                email: $("input[name=email]").val().trim(),
                password: $("input[name=password]").val().trim()
            });
            toggleExitConfirmation(false);

            switch (statusCode) {
                case 201:
                    $(location).attr("href", "/");
                    break;
                case 400:
                    handleBadRequest(notifications);
                    break;
                case 404:
                    sweetAlert("warning", notifications[0]);
                    break;
                default:
                    sweetAlert("error", notifications[0]);
                    break;
            }
        });
    });
})(jQuery);
