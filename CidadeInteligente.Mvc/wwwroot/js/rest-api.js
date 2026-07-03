class RestAPI {
    /**
     * URL Base para a API Rest da aplicação
     * @type {string}
     */
    BASE_URL = "/api";

    baseRequestAsync = async (url, method, data = null) => {
        const requestMetadata = this.buildRequestMetadata(data, method);

        const response = await fetch(`${this.BASE_URL}/${url}`, {
            method,
            ...requestMetadata
        });

        return await this.handleResponseAsync(response);
    }

    handleResponseAsync = async response => {
        const responseBody = await response.text();
        const baseResponse = {
            statusCode: response.status,
            headers: response.headers,
            body: {
                data: null,
                notifications: null
            }
        };

        const body = responseBody == "" ? null : JSON.parse(responseBody);
        if (body) {
            baseResponse.data = body.data;
            baseResponse.notifications = body.notifications;
        }

        return baseResponse;
    }

    buildRequestMetadata = data => {
        const request = { headers: null, body: null };

        if (!data) {
            delete request.headers;
            return request;
        }

        if (data instanceof FormData) {
            delete request.headers;
            request.body = data;
            return request;
        }

        if (typeof data == "object") {
            request.headers = {
                "Content-Type": "application/json"
            };
            request.body = JSON.stringify(data);
        }

        return request;
    }

    async getAsync(url) {
        return await this.baseRequestAsync(url, "GET");
    }

    async postAsync(url, data) {
        return await this.baseRequestAsync(url, "POST", data);
    }

    async patchAsync(url, data) {
        return await this.baseRequestAsync(url, "PATCH", data);
    }

    async deleteAsync(url) {
        return await this.baseRequestAsync(url, "DELETE");
    }
}

const restAPI = new RestAPI;
