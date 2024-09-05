class Api {
    async get(url) {
        return await fetch(`/API/${url}`, {
            method: "GET"
        }).then(response => response.json());
    }

    async post(url, data) {
        return await fetch(`/API/${url}`, {
            method: "POST",
            body: JSON.stringify(data),
            headers: {
                "Content-Type": "application/json"
            }
        });
    }

    async patch(url, body) {
        return await fetch(`/API/${url}`, {
            method: "PATCH",
            body: JSON.stringify(body),
            headers: {
                "Content-Type": "application/json"
            }
        });
    }

    async delete(url) {
        return await fetch(`/API/${url}`, {
            method: "DELETE"
        });
    }
}

const api = new Api;