// (function ($) {
//     "use strict";

//     $(document).ready(() => {
//         screenExitTargetBlocker.onClickBlockingTargetAndLeavingFromScreen($(".btn-save"), async () => {
//             if (hasEmptyField($("form")))
//                 return;

//             const projectId = +$(".modal input[name=projectId]").val().trim() || null;
//             const isUpdate = !!projectId;

//             project.title = $("input[name=title]").val().trim() || null;
//             project.description = $("textarea[name=description]").val().trim() || null;
//             project.startedAt = $("input[name=startedAt]").val().trim();
//             project.finishedAt = $("input[name=finishedAt]").val().trim() || null;
//             project.areaId = +$("select[name=areaId]").val();
//             project.courseId = +$("select[name=courseId]").val();

//             if (!validateInvolvedUsersAndMediasOnProject()) return;

//             sweetAlertUtils.sweetAlertBlockingScreen(`${isUpdate ? "Atualizando" : "Criando"} projeto`);

//             const formData = new FormData;
//             formData.append("title", project.title);
//             if (project.description) formData.append("description", project.description);
//             formData.append("startedAt", project.startedAt);
//             formData.append("finishedAt", project.finishedAt);
//             formData.append("areaId", project.areaId);
//             formData.append("courseId", project.courseId);
//             project.involvedUsers.forEach(involvedUserId => formData.append("involvedUsers", involvedUserId));
//             project.medias.forEach((media, index) => {
//                 if (media.mediaId) formData.append(`medias[${index}].mediaId`, media.mediaId);
//                 if (media.description) formData.append(`medias[${index}].description`, media.description);
//                 formData.append(`medias[${index}].title`, media.title);
//                 formData.append(`medias[${index}].file`, media.file || new File([], media.fileName));
//             });

//             const { statusCode, notifications, headers } = isUpdate
//                 ? await restApi.patchAsync(`v1/projects/${projectId}`, formData)
//                 : await restApi.postAsync("v1/projects", formData);
//             if (statusCode === null) {
//                 sweetAlertUtils.sweetAlertAsync("error", `Ocorreu um erro durante a ${isUpdate ? "atualização" : "criação"} do projeto.`);
//                 return;
//             }

//             if (statusCode !== 201 && statusCode !== 204) {
//                 showNotifications(notifications, statusCode);
//                 return;
//             }

//             if (isUpdate) {
//                 await sweetAlertUtils.sweetAlertAsync("success", "Projeto salvo com sucesso!");
//                 toggleAskBeforeExit(false);
//                 location.reload();
//                 return;
//             }

//             Swal.fire({
//                 icon: "success",
//                 html: `<div class="qrCode mb-1 d-flex justify-content-center"></div><h2 style="color:white;">Projeto criado com sucesso!</h2>`,
//                 background: "rgb(70, 5, 7)",
//                 allowOutsideClick: false
//             });
//             new QRCode($(".qrCode")[0], headers.get("Location"));
//             clearAllFields();
//             $(".medias").empty();
//             project.title = project.description = project.startedAt = project.finishedAt = project.areaId = project.courseId = null;
//             project.involvedUsers = [];
//             project.medias = [];
//         });
//     });
// })(jQuery);
