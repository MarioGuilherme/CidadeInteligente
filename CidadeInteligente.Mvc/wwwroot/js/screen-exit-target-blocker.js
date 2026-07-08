class ScreenExitTargetBlocker {
    #baseCallBackAsync = async function (customCallbackAsync) {
        const $btn = $(this).prop("disabled", true);
        toggleAskBeforeExit(true);

        try {
            await customCallbackAsync();
        } catch (e) {
            console.error(e);
            sweetAlertUtils.sweetAlertAsync("error", "Ocorreu um erro inesperado!");
        }

        toggleAskBeforeExit(false);
        $btn.prop("disabled", false);
    };

    onClickBlockingTargetAndLeavingFromScreen = ($btn, customCallbackAsync) => {
        $btn.click(async e => {
            await this.#baseCallBackAsync(() => customCallbackAsync(e.currentTarget));
        });
    };

    onClickBlockingTargetAndLeavingFromScreenEventDelegate = ($baseElement, targetElement, customCallbackAsync) => {
        $baseElement.on("click", targetElement, async e => {
            await this.#baseCallBackAsync(() => customCallbackAsync(e.currentTarget));
        });
    };
}

const screenExitTargetBlocker = new ScreenExitTargetBlocker;
