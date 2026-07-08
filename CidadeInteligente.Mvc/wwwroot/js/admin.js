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
                if (!Object.hasOwn(entity, "userId")) {
                    const { areaId, courseId, description } = entity;
                    const id = areaId ?? courseId;
                    dataTable.row.add([id, description, renderButtons(id)]).draw();
                    return;
                }

                const { userId, name, email, course, roleDescription } = entity;
                dataTable.row.add([userId, name, email, course, roleDescription, renderButtons(userId)]).draw();
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
            const formName = entityName === "users" ? "userForm" : entityName === "areas" ? "areaForm" : "courseForm";
            $("div#passwordInputs input").attr("required", entityName === "users" && !isUpdate);

            if (hasEmptyField($(`.modal form#${formName}`))) return;

            const dataJson = buildDataFromForm($(`.modal form#${formName}`));
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

            const entityName = getCurrentEntityName();
            const displayEntityName = entityName === "users" ? "usuário" : entityName === "areas" ? "área" : "curso";
            $(".modal-title").html(`Cadastrar ${displayEntityName}`);

            $("form#userForm, form#areaForm, form#courseForm").hide();
            if (getCurrentEntityName() === "users") {
                $("form#userForm, #passwordInputs").show();
                return;
            }
            $(`form#${getCurrentEntityName() === "areas" ? "area" : "course"}Form`).show();
        });

        screenExitTargetBlocker.onClickBlockingTargetAndLeavingFromScreenEventDelegate($("tbody"), ".btn-edit", async button => {
            sweetAlertUtils.sweetAlertBlockingScreen("Buscando os dados deste registro");

            const entityName = getCurrentEntityName();
            const displayEntityName = entityName === "users" ? "usuário" : entityName === "areas" ? "área" : "curso";
            const { statusCode, data, notifications } = await restApi.getAsync(`v1/${entityName}/${$(button).attr("id")}`);
            if (statusCode === null) {
                sweetAlertUtils.sweetAlertAsync("error", "Ocorreu um erro durante a busca do registro.");
                return;
            }

            if (statusCode !== 200) {
                showNotifications(notifications, statusCode);
                return;
            }

            const { userId, areaId, courseId, name, email, role, description } = data;

            $(".modal-title").html(`Editar ${displayEntityName}`);
            $("form#userForm, form#areaForm, form#courseForm, #passwordInputs").hide();
            $("input[name=entityId]").val(userId ?? areaId ?? courseId);

            if (entityName === "users") {
                $("form#userForm").show();
                $("input[name=name]").val(name);
                $("input[name=email]").val(email);
                $("select[name=courseId]").val(courseId);
                $("select[name=role]").val(role);
            } else {
                $(`form#${getCurrentEntityName() === "areas" ? "area" : "course"}Form`).show();
                $("input[name=description]").val(description);
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
