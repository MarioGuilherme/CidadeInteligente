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
        $btn.click(async ({ currentTarget }) => {
            await this.#baseCallBackAsync(() => customCallbackAsync(currentTarget));
        });
    };

    onClickBlockingTargetAndLeavingFromScreenEventDelegate = ($baseElement, targetElement, customCallbackAsync) => {
        $baseElement.on("click", targetElement, async ({ currentTarget }) => {
            await this.#baseCallBackAsync(() => customCallbackAsync(currentTarget));
        });
    };
}

const screenExitTargetBlocker = new ScreenExitTargetBlocker;
