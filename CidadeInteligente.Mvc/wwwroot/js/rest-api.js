class RestApi {
    #BASE_URL = "/api";
    #BASE_RESPONSE = {
        statusCode: null,
        headers: null,
        data: null,
        notifications: null
    };

    #baseRequestAsync = async (url, method, data = null) => {
        const requestMetadata = this.#buildRequestMetadata(data);
        const cleanUrl = url.replace(/^\/+/, "");

        try {
            const response = await fetch(`${this.#BASE_URL}/${cleanUrl}`, {
                method,
                ...requestMetadata
            });

            return await this.#handleResponseAsync(response);
        } catch (error) {
            console.error(error);
            return this.#BASE_RESPONSE;
        }
    };

    #handleResponseAsync = async response => {
        const { status, headers } = response;
        const baseResponse = {
            ...this.#BASE_RESPONSE,
            statusCode: status,
            headers
        };

        const contentType = headers.get("content-type") ?? "";
        if (contentType.includes("application/json")) {
            const body = await response.json();
            baseResponse.data ??= body?.data;
            baseResponse.notifications ??= body?.notifications;
        }

        return baseResponse;
    };

    #buildRequestMetadata = data => {
        const request = {};

        if (!data)
            return request;

        if (data instanceof FormData) {
            request.body = data;
            return request;
        }

        if (typeof data === "object") {
            request.headers = {
                "Content-Type": "application/json",
                ...request.headers
            };
            request.body = JSON.stringify(data);
        }

        return request;
    };

    getAsync = async url => {
        return this.#baseRequestAsync(url, "GET");
    };

    postAsync = async (url, data) => {
        return this.#baseRequestAsync(url, "POST", data);
    };

    patchAsync = async (url, data) => {
        return this.#baseRequestAsync(url, "PATCH", data);
    };

    putAsync = async (url, data) => {
        return this.#baseRequestAsync(url, "PUT", data);
    };

    deleteAsync = async (url, data = null) => {
        return this.#baseRequestAsync(url, "DELETE", data);
    };
}

const restApi = new RestApi;
