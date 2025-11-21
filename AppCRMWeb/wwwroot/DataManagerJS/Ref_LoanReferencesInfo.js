validation.init("#formLoanReferencesInfo");
function AddorUpdateLoanReferences() {
    if (!validation.isValid()) {
        validation.highlight();
    } else {
        $("#formLoanReferencesInfo").submit();
    }
}
$("#formLoanReferencesInfo").on("submit", function (event) {
    debugger;
    var $this = $(this);
    var frmValues = $this.serialize();
    if (!validation.isValid()) {
        validation.highlight();
    } else {
        $.ajax({
            type: $this.attr('method'),
            url: $this.attr('action'),
            data: frmValues
        }).done(function (data) {
            if (data.status == true) {
                $("formLoanReferencesInfo").trigger("reset");
                //var editor = $("#Description").data("kendoEditor");
                //editor.value("")
                toastr.success(data.message);
                $('#ModalPopup_Ref_LoanReferencesInfo').modal('toggle');
                $('#Table_Ref_LoanReferencesInfo').DataTable().ajax.reload();
                rowID = 0;
                LoadFuncation();
            } else {
                toastr.error(data.message);
            }
        }).fail(function (data) {
            toastr.error('Internal server error');
        });
    }
    event.preventDefault();
});