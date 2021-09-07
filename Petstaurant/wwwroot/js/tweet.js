
$(function () {


    $('#tweetData').keypress(function () {

        if (this.value.length > 280) {
            $('#tweetData').val($('#tweetData').val().substring(0, 280));
            $("#remainingC").text("Remaining characters : 0");
            return false;
        }

        $("#remainingC").text("Remaining characters : " + (280 - this.value.length));
    });

    $('#tweetData').keyup(function () {

        if (this.value.length > 280) {
            $('#tweetData').val($('#tweetData').val().substring(0, 280));
            $("#remainingC").text("Remaining characters : 0");
            return false;
        }

        $("#remainingC").text("Remaining characters : " + (280 - this.value.length));
    });

    $('#tweetData').keydown(function () {

        if (this.value.length > 280) {
            $('#tweetData').val($('#tweetData').val().substring(0, 280));
            $("#remainingC").text("Remaining characters : 0");
            return false;
        }

        $("#remainingC").text("Remaining characters : " + (280 - this.value.length));
    });

    $('#tweet-form').on("submit", function (e) {

        e.preventDefault();
        var checkLength = $('#tweetData').val();
        if (checkLength.length == 0) {
            $('#onSuccess').html('<h6 class="text-danger">Invalid Tweet</h6>')
            setTimeout(function () {
                $('#onSuccess').removeClass('text-danger');
                $('#onSuccess').text('');
            }, 5000);
        }
        else {
            var tweetValue = encodeURIComponent($('#tweetData').val());
            $('#tweetData').val('');
            $("#remainingC").text("Remaining characters : 280");
            setTimeout(function () {
                $('#onSuccess').text('');
            }, 10000);
            $('#onSuccess').load('/Users/Tweet?tweetData=' + tweetValue);
        }

    });


});