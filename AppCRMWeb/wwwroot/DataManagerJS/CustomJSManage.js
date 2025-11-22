AjaxCall.get("/api/user", { id: 1 })
    .done(function (res) {
        console.log("GET Response:", res);
    });

AjaxCall.post("/api/user/create", { name: "John", age: 30 })
    .done(function (res) {
        console.log("POST Success:", res);
    });

AjaxCall.put("/api/user/update/5", { name: "Updated Name" })
    .done(function (res) {
        console.log("Updated:", res);
    });


AjaxCall.delete("/api/user/delete/5", {})
    .done(function (res) {
        console.log("Deleted:", res);
    });

