window.application = window.application || {};
window.application.services = window.application.services || {};

window.application.services.baseService = (() => {
    const getAuthToken = () => localStorage.getItem('token');

    const getRequest = (url) => request(url);
    const postRequest = (url, body) => request(url, {
        method: 'POST',
        body: JSON.stringify(body)
    });
    const putRequest = (url, body) => request(url, {
        method: 'PUT',
        body: JSON.stringify(body)
    });
    const deleteRequest = (url, body) => request(url, {
        method: 'DELETE',
        body: JSON.stringify(body)
    });
    const request = (url, options) => {
        const authToken = getAuthToken();

        options.headers = {
            'Content-Type': 'application/json'
        };

        options.headers = authToken ? { ...options.headers, 'Authorization': 'Bearer ' + authToken} : options.headers;

        return fetch(url, options)
            .then(res => {
                if (res.status >= 200 && res.status < 300) {
                    return res.json();
                } else {
                    return res.json().then(Promise.reject.bind(Promise));
                }
            });
    }

    return {
        get: getRequest,
        post: postRequest,
        put: putRequest,
        delete: deleteRequest
    };
})();
