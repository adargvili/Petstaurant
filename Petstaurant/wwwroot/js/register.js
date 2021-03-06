$(function () {
    $('#regValidation').on("submit",function (e) {
        var f = $('#bDate').val();
        var currentYear = new Date().getFullYear();
        var msg = $('#bDateMsg');
        var year = f.split('-');
        if (currentYear-15 < year[0] || year[0] < currentYear - 120) {
            e.preventDefault();
            msg.text('You must be older than 14 or you entered invalid year');
        }

        var em = $('#email').val();
        var emE = $('#emailError');
        if (!(em.endsWith(".com") || em.endsWith(".co.il") || em.endsWith(".jp"))) {
            e.preventDefault();
            emE.text('Email Address must belong to .com/.co.il/.jp domains');
        }
    });
});