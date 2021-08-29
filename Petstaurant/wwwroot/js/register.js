$(function () {
    $('form').submit(function (e) {
        var f = $('#bDate').val();
        console.log(f);
        var currentYear = new Date().getFullYear();
        var msg = $('#error_message');
        var year = f.split('-');
        console.log(year[0] > currentYear);
        console.log(year[0] < currentYear);
        if (currentYear-14 < year[0] || year[0] < currentYear - 120) {
            e.preventDefault();
            msg.text('You must be older than 14 or you enter invalid year');
        }
    });
});