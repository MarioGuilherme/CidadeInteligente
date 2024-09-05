$(document).ready(() => {
    $(".btn-login").click(async () => {
        formHasEmptyField($("form").serializeArray());

        sweetAlertAwait("Fazendo login...");
        const { status } = await api.post("auth/login", {
            email: $("input[name=email]").val().trim(),
            password: $("input[name=password]").val().trim()
        });
        window.onbeforeunload = () => {}; // Desativa o alert de confirmação de saída

        status == 204
            ? sweetAlert("success", "Login efetuado!").then(({ value }) => value && $(location).attr("href", "/"))
            : sweetAlert("error", "E-mail e/ou senha não coincidem!");
    });
});