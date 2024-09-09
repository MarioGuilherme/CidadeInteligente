$(document).ready(() => {
    setDataTable();

    const getCurrentEntityName = () => $("main a.active").attr("id").split("-")[0];
    const btns = id => `
        <button id=${id} class="btn btn-edit btn-strong-gray">
            <i class="fa fa-solid fa-pencil"></i> Editar
        </button>
        <button id=${id} class="btn btn-delete btn-strong-red">
            <i class="fa fa-solid fa-trash"></i> Apagar
        </button>
    `;
    const resetDataTable = async () => {
        const entityName = getCurrentEntityName();
        const entities = await api.get(`admin/${entityName}`);
        const dataTable = $(`div#${entityName} table`).DataTable();

        if (entities.length == 0) {
            dataTable.clear().draw();
            return;
        }

        dataTable.clear();
        entities.forEach(entity => {
            let row;
            if (entity.hasOwnProperty("userId")) {
                const { userId, name, email, course, role } = entity;
                row = [
                    userId,
                    name,
                    email,
                    course,
                    role,
                    btns(userId)
                ];
            } else {
                const { areaId, courseId, description } = entity;
                row = [areaId ?? courseId, description, btns(areaId ?? courseId)];
            }
            dataTable.row.add(row).draw();
        });
    }

    $(".btn-save").click(async () => {
        const entityName = getCurrentEntityName();
        if (entityName == "users") {
            if ($("input[name=userId]").val().trim() == "") {
                formHasEmptyField($("form#userForm").serializeArray().slice(1, -1));

                if ($($("input[type=password")[0]).val().trim() != $($("input[type=password")[1]).val().trim()) {
                    sweetAlert("error", "As senhas não conferem");
                    return;
                }
                
                sweetAlertAwait("Cadastrando usuário");
                const { status } = await api.post(`admin/${entityName}`, {
                    name: $("input[name=name]").val(),
                    email: $("input[name=email]").val(),
                    password: $("input[name=password]").val(),
                    courseId: +$("select[name=courseId]").val(),
                    role: +$("select[name=role]").val()
                });
                toggleExitConfirmation(false);

                if (status == 201) {
                    await resetDataTable();
                    sweetAlert("success", "Usuário cadastrado com sucesso!");
                    cleanAllFields();
                    $(".modal").modal("hide");
                } else
                    sweetAlert("error", "Erro ao cadastrar o usuário!");
            } else {
                sweetAlertAwait("Salvando alterações");
                const userId = +$("input[name=userId]").val();
                const { status } = await api.patch(`admin/users/${userId}`, {
                    userId,
                    name: $("input[name=name]").val(),
                    email: $("input[name=email]").val(),
                    courseId: +$("select[name=courseId]").val(),
                    role: +$("select[name=role]").val()
                });
                toggleExitConfirmation(false);

                if (status == 204) {
                    await resetDataTable();
                    sweetAlert("success", "Usuário atualizado com sucesso!");
                    cleanAllFields();
                    $(".modal").modal("hide");
                } else
                    sweetAlert("success", "Erro ao atualizar usuário");
            }
        } else {
            if ($("input[name=entityId]").val().trim() == "") {
                sweetAlertAwait("Salvando registro...");
                const description = $("input[name=description]").val().trim();
                const { status } = await api.post(`admin/${entityName}`, { description });
                toggleExitConfirmation(false);

                if (status == 201) {
                    await resetDataTable();
                    sweetAlert("success", "Registro salvo com sucesso!");
                    cleanAllFields();
                    $(".modal").modal("hide");
                } else
                    sweetAlert("error", "Erro ao salvar registro!");

            } else {
                sweetAlertAwait("Salvando alterações");
                const [entityId, description] = $(".modal form#entityForm input").toArray().map(input => input.value.trim());
                const { status } = await api.patch(`admin/${entityName}/${+entityId}`, { description });
                toggleExitConfirmation(false);

                if (status == 204) {
                    await resetDataTable();
                    sweetAlert("success", "Registro atualizado com sucesso!");
                    cleanAllFields();
                    $(".modal").modal("hide");
                } else
                    sweetAlert("error", "Erro ao atualizar o registro!");
            }
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

    $("tbody").on("click", ".btn-edit", async function() {
        sweetAlertAwait("Buscando os dados deste registro...");
        const entityName = getCurrentEntityName();
        const entity = await api.get(`admin/${entityName}/${$(this).attr("id")}`);

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

    $("tbody").on("click", ".btn-delete", async function() {
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

            sweetAlertAwait("Apagando registro...");
            const { status } = await api.delete(`admin/${getCurrentEntityName()}/${$(this).attr("id")}`);
            toggleExitConfirmation(false);

            if (status == 204) {
                await resetDataTable();
                sweetAlert("success", "Registro apagado com sucesso!");
            } else
                sweetAlert("error", "Erro ao apagar registro!");
        });
    });
});