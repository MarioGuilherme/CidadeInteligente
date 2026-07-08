const setDataTable = () => $("table").DataTable({
    pageLength: 5,
    lengthMenu: [5, 10, 25, 50],
    language: {
        url: "https://cdn.datatables.net/plug-ins/1.13.4/i18n/pt-BR.json"
    }
});

const toggleAskBeforeExit = (askBeforeExit = true) => window.onbeforeunload = askBeforeExit ? () => true : null;

const hasEmptyField = $form => {
    const hasEmpty = [...$form.find("input[required], select[required], textarea[required]")]
        .map(field => field.value.trim() || null)
        .filter(value => value === null).length > 0;

    if (hasEmpty)
        sweetAlertUtils.sweetAlertAsync("warning", "Há campo(s) vazio(s) que precisa(m) ser preenchido(s)!");

    return hasEmpty;
};

const buildDataFromForm = form => [...form.find("input, textarea, select")].reduce((acc, { name, value }) => {
    const trimmedNullableValue = value.trim() || null;
    acc[name] = !isNaN(+trimmedNullableValue) ? +trimmedNullableValue : trimmedNullableValue;
    return acc;
}, {});

const clearAllFields = () => {
    $(".user").attr("involved", false);
    $("input, select, textarea").val("");
    $("select").prop("selectedIndex", 0);
};

const showNotifications = (notifications, statusCode) => {
    const finalNotifications = statusCode === 400
        ? Object.values(notifications).flatMap(v => v)
        : !notifications || notifications.length === 0 ? ["<ul>Erro não mapeado</ul>"] : notifications;

    if (statusCode >= 500) {
        sweetAlertUtils.sweetAlertAsync("error", `<ul>${finalNotifications.map(n => `<li>${n}</li>`).join("")}</ul>`);
        return;
    }

    sweetAlertUtils.sweetAlertAsync("warning", `<ul>${finalNotifications.map(n => `<li>${n}</li>`).join("")}</ul>`);
};
