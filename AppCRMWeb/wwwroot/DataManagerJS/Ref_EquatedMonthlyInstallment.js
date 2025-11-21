$('.defaultNumericValue').focusout(function () {
    var id = this.id
    if (id != null) {
        var inputValue = $("#" + id + "").val();
        if (inputValue == '') {
            var assignvalue = $("#" + id + "");
            assignvalue.val(0);
        }
    }
});
$('.defaultNumericValue').focus(function () {
    var id = this.id
    if (id != null) {
        var inputValue = $("#" + id + "").val();
        if (inputValue == "0") {
            var assignvalue = $("#" + id + "");
            assignvalue.val("");
        }
    }
});
validation.init("#formEquatedMonthlyInstallment");
$("#btnSaveEquatedMonthly").click(function (event) {
    var $this = $("#formEquatedMonthlyInstallment");
    var frmValues = $this.serialize();
    $.ajax({
        type: $this.attr('method'),
        url: $this.attr('action'),
        data: frmValues
    }).done(function (data) {
        if (data.status == true) {
            $("formEquatedMonthlyInstallment").trigger("reset");
            //var editor = $("#Description").data("kendoEditor");
            //editor.value("")
            toastr.success(data.message);
            $('#ModalPopup_Ref_EquatedMonthlyInstallment').modal('toggle');
            $('#Table_Ref_EquatedMonthlyInstallment').DataTable().ajax.reload();
            rowID = 0;
            LoadFuncation();
        } else {
            toastr.error(data.message);
        }
    }).fail(function (data) {
        toastr.error('Internal server error');
    });
    event.preventDefault();
});