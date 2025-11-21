/* Extend jQuery with functions for PUT and DELETE requests. */

function _ajax_request(url, data, callback, type, method) {
    if (jQuery.isFunction(data)) {
        callback = data;
        data = {};
    }

    return jQuery.ajax({
        type: method,
        url: url,
        data: data,
        contentType: "application/json; charset=utf-8",
        success: callback,
        dataType: type,
        error: function (request, msg, error) {
            console.log(request);
            console.log('--------------------------');
            console.log(msg);
            console.log(error);
        }
    });
}

var AjaxCall = ({
    postRequest: function (url, data, callback, type) {
        return _ajax_request(url, data, callback, type, 'post');
    },
    getRequest: function (url, data, callback, type) {
        return _ajax_request(url, data, callback, type, 'get');
    },
    putRequest: function (url, data, callback, type) {
        return _ajax_request(url, data, callback, type, 'PUT');
    },
    deleteRequest: function (url, data, callback, type) {
        return _ajax_request(url, data, callback, type, 'DELETE');
    }
});
