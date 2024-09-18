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

const cleanAllFields = () => {
    $(".user").attr("involved", false);
    $(".medias-uploaded").empty();
    $("input, select, textarea").val("");
    $("select").prop("selectedIndex", 0);
}

const fileIsValidWithAlertReturn = ({ size, type }) => {
    const validExtensions = ["mp4", "png", "jpg", "jpeg"];
    const [mimeType, extension] = type.split("/");

    if (size > 4 * 1024 ** 2) {
        sweetAlert("warning", "Por favor, selecione mídias com menos de 4MB.");
        return false;
    }
    if (mimeType != "image" && mimeType != "video") {
        sweetAlert("warning", "É permitido apenas anexos do tipo vídeo e foto.");
        return false;
    }
    if (!validExtensions.includes(extension)) {
        sweetAlert("warning", "Apenas mídias .jpg, .jpeg, .png e .mp4.");
        return false;
    }

    return true;
}

const handleBadRequest = errors => sweetAlert("warning", "Campos inválidos", `<ul>${errors.map(v => `<li>${v}</li>`).join("")}</ul>`);