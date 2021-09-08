$(function () {
    $('#search-form').on("submit", function (e) {
        e.preventDefault();

        var queryUserName = encodeURIComponent($('#queryUserName').val());
        var phoneNumber = encodeURIComponent($('#phoneNumber').val());
        var postalCode = encodeURIComponent($('#postalCode').val());

        $('tbody').load('/Orders/Search?queryUserName=' + queryUserName + "&phoneNumber=" + phoneNumber + "&postalCode=" + postalCode);
    });
});