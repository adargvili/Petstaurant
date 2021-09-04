$(function () {
    $('#userValidation').on("submit",function (e) {
        var f = $('#bDate').val();
        var currentYear = new Date().getFullYear();
        var msg = $('#bDateMsg');
        var year = f.split('-');
        if (currentYear-15 < year[0] || year[0] < currentYear - 120) {
            e.preventDefault();
            msg.text('You must be older than 14 or you entered invalid year');
        }
    });
});