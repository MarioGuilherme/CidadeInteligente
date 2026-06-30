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
            const formData = new FormData;
            formData.append("projectId", project.projectId);
            formData.append("title", project.title);
            if (project.description) formData.append("description", project.description);
            formData.append("startedAt", project.startedAt);
            formData.append("finishedAt", project.finishedAt);
            formData.append("areaId", project.areaId);
            formData.append("courseId", project.courseId);
            project.involvedUsers.forEach(id => formData.append("involvedUsers", id));
            project.medias.forEach((media, i) => {
                if (media.mediaId) formData.append(`medias[${i}].mediaId`, media.mediaId);
                if (media.description) formData.append(`medias[${i}].description`, media.description);
                formData.append(`medias[${i}].title`, media.title);
                formData.append(`medias[${i}].file`, media.file || new File([], media.fileName));
            });
            const { status, body } = await restAPI.patchFormData(`v1/projects/${project.projectId}`, formData);
            toggleExitConfirmation(false);

            switch (status) {
                case 204:
                    sweetAlert("success", "Projeto salvo com sucesso!").then(({ value }) => value && location.reload());
                    break;
                case 400:
                    handleBadRequest(body);
                    break;
                case 403:
                    sweetAlert("warning", "Você não está envolvido e nem é o criador deste projeto!");
                    break;
                case 404:
                    sweetAlert("warning", "Esse projeto não existe mais!");
                    break;
                default:
                    sweetAlert("error", "Erro ao salvar alterações!");
                    break;
            }
        });
    });
})(jQuery);
