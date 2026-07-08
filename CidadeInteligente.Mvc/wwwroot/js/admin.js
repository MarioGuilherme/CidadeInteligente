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
            const { statusCode, data, notifications } = await restApi.getAsync(`v1/${entityName}`);
            if (statusCode === null) {
                sweetAlertUtils.sweetAlertAsync("error", `Ocorreu um erro durante a atualização da tabela.`);
                return;
            }

            if (statusCode !== 200) {
                showNotifications(notifications, statusCode);
                return;
            }

            const dataTable = $(`div#${entityName} table`).DataTable();

            if (entityName === "courses")
                refreshCoursesSelectInUserForm(data);

            if (data.length === 0) {
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
        };

        const refreshCoursesSelectInUserForm = async courses => {
            $("select[name=courseId] option").remove();
            $("select[name=courseId]").append(courses.reduce((acc, { courseId, description }) => acc + `<option value=${courseId}>${description}</option>`, ""));
        };

        screenExitTargetBlocker.onClickBlockingTargetAndLeavingFromScreen($(".btn-save"), async () => {
            const entityName = getCurrentEntityName();
            const entityId = +$(".modal input[name=entityId]").val().trim() || null;
            const isUpdate = !!entityId;
            $("div#passwordInputs input").attr("required", entityName === "users" && !isUpdate);

            if (hasEmptyField($(`.modal form#${entityName === "users" ? "userForm" : "areaOrCourseForm"}`))) return;

            const dataJson = buildDataFromForm($(`.modal form#${entityName === "users" ? "userForm" : "areaOrCourseForm"}`));
            sweetAlertUtils.sweetAlertBlockingScreen(`${isUpdate ? "Atualizando" : "Criando"} registro`);

            const { statusCode, data, notifications } = isUpdate
                ? await restApi.patchAsync(`v1/${entityName}/${entityId}`, dataJson)
                : await restApi.postAsync(`v1/${entityName}`, dataJson);
            if (statusCode === null) {
                sweetAlertUtils.sweetAlertAsync("error", `Ocorreu um erro durante a ${isUpdate ? "atualização" : "criação"} do registro.`);
                return;
            }

            if (statusCode !== 204 && statusCode !== 201) {
                showNotifications(notifications, statusCode);
                return;
            }

            resetDataTableAsync();
            $(".modal").modal("hide");
            sweetAlertUtils.sweetAlertAsync("success", `Registro ${isUpdate ? "atualizado" : "criado"} com sucesso!`);
        });

        $("button[data-target='#formModal']").click(() => {
            clearAllFields();

            $(".modal-title").html("Novo registro");

            if (getCurrentEntityName() === "users") {
                $("form#areaOrCourseForm").hide();
                $("form#userForm, #passwordInputs").show();
                return;
            }

            $("form#areaOrCourseForm").show();
            $("form#userForm").hide();
        });

        screenExitTargetBlocker.onClickBlockingTargetAndLeavingFromScreenEventDelegate($("tbody"), ".btn-edit", async button => {
            sweetAlertUtils.sweetAlertBlockingScreen("Buscando os dados deste registro");

            const entityName = getCurrentEntityName();
            const { statusCode, data, notifications } = await restApi.getAsync(`v1/${entityName}/${$(button).attr("id")}`);
            if (statusCode === null) {
                sweetAlertUtils.sweetAlertAsync("error", "Ocorreu um erro durante a busca do registro.");
                return;
            }

            if (statusCode !== 200) {
                showNotifications(notifications, statusCode);
                return;
            }

            $(".modal-title").html("Editar Registro");
            $("#passwordInputs").hide();
            $("input[name=entityId]").val(data.userId ?? data.areaId ?? data.courseId);

            if (entityName === "users") {
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

            swal.close();
            $(".modal").modal("show");
        });

        screenExitTargetBlocker.onClickBlockingTargetAndLeavingFromScreenEventDelegate($("tbody"), ".btn-delete", async button => {
            const { value } = await sweetAlertUtils.sweetAlertQuestionAsync("Deseja mesmo excluir este registro");
            if (!value) return;

            sweetAlertUtils.sweetAlertBlockingScreen("Apagando registro");
            const entityName = getCurrentEntityName();

            const { statusCode, data, notifications } = await restApi.deleteAsync(`v1/${entityName}/${$(button).attr("id")}`);
            if (statusCode === null) {
                sweetAlertUtils.sweetAlertAsync("error", "Ocorreu um erro durante a exclusão do registro.");
                return;
            }

            if (statusCode !== 204) {
                showNotifications(notifications, statusCode);
                return;
            }

            await resetDataTableAsync();
            sweetAlertUtils.sweetAlertAsync("success", "Registro apagado com sucesso!");
        });
    });
})(jQuery);
