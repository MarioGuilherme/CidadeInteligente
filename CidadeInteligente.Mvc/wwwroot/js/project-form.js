(function ($) {
    "use strict";

    $(document).ready(() => {
        const project = {
            projectId: +$("input[name=projectId]").val() || null,
            title: $("input[name=title]").val().trim() || null,
            startedAt: $("input[name=startedAt]").val() || null,
            finishedAt: $("input[name=finishedAt]").val() || null,
            description: $("textarea[name=description]").val()?.trim() || null,
            areaId: +$("select[name=areaId]").val() || null,
            courseId: +$("select[name=courseId]").val() || null,
            involvedUsers: $(".user[involved=true]").toArray().map(e => +$(e).attr("id")),
            medias: $(".media").toArray().map(e => ({
                mediaId: +$(e).attr("id"),
                title: $(e).find("input").val().trim(),
                description: $(e).find("textarea").val()?.trim() || null,
                file: null,
                fileName: $(e).find("img, video").attr("src").split("/").at(-1),
                preview: {
                    extension: $(e).find("img, video").attr("src").split(".").at(-1),
                    path: $(e).find("img, video").attr("src")
                }
            }))
        };

        screenExitTargetBlocker.onClickBlockingTargetAndLeavingFromScreen($(".btn-save"), async () => {
            if (hasEmptyField($("form")))
                return;

            if (project.involvedUsers.length === 0) {
                sweetAlertUtils.sweetAlertAsync("warning", "Por favor, selecione pelo menos uma pessoa envolvida no projeto!");
                return;
            }

            if (project.medias.length === 0) {
                sweetAlertUtils.sweetAlertAsync("warning", "Por favor, anexe pelo menos uma mídia para o projeto!");
                return;
            }

            if (project.medias.length > MAX_MEDIAS) {
                sweetAlertUtils.sweetAlertAsync("warning", `Por favor, anexe no máximo ${MAX_MEDIAS} mídias para o projeto!`);
                return;
            }

            const projectId = +$("input[name=projectId]").val().trim() || null;
            const isUpdate = !!projectId;
            project.title = $("input[name=title]").val().trim() || null;
            project.description = $("textarea[name=description]").val().trim() || null;
            project.startedAt = $("input[name=startedAt]").val().trim();
            project.finishedAt = $("input[name=finishedAt]").val().trim() || null;
            project.areaId = +$("select[name=areaId]").val();
            project.courseId = +$("select[name=courseId]").val();

            sweetAlertUtils.sweetAlertBlockingScreen(`${isUpdate ? "Atualizando" : "Criando"} projeto`);

            const formData = new FormData;
            formData.append("title", project.title);
            if (project.description) formData.append("description", project.description);
            formData.append("startedAt", project.startedAt);
            formData.append("finishedAt", project.finishedAt);
            formData.append("areaId", project.areaId);
            formData.append("courseId", project.courseId);
            project.involvedUsers.forEach(involvedUserId => formData.append("involvedUsers", involvedUserId));
            project.medias.forEach((media, index) => {
                if (media.mediaId) formData.append(`medias[${index}].mediaId`, media.mediaId);
                if (media.description) formData.append(`medias[${index}].description`, media.description);
                formData.append(`medias[${index}].title`, media.title);
                formData.append(`medias[${index}].file`, media.file || new File([], media.fileName));
            });

            const { statusCode, notifications, headers } = isUpdate
                ? await restApi.patchAsync(`v1/projects/${projectId}`, formData)
                : await restApi.postAsync("v1/projects", formData);
            if (statusCode === null) {
                sweetAlertUtils.sweetAlertAsync("error", `Ocorreu um erro durante a ${isUpdate ? "atualização" : "criação"} do projeto.`);
                return;
            }

            if (statusCode !== 201 && statusCode !== 204) {
                showNotifications(notifications, statusCode);
                return;
            }

            if (isUpdate) {
                await sweetAlertUtils.sweetAlertAsync("success", "Projeto salvo com sucesso!");
                toggleAskBeforeExit(false);
                location.reload();
                return;
            }

            Swal.fire({
                icon: "success",
                html: `<div class="qrCode mb-1 d-flex justify-content-center"></div><h2 style="color:white;">Projeto criado com sucesso!</h2>`,
                background: "rgb(70, 5, 7)",
                allowOutsideClick: false
            });
            new QRCode($(".qrCode")[0], headers.get("Location"));
            clearAllFields();
            $(".medias").empty();
            project.title = project.description = project.startedAt = project.finishedAt = project.areaId = project.courseId = null;
            project.involvedUsers = [];
            project.medias = [];
        });


        const fileIsValidWithAlertReturn = ({ size, type }) => {
            const validExtensions = ["mp4", "png", "jpg", "jpeg"];
            const [mimeType, extension] = type.split("/");

            if (size > 4 * 1024 ** 2) {
                sweetAlertUtils.sweetAlertAsync("warning", "Por favor, selecione mídias com menos de 4MB!");
                return false;
            }

            if (mimeType !== "image" && mimeType !== "video") {
                sweetAlertUtils.sweetAlertAsync("warning", "É permitido apenas anexos do tipo vídeo e foto!");
                return false;
            }

            if (!validExtensions.includes(extension)) {
                sweetAlertUtils.sweetAlertAsync("warning", "Apenas mídias .jpg, .jpeg, .png e .mp4!");
                return false;
            }

            return true;
        };

        const MAX_MEDIAS = 10;
        let indexMediaToUpdate = 0;
        let pendingMedias = 0;

        $(".user").click(function () {
            $(this).attr("involved") === "true"
                ? project.involvedUsers.splice(project.involvedUsers.indexOf(+$(this).attr("id")), 1)
                : project.involvedUsers.push(+$(this).attr("id"));
            $(this).attr("involved", $(this).attr("involved") !== "true");
        });

        $(".btn-add-media").click(() => {
            if (project.medias.length + pendingMedias >= MAX_MEDIAS) {
                sweetAlertUtils.sweetAlertAsync("warning", `Limite de ${MAX_MEDIAS} mídias atingido.`);
                return;
            }
            $(".input-new-medias").click();
        });

        $(".medias").on("click", ".btn-change-media", function () {
            indexMediaToUpdate = [...$(".media")].indexOf($(this).parents(".media")[0]);
            $(".input-change-media").click();
        });

        $("input[name=startedAt]").change(() => {
            const startedAt = $("input[name=startedAt]").val();
            $("input[name=finishedAt]").attr("min", startedAt);
        });

        $("input[name=finishedAt]").change(() => {
            const startedAt = $("input[name=finishedAt]").val();
            $("input[name=startedAt]").attr("max", startedAt);
        });

        $(".medias").on("input", "input, textarea", function () {
            const i = [...$(".medias > div")].indexOf($(this).parents(".col-12")[0]);
            project.medias[i][$(this).prop("tagName") === "TEXTAREA" ? "description" : "title"] = $(this).val() || null;
        });

        $(".input-change-media").on("change", function () {
            const [file] = $(this)[0].files;
            $(this).val(null);

            if (!file || !fileIsValidWithAlertReturn(file)) return;

            const fileReader = new FileReader;
            fileReader.onloadend = ({ target: { result } }) => {
                const extension = file.type.split("/")[1];
                $($(".media")[indexMediaToUpdate]).find("img, video").remove();
                $($(".media")[indexMediaToUpdate]).find(".card-body").prepend(extension === "mp4"
                    ? `<video src="${result}" style="max-width: 100%" controls></video>`
                    : `<img src="${result}" class="img-fluid">`
                );

                project.medias[indexMediaToUpdate].preview.path = result.split(",")[1];
                project.medias[indexMediaToUpdate].preview.extension = extension;
                project.medias[indexMediaToUpdate].file = file;
            };

            fileReader.readAsDataURL(file);
        });

        $(".medias").on("click", ".btn-remove-media", function () {
            const indexMediaToDelete = [...$(".media")].indexOf($(this).parents(".media")[0]);
            setTimeout(() => $(this).parents(".col-12").remove(), 1000);
            $(this).parents(".col-12").hide(150);
            project.medias.splice(indexMediaToDelete, 1);
        });

        $(".input-new-medias").on("change", function () {
            const files = [...$(this)[0].files];
            $(this).val(null);

            let availableSlots = MAX_MEDIAS - project.medias.length - pendingMedias;
            if (availableSlots <= 0) {
                sweetAlertUtils.sweetAlertAsync("warning", `Limite de ${MAX_MEDIAS} mídias atingido.`);
                return;
            }

            let ignoredByLimit = 0;
            files.forEach(file => {
                if (availableSlots <= 0) {
                    ignoredByLimit++;
                    return;
                }

                if (!fileIsValidWithAlertReturn(file)) return;

                availableSlots--;
                pendingMedias++;

                const fileReader = new FileReader;
                fileReader.onloadend = ({ target: { result } }) => {
                    pendingMedias--;
                    if (!result) return;

                    const media = {
                        file,
                        title: file.name.split(".").slice(0, -1).join(".").slice(0, 60),
                        description: null,
                        preview: {
                            extension: file.type.split("/")[1],
                            path: result.split(",")[1]
                        }
                    };

                    $(".medias").append(`
                        <div class="col-12 col-sm-12 col-lg-3 col-md-3 my-3">
                            <div class="card media">
                                <div class="card-body">
                                    ${media.preview.extension === "mp4"
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
                                        <input required maxlength="60" class="form-control" type="text" value="${media.title}">
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
                };
                fileReader.readAsDataURL(file);
            });

            if (ignoredByLimit > 0)
                sweetAlertUtils.sweetAlertAsync("warning", `Você pode anexar no máximo ${MAX_MEDIAS} mídias. ${ignoredByLimit} arquivo(s) ignorado(s).`);
        });
    });
})(jQuery);
