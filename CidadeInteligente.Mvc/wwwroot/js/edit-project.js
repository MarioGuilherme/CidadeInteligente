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
            const { statusCode, notifications } = await restAPI.patchAsync(`v1/projects/${project.projectId}`, formData);
            toggleExitConfirmation(false);

            switch (statusCode) {
                case 204:
                    sweetAlert("success", "Projeto salvo com sucesso!").then(({ value }) => value && location.reload());
                    break;
                case 400:
                    handleBadRequest(body);
                    break;
                case 403:
                case 404:
                    sweetAlert("warning", notifications[0]);
                    break;
                default:
                    sweetAlert("error", notifications[0]);
                    break;
            }
        });
    });
})(jQuery);
