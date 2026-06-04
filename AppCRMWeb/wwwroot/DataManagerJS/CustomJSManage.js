var columnTable = [];
const ManageColumns = (controllerName, fieldName) => {
    switch (controllerName) {
        case "Ref_EquatedMonthlyInstallment":
            EquatedMonthlyInstallment(fieldName);
            break;
        default:
            columnTable.push({ "data": fieldName, "name": fieldName, "autoWidth": true });
    }
}

const EquatedMonthlyInstallment = (fieldName) => {
    if (fieldName == "refPaymentStatusName") {
        columnTable.push({
            "data": "refPaymentStatusName",
            "searchable": false,
            "sortable": false,
            "render": function (data, type, full, meta) {
                if (data == "Done") {
                    return '<button type="button" class="btn btn-success rounded-pill">' + data + '</button>';
                } else {
                    return '<button type="button" class="btn btn-warning rounded-pill">' + data + '</button>';
                }
            }
        });
    } else {
        columnTable.push({ "data": fieldName, "name": fieldName, "autoWidth": true });
    }
}

function CapToLowerCase(str) {
    return str
        .replace(/^[^ ]/g, match => (match.toLowerCase()));
}

function LoadDataTable() {

    var columns = [];

    $.each(jsonData, function (key, value) {
        var fieldName = value.fieldName;

        if (fieldName === "ID") {
            fieldName = "id";
        } else {
            fieldName = CapToLowerCase(fieldName);
        }

        columns.push({
            data: fieldName,
            name: value.fieldName,
            autoWidth: true
        });
    });
    $.ajax({
        type: "post",
        url: dataUrl,
    }).done(function (data) {
       
    }).fail(function (data) {
        toastr.error('Internal server error');
    });
    const tableId = "#" + ControllerName;

    // destroy old table safely
    if ($.fn.DataTable.isDataTable(tableId)) {
        $(tableId).DataTable().destroy();
        $(tableId + " tbody").empty();
    }

    // initialize DataTable
    var table = $(tableId).DataTable({
        stateSave: true,
        processing: true,
        serverSide: true,
        filter: true,

        ajax: {
            url: dataUrl,
            type: "POST",
            dataType: "json",

            beforeSend: function (xhr, settings) {
                debugger;   // 👈 stops BEFORE request is sent
                console.log("About to call:", settings.url);
            },

            data: function (d) {
                d.status = $("#statusFilter").val();
                return d;
            }
        },

        language: {
            emptyTable: "No record found.",
            processing: `
                <i class="fa fa-spinner fa-spin fa-3x fa-fw"></i>
                <span class="sr-only">Loading...</span>
            `
        },

        columnDefs: [
            {
                targets: [0],
                visible: false,
                searchable: false
            }
        ],

        columns: columns
    });

    // row click event (attach AFTER init)
    $(tableId + " tbody").off("click").on("click", "tr", function () {

        var data = table.row(this).data();

        if (!data) return;

        table.$("tr.selected").removeClass("selected");
        $(this).addClass("selected");

        rowID = data.uniqueNumber || data.id;

        LoadFuncation();

        console.log("Selected Row:", data);
        console.log("Row ID:", rowID);
    });
}

// AjaxCall.get("/api/user", { id: 1 })
//     .done(function (res) {
//         console.log("GET Response:", res);
//     });

// AjaxCall.post("/api/user/create", { name: "John", age: 30 })
//     .done(function (res) {
//         console.log("POST Success:", res);
//     });

// AjaxCall.put("/api/user/update/5", { name: "Updated Name" })
//     .done(function (res) {
//         console.log("Updated:", res);
//     });


// AjaxCall.delete("/api/user/delete/5", {})
//     .done(function (res) {
//         console.log("Deleted:", res);
//     });

