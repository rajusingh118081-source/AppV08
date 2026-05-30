var AjaxCall = {
    request: function(method, url, data, type) {
        type = type || "json";

        return $.ajax({
            url: url,
            method: method,
            dataType: type,
            data: method === "GET" ? data : JSON.stringify(data),
            contentType: method === "GET" ? undefined : "application/json; charset=utf-8",
            error: function (xhr, status, error) {
                console.error("AJAX Error:", status, error, xhr.responseText);
            }
        });
    },

    get: function (url, data, type) { return AjaxCall.request("GET", url, data, type); },
    post: function (url, data, type) { return AjaxCall.request("POST", url, data, type); },
    put: function (url, data, type) { return AjaxCall.request("PUT", url, data, type); },
    delete: function (url, data, type) { return AjaxCall.request("DELETE", url, data, type); }
};
