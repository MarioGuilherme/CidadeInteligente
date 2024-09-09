$(document).ready(() => {
    $(".btn-recover").click(() => {
        formHasEmptyField($("form").serializeArray());

        (async () => {
            sweetAlertAwait("Enviando email de recuperação");
            const { icon, message } = await api.post("sendEmailRecover", { email: $("input[name=email]").val() });
            toggleExitConfirmation(false);

            if (icon == "success") {
                sweetAlert(icon, message);
                cleanAllFields();
            } else
                sweetAlert(icon, message);
        })();
    });
});