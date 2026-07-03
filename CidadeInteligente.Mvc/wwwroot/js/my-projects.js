(function ($) {
    "use strict";

    $(document).ready(() => {
        $(".btn-delete-project").click(async function () {
            const { value } = await Swal.fire({
                html: `<h2 style="color: white;">Deseja mesmo excluir este projeto?</h2>`,
                background: "rgb(70, 5, 7)",
                icon: "question",
                showCancelButton: true,
                allowOutsideClick: false,
                confirmButtonText: "Sim",
                confirmButtonColor: "#d9534f",
                cancelButtonText: "Não",
                cancelButtonColor: "#f0ad4e"
            });

            if (!value) return;

            sweetAlertAwait("Apagando projeto");
            const { statusCode, notifications } = await restAPI.deleteAsync(`v1/projects/${$(this).attr("id")}`);
            toggleExitConfirmation(false);

            switch (statusCode) {
                case 204:
                    sweetAlert("success", "Projeto apagado com sucesso!");
                    $(this).parents("div.col-12").hide(500);
                    setTimeout(() => {
                        $(this).parents("div.col-12").remove();
                        if ($(".myProjects").find("div.col-12").length == 0 && $(".pages").find("a").length == 0) // Se há não mais projetos nestá única página, aparece uma mensagem de que não há projetos
                            $(".myProjects").html("<h3 class='text-center'>Você não está envolvido em nenhum projeto.</h3>");

                        if ($(".myProjects").find("div.col-12").length == 0 && $(".pages").find("a").length > 0) // Se há não houver mais projetos nesta página, vai para a página anterior
                            [...$(".pages a")].at(-1).click();
                    }, 500);
                    break;
                case 403:
                case 404:
                    sweetAlert("warning", notifications[0]);
                    break;
                default:
                    sweetAlert("error", "Erro ao apagar projeto!");
                    break;
            }
        })
    });
})(jQuery);
