
$(function () {
    $('#tweet-form').submit(function (e) {
        var toastLiveExample = $('#liveToast');
        var toast = new bootstrap.Toast(toastLiveExample);
        toast.show();
    });
});