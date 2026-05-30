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
        var url = $this.attr('action'),
        var data = frmValues;
        AjaxCall.postRequest(
            url,
            data,
            function (res) {
                console.log("POST success:", res);
            }
        );
    }
    event.preventDefault();
});