$(function () {
    $('form').submit(function (e) {
        var f = $('#bDate').val();
        var currentYear = new Date().getFullYear();
        var msg = $('#bDateMsg');
        var year = f.split('-');
        if (currentYear-14 < year[0] || year[0] < currentYear - 120) {
            e.preventDefault();
            msg.text('You must be older than 14 or you enter invalid year');
        }

        var em = $('#email').val();
        var emE = $('#emailError');
        if (!em.endsWith(".com")) {
            e.preventDefault();
            emE.text('Email Address must belong to .com')
        }
    });
});