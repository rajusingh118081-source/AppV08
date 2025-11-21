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


