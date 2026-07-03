(function ($) {
    "use strict";

    $(document).ready(() => {
        setDataTable();

        const getCurrentEntityName = () => $("main a.active").attr("id").split("-")[0];
        const renderButtons = id => `
            <button id=${id} class="btn btn-edit btn-strong-gray">
                <i class="fa fa-solid fa-pencil"></i> Editar
            </button>
            <button id=${id} class="btn btn-delete btn-strong-red">
                <i class="fa fa-solid fa-trash"></i> Apagar
            </button>
        `;
        const resetDataTableAsync = async () => {
            const entityName = getCurrentEntityName();
            const { statusCode, data, notifications } = await restAPI.getAsync(`v1/${entityName}`);
            const dataTable = $(`div#${entityName} table`).DataTable();

            if (entityName == "courses")
                refreshCoursesSelectInUserForm(data);

            if (data.length == 0) {
                dataTable.clear().draw();
                return;
            }

            dataTable.clear();
            data.forEach(entity => {
                if (!entity.hasOwnProperty("userId")) {
                    dataTable.row.add([entity.areaId ?? entity.courseId, entity.description, renderButtons(entity.areaId ?? entity.courseId)]).draw();
                    return;
                }

                dataTable.row.add([
                    entity.userId,
                    entity.name,
                    entity.email,
                    entity.course,
                    entity.roleDescription,
                    renderButtons(entity.userId)
                ]).draw();
            });
        }

        const refreshCoursesSelectInUserForm = async courses => {
            $("select[name=courseId] option").remove();
            $("select[name=courseId]").append(courses.reduce((acc, { courseId, description }) => acc + `<option value=${courseId}>${description}</option>`, ""));
        }

        $(".btn-save").click(async () => {
            const entityName = getCurrentEntityName();
            const dataJson = buildDataFromForm($(`.modal form#${entityName == "users" ? "userForm" : "areaOrCourseForm"}`));
            if (hasUndefinedOrNullOrEmptyField(dataJson)) {
                sweetAlert("warning", "Há campo(s) vazio(s) que precisam ser preenchido(s)!");
                return;
            }

            const entityId = +$(".modal input[name=entityId]").val().trim() || null;
            const isUpdate = !!entityId;
            sweetAlertAwait(`${isUpdate ? "Atualizando" : "Criando"} registro`);
            const { statusCode, data, notifications } = isUpdate
                ? await restAPI.patchAsync(`v1/${entityName}/${+entityId}`, dataJson)
                : await restAPI.postAsync(`v1/${entityName}`, dataJson);
            toggleExitConfirmation(false);

            switch (statusCode) {
                case 201:
                case 204:
                    resetDataTableAsync();
                    $(".modal").modal("hide");
                    cleanAllFields();
                    sweetAlert("success", `Registro ${isUpdate ? "atualizado" : "criado"} com sucesso!`);
                    break;
                case 400:
                    handleBadRequest(notifications);
                    break;
                case 500:
                    sweetAlert("error", "Um erro desconhecido ocorreu ao cadastrar o usuário!");
                    break;
                default:
                    sweetAlert("warning", notifications[0]);
                    break;
            }
        });

        $("button[data-target='#formModal']").click(() => {
            cleanAllFields();

            $(".modal-title").html("Novo registro");

            if (getCurrentEntityName() == "users") {
                $("form#areaOrCourseForm").hide();
                $("form#userForm, #passwordInputs").show();
                return;
            }

            $("form#areaOrCourseForm").show();
            $("form#userForm").hide();
        });

        $("tbody").on("click", ".btn-edit", async function () {
            sweetAlertAwait("Buscando os dados deste registro");
            const entityName = getCurrentEntityName();
            const { statusCode, data, notifications } = await restAPI.getAsync(`v1/${entityName}/${$(this).attr("id")}`);

            $(".modal-title").html("Editar Registro");
            $("#passwordInputs").hide();
            $("input[name=entityId]").val(data.userId ?? data.areaId ?? data.courseId);

            if (entityName == "users") {
                $("form#areaOrCourseForm").hide();
                $("form#userForm").show();
                $("input[name=name]").val(data.name);
                $("input[name=email]").val(data.email);
                $("select[name=courseId]").val(data.courseId);
                $("select[name=role]").val(data.role);
            } else {
                $("form#userForm").hide();
                $("form#areaOrCourseForm").show();
                $("input[name=description]").val(data.description);
            }

            $(".modal").modal("show");
            swal.close();
            toggleExitConfirmation(false);
        });

        $("tbody").on("click", ".btn-delete", async function () {
            const { value } = await Swal.fire({
                html: `<h2 style="color: white">Deseja mesmo excluir este registro?</h2>`,
                background: "rgb(70, 5, 7)",
                icon: "question",
                showCancelButton: true,
                allowOutsideClick: false,
                confirmButtonText: "Sim",
                confirmButtonColor: "#d9534f",
                cancelButtonText: "Não",
                cancelButtonColor: "#f0ad4e"
            });

            if (!value) return;
            
            sweetAlertAwait("Apagando registro");
            const entityName = getCurrentEntityName();
            const { statusCode, data, notifications } = await restAPI.deleteAsync(`v1/${entityName}/${$(this).attr("id")}`);

            toggleExitConfirmation(false);

            switch (statusCode) {
                case 204:
                    await resetDataTableAsync();
                    sweetAlert("success", "Registro apagado com sucesso!");
                    break;
                case 400:
                    handleBadRequest(notifications);
                    break;
                case 404:
                case 409:
                    sweetAlert("warning", notifications[0]);
                    break;
                case 500:
                    sweetAlert("error", notifications[0]);
                    break;
            }
        });
    });
})(jQuery);
