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

(function ($) {
    "use strict";

    $(document).ready(() => {

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

        let indexMediaToUpdate = 0;

        $(".user").click(function () {
            $(this).attr("involved") == "true"
                ? project.involvedUsers.splice(project.involvedUsers.indexOf(+$(this).attr("id")), 1)
                : project.involvedUsers.push(+$(this).attr("id"));
            $(this).attr("involved", $(this).attr("involved") != "true");
        });

        $(".btn-add-media").click(() => {
            if (project.medias.length >= 10) {
                sweetAlert("warning", "Limite de dez mídias atingido.");
                return;
            };
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
            project.medias[i][$(this).prop("tagName") == "TEXTAREA" ? "description" : "title"] = $(this).val() || null;
        });

        $(".input-change-media").on("change", function () {
            const file = $(this)[0].files[0];
            const fileReader = new FileReader;

            fileReader.onloadend = ({ target: { result } }) => {
                if (!fileIsValidWithAlertReturn(file)) return;

                const extension = file.type.split("/")[1];
                $($(".media")[indexMediaToUpdate]).find("img, video").remove();
                $($(".media")[indexMediaToUpdate]).find(".card-body").prepend(extension == "mp4"
                    ? `<video src="${result}" style="max-width: 100%" controls></video>`
                    : `<img src="${result}" class="img-fluid">`
                );

                project.medias[indexMediaToUpdate].preview.path = result.split(",")[1];
                project.medias[indexMediaToUpdate].preview.extension = extension;
                project.medias[indexMediaToUpdate].file = file;
            }

            fileReader.readAsDataURL(file);
            $(this).val(null);
        });

        $(".medias").on("click", ".btn-remove-media", function () {
            const indexMediaToDelete = [...$(".media")].indexOf($(this).parents(".media")[0]);
            setTimeout(() => $(this).parents(".col-12").remove(), 1000);
            $(this).parents(".col-12").hide(500);
            project.medias.splice(indexMediaToDelete, 1);
        });

        $(".input-new-medias").on("change", function() {
            const files = $(this)[0].files;

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
                        file,
                        title: file.name.split(".").slice(0, -1).join(".").substr(0, 60),
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
                                    ${media.preview.extension == "mp4"
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
            $(this).val(null);
        });
    });
})(jQuery);
