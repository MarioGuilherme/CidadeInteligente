class SweetAlertUtils {
    #BASE_SWEET_ALERT = {
        icon: "info",
        background: "rgb(70, 5, 7)",
        allowOutsideClick: false
    };

    #buildSweetAlertHtml = (message, icon = null) =>
        `<h2 class="mb-1 d-flex justify-content-center" style=color:white>${icon === "success" ? "Feito!" : icon === "question" ? "Confirme!" : "Ops!"}</h2><div style=color:white>${message}</div>`;

    sweetAlertAsync = async (icon, message) => await Swal.fire({
        ...this.#BASE_SWEET_ALERT,
        icon,
        html: this.#buildSweetAlertHtml(message, icon),
    });

    sweetAlertQuestionAsync = async question => await Swal.fire({
        ...this.#BASE_SWEET_ALERT,
        icon: "question",
        html: this.#buildSweetAlertHtml(question, "question"),
        showCancelButton: true,
        confirmButtonText: "Sim",
        confirmButtonColor: "#d9534f",
        cancelButtonText: "Não",
        cancelButtonColor: "#f0ad4e"
    });

    sweetAlertBlockingScreen = message => Swal.fire({
        ...this.#BASE_SWEET_ALERT,
        html: `<div class="qrCode mb-1 d-flex justify-content-center"></div><h2 style="color:white;">${message}, aguarde...</h2>`,
        showConfirmButton: false
    });
}

const sweetAlertUtils = new SweetAlertUtils;
