(function ($) {
    "use strict";

    $(document).ready(() => {
        screenExitTargetBlocker.onClickBlockingTargetAndLeavingFromScreen($(".btn-delete-project"), async button => {
            const { value } = await sweetAlertUtils.sweetAlertQuestionAsync("Deseja mesmo excluir este projeto?");
            if (!value) return;

            sweetAlertUtils.sweetAlertBlockingScreen("Apagando projeto");
            const { statusCode, notifications } = await restApi.deleteAsync(`v1/projects/${$(button).attr("id")}`);
            if (statusCode === null) {
                sweetAlertUtils.sweetAlertAsync("error", "Ocorreu um erro durante a exclusão do projeto!");
                return;
            }

            if (statusCode !== 204) {
                showNotifications(notifications, statusCode);
                return;
            }

            sweetAlertUtils.sweetAlertAsync("success", "Projeto apagado com sucesso!");
            $(button).parents("div.col-12").hide(500);
            setTimeout(() => {
                $(button).parents("div.col-12").remove();
                if ($(".myProjects").find("div.col-12").length == 0 && $(".pages").find("a").length == 0)
                    $(".myProjects").html("<h3 class='text-center'>Você não está envolvido em nenhum projeto.</h3>");

                if ($(".myProjects").find("div.col-12").length == 0 && $(".pages").find("a").length > 0)
                    [...$(".pages a")].at(-1).click();
            }, 500);
        });
    });
})(jQuery);
