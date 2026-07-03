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
        const resetDataTable = async () => {
            const entityName = getCurrentEntityName();
            const { data: entities } = await restAPI.get(`v1/${entityName}`);
            const dataTable = $(`div#${entityName} table`).DataTable();

            if (entityName == "courses")
                refreshCoursesSelectInUserForm(entities);

            if (entities.length == 0) {
                dataTable.clear().draw();
                return;
            }

            dataTable.clear();
            entities.forEach(entity => {
                let row;
                if (entity.hasOwnProperty("userId")) {
                    const { userId, name, email, course, roleDescription } = entity;
                    row = [
                        userId,
                        name,
                        email,
                        course,
                        roleDescription,
                        renderButtons(userId)
                    ];
                } else {
                    const { areaId, courseId, description } = entity;
                    row = [areaId ?? courseId, description, renderButtons(areaId ?? courseId)];
                }
                dataTable.row.add(row).draw();
            });
        }

        const refreshCoursesSelectInUserForm = async courses => {
            $("select[name=courseId] option").remove();
            $("select[name=courseId]").append(courses.reduce((acc, { courseId, description }) => acc + `<option value=${courseId}>${description}</option>`, ""));
        }

        $("ul.nav a.nav-link").click(function() {
            const entityName = $(this)[0].id.replace("-tab", "");
            history.pushState({}, "", `/admin?tab=${entityName}`);
        });

        $(".btn-save").click(async () => {
            const entityName = getCurrentEntityName();
            if (entityName == "users") {
                if ($("input[name=userId]").val().trim() == "") {
                    formHasEmptyField($("form#userForm").serializeArray().slice(1, -1));

                    if ($($("input[type=password")[0]).val() != $($("input[type=password")[1]).val()) {
                        sweetAlert("error", "As senhas não conferem");
                        return;
                    }

                    sweetAlertAwait("Cadastrando usuário");
                    const { status, body } = await restAPI.post("v1/users", {
                        name: $("input[name=name]").val().trim(),
                        email: $("input[name=email]").val().trim(),
                        password: $("input[name=password]").val(),
                        courseId: +$("select[name=courseId]").val(),
                        role: +$("select[name=role]").val()
                    });
                    toggleExitConfirmation(false);

                    switch (status) {
                        case 201:
                            await resetDataTable();
                            sweetAlert("success", "Usuário cadastrado com sucesso!");
                            cleanAllFields();
                            $(".modal").modal("hide");
                            break;
                        case 400:
                            handleBadRequest(body);
                            break;
                        case 409:
                            sweetAlert("warning", "Este e-mail já está em uso!");
                            break;
                        case 500:
                            sweetAlert("error", "Um erro desconhecido ocorreu ao cadastrar o usuário!");
                            break;
                    }
                    return
                }

                sweetAlertAwait("Salvando alterações");
                const { status, body } = await restAPI.patch(`v1/users/${+$("input[name=userId]").val()}`, {
                    name: $("input[name=name]").val().trim(),
                    email: $("input[name=email]").val().trim(),
                    courseId: +$("select[name=courseId]").val(),
                    role: +$("select[name=role]").val()
                });
                toggleExitConfirmation(false);

                switch (status) {
                    case 204:
                        await resetDataTable();
                        sweetAlert("success", "Usuário atualizado com sucesso!");
                        cleanAllFields();
                        $(".modal").modal("hide");
                        break;
                    case 400:
                        handleBadRequest(body);
                        break;
                    case 409:
                        sweetAlert("warning", "Este e-mail já está em uso!");
                        break;
                    case 500:
                        sweetAlert("error", "Um erro desconhecido ocorreu ao atualizar o usuário!");
                        break;
                }
                return;
            }

            if ($("input[name=entityId]").val().trim() == "") {
                sweetAlertAwait("Salvando registro");
                const description = $("input[name=description]").val().trim();

                const { status, body } = await restAPI.post(`v1/${entityName}`, { description });
                toggleExitConfirmation(false);

                switch (status) {
                    case 201:
                        await resetDataTable();
                        sweetAlert("success", "Registro salvo com sucesso!");
                        cleanAllFields();
                        $(".modal").modal("hide");
                        break;
                    case 400:
                        handleBadRequest(body);
                        break;
                    case 500:
                        sweetAlert("error", "Um erro desconhecido ocorreu ao salvar registro!");
                        break;
                }
                return;
            }

            sweetAlertAwait("Salvando alterações");
            const [entityId, description] = $(".modal form#entityForm input").toArray().map(input => input.value.trim());
            const { status, body } = await restAPI.patch(`v1/${entityName}/${+entityId}`, { description });
            toggleExitConfirmation(false);

            switch (status) {
                case 204:
                    await resetDataTable();
                    sweetAlert("success", "Registro atualizado com sucesso!");
                    cleanAllFields();
                    $(".modal").modal("hide");
                    break;
                case 400:
                    handleBadRequest(body);
                    break;
                case 500:
                    sweetAlert("error", "Um erro desconhecido ocorreu ao atualizar registro!");
                    break;
            }
        });

        $("button[data-target='#formModal']").click(() => {
            if (getCurrentEntityName() == "users") {
                $("#passwordInputs").show();
                $(".modal-body form#entityForm").hide();
                $(".modal-body form#userForm").show();
            } else {
                $(".modal-body form#entityForm").show();
                $(".modal-body form#userForm").hide();
            }
            cleanAllFields();
            $(".modal-title").html("Novo registro");
        });

        $("tbody").on("click", ".btn-edit", async function () {
            sweetAlertAwait("Buscando os dados deste registro");
            const entityName = getCurrentEntityName();
            const { data: entity } = await restAPI.get(`v1/${entityName}/${$(this).attr("id")}`);

            if (entityName == "users") {
                $("#passwordInputs").show();
                $(".modal-body form#entityForm").hide();
                $(".modal-body form#userForm").show();
                const { userId, name, email, courseId, role } = entity;
                $("#passwordInputs").hide();
                $("input[name=userId]").val(userId);
                $("input[name=name]").val(name);
                $("input[name=email]").val(email);
                $("select[name=courseId]").val(courseId);
                $("select[name=role]").val(role);
                $(".modal-title").html("Editar Usuário");
                $(".modal").modal("show");
            } else {
                $(".modal-body form#entityForm").show();
                $(".modal-body form#userForm").hide();
                const { areaId, courseId, description } = entity;
                $("input[name=entityId]").val(areaId ?? courseId);
                $("input[name=description]").val(description);
                $(".modal-title").html("Editar Registro");
                $(".modal").modal("show");
            }

            swal.close();
            toggleExitConfirmation(false);
        });

        $("tbody").on("click", ".btn-delete", async function () {
            Swal.fire({
                html: `<h2 style="color: white">Deseja mesmo excluir este registro?</h2>`,
                background: "rgb(70, 5, 7)",
                icon: "question",
                showCancelButton: true,
                allowOutsideClick: false,
                confirmButtonText: "Sim",
                confirmButtonColor: "#d9534f",
                cancelButtonText: "Não",
                cancelButtonColor: "#f0ad4e"
            }).then(async ({ value }) => {
                if (!value) return;

                sweetAlertAwait("Apagando registro");
                const entityName = getCurrentEntityName();
                const response = await restAPI.delete(`v1/${entityName}/${$(this).attr("id")}`);
                const notifications = response.body?.notifications;
                const { status } = response;

                toggleExitConfirmation(false);

                if (status == 204) {
                    await resetDataTable();
                    sweetAlert("success", "Registro apagado com sucesso!");
                    return;
                }

                if (status == 404 || status == 409) {
                    sweetAlert("warning", notifications[0]);
                    return;
                }

                sweetAlert("error", "Um erro desconhecido ocorreu ao apagar este registro!");
            });
        });
    });
})(jQuery);
