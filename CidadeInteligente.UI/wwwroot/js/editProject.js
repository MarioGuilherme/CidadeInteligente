const project = {
    projectId: +$("input[name=projectId]").val(),
    title: $("input[name=title]").val(),
    startedAt: $("input[name=startedAt]").val(),
    finishedAt: $("input[name=finishedAt]").val() || null,
    description: $("textarea[name=description]").val() || null,
    areaId: +$("select[name=areaId]").val(),
    courseId: +$("select[name=courseId]").val(),
    involvedUsers: $(".user[involved]").toArray().map(e => +$(e).attr("id")),
    medias: $(".media").toArray().map(e => ({
        mediaId: +$(e).attr("id"),
        title: $(e).find("input").val(),
        description: $(e).find("textarea").val() || null,
        extension: $(e).find("img, video").attr("src").split(".").at(-1),
        path: $(e).find("img, video").attr("src")
    }))
};

$(document).ready(() => {

    $(".btn-save").click(async () => {
        formHasEmptyField([
            ...$(".medias").find("input"),
            $("input[name=title]")[0],
            $("input[name=startedAt]")[0],
            $("select[name=courseId]")[0],
            $("select[name=areaId]")[0]
        ]);

        project.title = $("input[name=title]").val().trim();
        project.startedAt = $("input[name=startedAt]").val().trim();
        project.finishedAt = $("input[name=finishedAt]").val().trim() || null;
        project.description = $("textarea[name=description]").val().trim() || null;
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
   
        sweetAlertAwait("Salvando alterações...");
        const { status } = await api.patch(`projects/${+$("input[name=projectId]").val()}`, project);
        toggleExitConfirmation(false);

        switch (status) {
            case 204:
                sweetAlert("success", "Projeto salvo com sucesso!").then(({ value }) => value && location.reload());
                break;
            case 404:
                sweetAlert("warning", "Esse projeto não existe mais!");
                break;
            default:
                sweetAlert("error", "Erro ao salvar alterações!");
                break;
        }
    });

    $(".medias").on("click", ".btn-remove-media", function() {
        const mediaId = +$(this).attr("id");
        if (!isNaN(mediaId)) {
            for (let i = 0; i < project.medias.length; i++)
                if (project.medias[i].mediaId == mediaId) {
                    project.medias.splice(i, 1);
                    break;
                }
            setTimeout(() => $(`.media[id=${mediaId}]`).parent().remove(), 1000);
            $(`.media[id=${mediaId}]`).parent().hide(500);
        } else {
            const i = [...$(".media")].indexOf($(this).parents(".media")[0]);
            setTimeout(() => $(this).parents(".col-12").remove(), 1000);
            $(this).parents(".col-12").hide(500);
            project.medias.splice(i, 1);
        }
    });

    $(".input-new-medias").on("change", function() {
        const files = $(".input-new-medias")[0].files;

        for (let i = 0; i < files.length; i++) {
            const fileReader = new FileReader;
            const file = files[i];

            fileReader.onloadend = ({ target: { result } }) => {
                if (project.medias.length == 10) {
                    sweetAlert("warning", "Limite de 10 mídias atingido.");
                    return;
                }

                if (fileIsValidWithAlertReturn(file)) {
                    const media = {
                        title: file.name.substr(0, file.name.lastIndexOf(".")) || file.name,
                        extension: file.type.split("/")[1],
                        description: null,
                        base64: result.split(",")[1]
                    };

                    $(".medias").append(`
                        <div class="col-12 col-sm-12 col-lg-3 col-md-3 my-3">
                            <div class="card media">
                                <div class="card-body">
                                    ${media.extension == "mp4"
                                        ? `<video src="${result}" style="max-width: 100%;" controls></video>`
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
                                        <label>
                                            Nome
                                        </label>
                                        <input maxlength="60" class="form-control" type="text" value="${file.name.substr(0, file.name.lastIndexOf(".")) || file.name}">
                                    </div>
                                    <div class="form-group">
                                        <label>
                                            Descrição
                                        </label>
                                        <textarea maxlength="200" class="form-control" rows="5"></textarea>
                                    </div>
                                </div>
                            </div>
                        </div>
                    `);
                    project.medias.push(media);
                }
            }
            fileReader.readAsDataURL(file);
        }
        $(".input-new-medias").val(null);
    });
});