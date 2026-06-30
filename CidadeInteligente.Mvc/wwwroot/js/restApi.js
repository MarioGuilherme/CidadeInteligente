class RestAPI {
    /**
     * URL Base para a WEB API Rest da aplicação
     * @type {string}
     */
    BASE_URL = "/api";

    async get(url) {
        return await fetch(`${this.BASE_URL}/${url}`, { method: "GET" })
            .then(response => response.json());
    }

    async post(url, data) {
        const request = await fetch(`${this.BASE_URL}/${url}`, {
            method: "POST",
            body: JSON.stringify(data),
            headers: {
                "Content-Type": "application/json"
            }
        });
        const body = await request.text();
        return { status: request.status, body: body == "" ? null : JSON.parse(body), headers: request.headers }
    }

    async postFormData(url, data) {
        const request = await fetch(`${this.BASE_URL}/${url}`, {
            method: "POST",
            body: data
        });
        const body = await request.text();
        return { status: request.status, body: body == "" ? null : JSON.parse(body), headers: request.headers }
    }

    async patchFormData(url, data) {
        const request = await fetch(`${this.BASE_URL}/${url}`, {
            method: "PATCH",
            body: data
        });
        const responseBody = await request.text();
        return { status: request.status, body: responseBody == "" ? null : JSON.parse(responseBody) }
    }

    async patch(url, body) {
        const request = await fetch(`${this.BASE_URL}/${url}`, {
            method: "PATCH",
            body: JSON.stringify(body),
            headers: {
                "Content-Type": "application/json"
            }
        });
        const responseBody = await request.text();
        return { status: request.status, body: responseBody == "" ? null : JSON.parse(responseBody) }
    }

    async delete(url) {
        const request = await fetch(`${this.BASE_URL}/${url}`, { method: "DELETE" });
        const responseBody = await request.text();
        return { status: request.status, body: responseBody == "" ? null : JSON.parse(responseBody) }
    }
}

const restAPI = new RestAPI;