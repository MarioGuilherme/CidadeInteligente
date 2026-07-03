const setDataTable = () => {
    $("table").DataTable({
        pageLength: 5,
        lengthMenu: [5, 10, 25, 50],
        language: {
            url: "https://cdn.datatables.net/plug-ins/1.13.4/i18n/pt-BR.json"
        }
    });
}

const sweetAlert = (icon, msg, text = null) => Swal.fire({
    icon,
    html: `<h2 style=color:white>${msg}</h2>${text ? `<div style=color:white>${text}</div>` : ""}`,
    background: "rgb(70, 5, 7)",
    allowOutsideClick: false
});

const sweetAlertAwait = msg => {
    toggleExitConfirmation();
    return Swal.fire({
        icon: "info",
        html: `<div class="qrCode mb-1 d-flex justify-content-center"></div><h2 style="color:white;">${msg}, aguarde...</h2>`,
        background: "rgb(70, 5, 7)",
        allowOutsideClick: false,
        showConfirmButton: false
    });
}

const toggleExitConfirmation = (enabled = true) => window.onbeforeunload = enabled ? () => true : null;

const formHasEmptyField = form => {
    for (let i = 0; i < form.length; i++)
        if (form[i].value.trim() == "") {
            sweetAlert("warning", "Há campo(s) vazio(s) que precisam ser preenchido(s)!");
            throw "exit";
        }
}

const buildDataFromForm = form => [...form.find("input, textarea, select")].reduce((acc, field) => {
    const trimmedNullableValue = field.value.trim() || null;
    acc[field.name] = field.type == "text" ? trimmedNullableValue : +trimmedNullableValue;
    return acc;
}, {});

const hasUndefinedOrNullOrEmptyField = dataJson => Object.values(dataJson).some(f => f == undefined || f == null || f === "");

const cleanAllFields = () => {
    $(".user").attr("involved", false);
    $(".medias-uploaded").empty();
    $("input, select, textarea").val("");
    $("select").prop("selectedIndex", 0);
}

const handleBadRequest = notifications => sweetAlert("warning", "Campos inválidos", `<ul>${notifications.map(v => `<li>${v}</li>`).join("")}</ul>`);
const handleResponseAsync = async (statusCode, handlers) => handlers[statusCode] ? await handlers[statusCode]() : sweetAlert("error", "Erro!", "Ocorreu um erro inesperado.");
