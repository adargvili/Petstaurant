$(function () {
    $('#search-form').on("submit", function (e) {
        e.preventDefault();

        var queryUserName = encodeURIComponent($('#queryUserName').val());
        var queryName = encodeURIComponent($('#queryName').val());
        var selectType = encodeURIComponent($('#selectType').val());

        $('tbody').load('/Users/Search?queryUserName=' + queryUserName + "&queryName=" + queryName + "&selectType=" + selectType);
    });
});