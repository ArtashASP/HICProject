$(document).ready(function () {
    /* $("#sec1").click(function () {
         var singleValues = $("#sec1").val();

         alert(singleValues)
     });*/

    $('#sel1').change(function () {
        var selval = $(this).val();
        var inp1 = $("#inp1").val();
        $.ajax({
            method: 'get',
            url: '../../api/ajax',
            data: { 'selval': selval, 'inp1': inp1 },
            success: function (data) {
            }
        })
    });
});