// (function ($) {
//     "use strict";

//     $(document).ready(() => {
//         screenExitTargetBlocker.onClickBlockingTargetAndLeavingFromScreen($(".btn-save"), async () => {
//             if (hasEmptyField($("form")))
//                 return;

//             project.title = $("input[name=title]").val().trim();
//             project.description = $("textarea[name=description]").val().trim() || null;
//             project.startedAt = $("input[name=startedAt]").val().trim();
//             project.finishedAt = $("input[name=finishedAt]").val().trim() || null;
//             project.areaId = +$("select[name=areaId]").val();
//             project.courseId = +$("select[name=courseId]").val();

//             if (!validateInvolvedUsersAndMediasOnProject()) return;

//             sweetAlertUtils.sweetAlertBlockingScreen("Salvando alterações...");

//             const formData = new FormData;
//             formData.append("title", project.title);
//             if (project.description) formData.append("description", project.description);
//             formData.append("startedAt", project.startedAt);
//             formData.append("finishedAt", project.finishedAt);
//             formData.append("areaId", project.areaId);
//             formData.append("courseId", project.courseId);
//             project.involvedUsers.forEach(id => formData.append("involvedUsers", id));
//             project.medias.forEach((media, i) => {
//                 if (media.mediaId) formData.append(`medias[${i}].mediaId`, media.mediaId);
//                 if (media.description) formData.append(`medias[${i}].description`, media.description);
//                 formData.append(`medias[${i}].title`, media.title);
//                 formData.append(`medias[${i}].file`, media.file || new File([], media.fileName));
//             });

//             const { statusCode, notifications } = await restApi.patchAsync(`v1/projects/${project.projectId}`, formData);
//             if (statusCode === null) {
//                 sweetAlertUtils.sweetAlertAsync("error", "Ocorreu um erro durante a atualização do projeto.");
//                 return;
//             }

//             if (statusCode !== 204) {
//                 showNotifications(notifications, statusCode);
//                 return;
//             }

//             await sweetAlertUtils.sweetAlertAsync("success", "Projeto salvo com sucesso!");
//             toggleAskBeforeExit(false);
//             location.reload();
//         });
//     });
// })(jQuery);
