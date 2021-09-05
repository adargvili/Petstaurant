$(function () {
    $('#search-form').on("submit", function (e) {
        e.preventDefault();

        var queryUserName = $('#queryUserName').val();
        var queryName = $('#queryName').val();
        var selectType = $('#selectType').val();

        $('tbody').load('/Users/Search?queryUserName=' + queryUserName + "&queryName=" + queryName + "&selectType=" + selectType);
    });
});