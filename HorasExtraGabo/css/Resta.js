$(function () {

    $("#horaentrada,#horasalida").keyup(function (e) {

        var q = $("#horaentrada").val();
        var p = $("#horasalida").val();
        var result = "";

        if (q !== "" && p !== "" && $.isNumeric(q) && $.isNumeric(p)) {
            result = parseInt(p) - parseInt(q);
        }
        $("#hvalidas").val(result);

    });

});