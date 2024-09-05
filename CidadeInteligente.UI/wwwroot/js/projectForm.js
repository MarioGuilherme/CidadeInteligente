$(document).ready(() => {
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

    $(".medias").on("click", ".btn-change-media", function() {
        indexMediaToUpdate = [...$(".media")].indexOf($(this).parents(".media")[0]);
        $(".input-change-media").click();
    });

    $(".medias").on("input", "input, textarea", function () {
        const i = [...$(".medias > div")].indexOf($(this).parents(".col-12")[0]);
        project.medias[i][$(this).prop("tagName") == "TEXTAREA" ? "description" : "title"] = $(this).val() || null;
    });

    $(".input-change-media").on("change", function() {
        const file = $(".input-change-media")[0].files[0];
        const fileReader = new FileReader;

        fileReader.onloadend = ({ target: { result } }) => {
            if (!fileIsValidWithAlertReturn(file)) return;

            const extension = file.type.split("/")[1];
            $($(".media")[indexMediaToUpdate]).find("img, video").remove();
            $($(".media")[indexMediaToUpdate]).find(".card-body").prepend(
                extension == "mp4"
                    ? `<video src="${result}" style="max-width: 100%" controls></video>`
                    : `<img src="${result}" class="img-fluid">`
            );

            delete project.medias[indexMediaToUpdate].path;
            project.medias[indexMediaToUpdate].base64 = result.split(",")[1];
            project.medias[indexMediaToUpdate].extension = extension;
        }
        fileReader.readAsDataURL(file);
        $(".input-change-media").val(null);
    });
});