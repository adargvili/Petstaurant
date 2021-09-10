$(function () {
    $('#search-form').on("submit", function (e) {
        e.preventDefault();

        var queryUserName = encodeURIComponent($('#queryUserName').val());
        var phoneNumber = encodeURIComponent($('#phoneNumber').val());
        var orderNumber = encodeURIComponent($('#orderNumber').val());

        $('tbody').load('/Orders/Search?queryUserName=' + queryUserName + "&phoneNumber=" + phoneNumber + "&orderNumber=" + orderNumber);
    });
});