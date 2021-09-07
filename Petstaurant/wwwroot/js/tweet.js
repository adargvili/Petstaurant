
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
});