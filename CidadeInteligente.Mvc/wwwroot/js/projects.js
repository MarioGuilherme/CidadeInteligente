(function ($) {
    "use strict";

    $(document).ready(() => {
        const element = $(".title")[0];
        const textoArray = element.innerHTML.split("");
        element.innerHTML = " ";
        textoArray.forEach((letra, i) => setTimeout(() => element.innerHTML += letra, 75 * i));
    });
})(jQuery);
