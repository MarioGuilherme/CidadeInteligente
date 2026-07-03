(function ($) {
    "use strict";

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
            project.description = $("textarea[name=description]").val().trim() || null;
            project.startedAt = $("input[name=startedAt]").val().trim();
            project.finishedAt = $("input[name=finishedAt]").val().trim() || null;
            project.areaId = +$("select[name=areaId]").val();
            project.courseId = +$("select[name=courseId]").val();

            if (project.involvedUsers.length == 0) {
                sweetAlert("warning", "Por favor, selecione pelo menos uma pessoa envolvida no projeto.");
                return;
            }

            if (project.medias.length == 0) {
                sweetAlert("warning", "Por favor, anexe pelo menos uma mídia para o projeto.");
                return;
            }

            if (project.medias.length > 10) {
                sweetAlert("warning", "Por favor, anexe no máximo 10 mídias para o projeto.");
                return;
            }

            sweetAlertAwait("Criando projeto");

            const formData = new FormData;
            formData.append("title", project.title);
            if (project.description)
                formData.append("description", project.description);
            formData.append("startedAt", project.startedAt);
            formData.append("finishedAt", project.finishedAt);
            formData.append("areaId", project.areaId);
            formData.append("courseId", project.courseId);
            project.involvedUsers.forEach(id => formData.append("involvedUsers", id));
            project.medias.forEach((media, index) => {
                formData.append(`medias[${index}].title`, media.title);
                formData.append(`medias[${index}].file`, media.file);
                if (media.description)
                    formData.append(`medias[${index}].description`, media.description);
            });

            const { statusCode, body: { notifications }, headers } = await restAPI.postAsync("v1/projects", formData);
            toggleExitConfirmation(false);

            switch (statusCode) {
                case 201:
                    Swal.fire({
                        icon: "success",
                        html: `<div class="qrCode mb-1 d-flex justify-content-center"></div><h2 style="color:white;">Projeto criado com sucesso!</h2>`,
                        background: "rgb(70, 5, 7)",
                        allowOutsideClick: false
                    });
                    new QRCode($(".qrCode")[0], headers.get("Location"));
                    cleanAllFields();
                    $(".medias").empty();
                    project.title = project.description = project.date = project.areaId = project.courseId = null;
                    project.involvedUsers = project.medias = [];
                    break;
                case 400:
                    handleBadRequest(notifications);
                    break;
                case 500:
                    sweetAlert("error", notifications[0]);
                    break;
            }
        });
    });
})(jQuery);
