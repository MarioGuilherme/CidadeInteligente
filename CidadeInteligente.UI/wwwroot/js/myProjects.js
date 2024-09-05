$(document).ready(() => {
    $(".btn-delete-project").click(function() {
        Swal.fire({
            html: `<h2 style="color: white;">Deseja mesmo excluir este projeto?</h2>`,
            background: "rgb(70, 5, 7)",
            icon: "question",
            showCancelButton: true,
            allowOutsideClick: false,
            confirmButtonText: "Sim",
            confirmButtonColor: "#d9534f",
            cancelButtonText: "Não",
            cancelButtonColor: "#f0ad4e"
        }).then(async ({ value }) => {
            if (value) {
                sweetAlertAwait("Apagando projeto");
                const { status } = await api.delete(`projects/${$(this).attr("id")}`);
                window.onbeforeunload = () => {}; // Desativa o alert de confirmação de saída

                switch (status) {
                    case 204:
                        sweetAlert("success", "Projeto apagado com sucesso!");
                        $(this).parents("div.col-12").hide(500);
                        setTimeout(() => {
                            $(this).parents("div.col-12").remove();
                            if ($(".myProjects").find("div.col-12").length == 0 && $(".pages").find("a").length == 0) // Se há não mais projetos nestá única página, aparece uma mensagem de que não há projetos
                                $(".myProjects").html("<h3 class='text-center'>Você não está participando de nenhum projeto ainda.</h3>");

                            if ($(".myProjects").find("div.col-12").length == 0 && $(".pages").find("a").length > 0) // Se há não hour mais projetos nesta página, vai para a página anterior
                                [...$(".pages a")].at(-1).click();
                        }, 500);
                        break;
                    case 404:
                        sweetAlert("error", "Este projeto não existe mais!");
                        break;
                }
            }
        });
    })
});