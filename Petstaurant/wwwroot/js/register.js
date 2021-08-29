$(function () {
    $('form').submit(function (e) {
        var f = $('#foo').val();
        var msg = $('#error_message');

        if (f.length == 0) {
            e.preventDefault();

            msg.text('The field foo cannot be empty');
        }
    });
});