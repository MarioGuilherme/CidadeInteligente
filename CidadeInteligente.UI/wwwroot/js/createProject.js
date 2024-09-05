const project = {
    title: null,
    description: null,
    startedAt: null,
    finishedAt: null,
    areaId: null,
    courseId: null,
    involvedUsers: [],
    medias: []
};
$(document).ready(() => {
    $(".btn-new-project").click(async () => {
        formHasEmptyField([
            ...$(".medias").find("input"),
            $("input[name=title]")[0],
            $("input[name=startedAt]")[0],
            $("select[name=courseId]")[0],
            $("select[name=areaId]")[0]
        ]);

        project.title = $("input[name=title]").val().trim();
        project.description = $("textarea[name=description]").val().trim() || null;
        project.startedAt = $("input[name=startedAt]").val().trim();
        project.finishedAt = $("input[name=finishedAt]").val().trim() || null;
        project.areaId = +$("select[name=areaId]").val();
        project.courseId = +$("select[name=courseId]").val();

        if (project.involvedUsers.length == 0) {
            sweetAlert("warning", "Por favor, selecione pelo menos uma pessoa envolvido no projeto.");
            return;
        }

        if (project.involvedUsers.length == 0 || project.medias.length == 0) {
            sweetAlert("warning", "Por favor, anexe pelo menos uma mídia para o projeto.");
            return;
        }

        if (project.medias.length > 10) {
            sweetAlert("warning", "Por favor, anexe no máximo 10 mídias para o projeto.");
            return;
        }
     
        sweetAlertAwait("Criando projeto...");
        const { status, headers } = await api.post("projects", project);
        window.onbeforeunload = () => {};

        if (status == 201) {
            Swal.fire({
                icon: "success",
                html: `<div class="qrCode mb-1 d-flex justify-content-center"></div><h2 style="color:white;">Projeto criado com sucesso!</h2>`,
                background: "rgb(70, 5, 7)",
                allowOutsideClick: false
            });
            new QRCode($(".qrCode")[0], headers.get("Location"));
            cleanAllFields();
            $(".medias").empty();
            project.title = null;
            project.date = null;
            project.description = null;
            project.areaId = null;
            project.courseId = null;
            project.involvedUsers = [];
            project.medias = [];
        } else
            sweetAlert("error", "Erro ao criar projeto.");
    });

    $(".medias").on("click", ".btn-remove-media", function() {
        const indexMediaToDelete = [...$(".media")].indexOf($(this).parents(".media")[0]);
        setTimeout(() => $(this).parents(".col-12").remove(), 1000);
        $(this).parents(".col-12").hide(500);
        project.medias.splice(indexMediaToDelete, 1);
    });

    $(".input-new-medias").on("change", function() {
        const files = $(".input-new-medias")[0].files;

        for (let i = 0; i < files.length; i++) {
            const file = files[i];
            const fileReader = new FileReader;

            fileReader.onloadend = ({ target: { result } }) => {
                if (project.medias.length == 10) {
                    sweetAlert("warning", "Limite de 10 mídias atingido.");
                    return;
                }

                if (!fileIsValidWithAlertReturn(file)) return;

                const media = {
                    title: file.name.split(".").slice(0, -1).join(".").substr(0, 60),
                    description: null,
                    extension: file.type.split("/")[1],
                    base64: result.split(",")[1]
                };

                $(".medias").append(`
                    <div class="col-12 col-sm-12 col-lg-3 col-md-3 my-3">
                        <div class="card media">
                            <div class="card-body">
                                ${media.extension == "mp4"
                                    ? `<video src="${result}" style="max-width: 100%" controls></video>`
                                    : `<img src="${result}" class="d-block w-100">`
                                }
                                <div class="row my-1">
                                    <div class="col-12 col-lg-6">
                                        <button type="button" class="btn btn-change-media w-100 btn-warning my-1">
                                            <i class="mdi mdi-pencil"></i>
                                            Alterar
                                        </button>
                                    </div>
                                    <div class="col-12 col-lg-6">
                                        <button type="button" class="btn w-100 btn-remove-media btn-default-red my-1">
                                            <i class="mdi mdi-trash-can-outline"></i>
                                            Apagar
                                        </button>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label>Nome</label>
                                    <input maxlength="60" class="form-control" type="text" value="${media.title}">
                                </div>
                                <div class="form-group">
                                    <label>Descrição</label>
                                    <textarea maxlength="300" class="form-control" rows="5"></textarea>
                                </div>
                            </div>
                        </div>
                    </div>
                `);
                project.medias.push(media);
            }
            fileReader.readAsDataURL(file);
        }
        $(".input-new-medias").val(null);
    });
});