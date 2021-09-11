$(function () {
    $('#dish-filter-form').on("submit", function (e) {
        e.preventDefault();

        var dishName = encodeURIComponent($('#dishName').val());
        var selectFoodType = encodeURIComponent($('#selectFoodType').val());
        var cities = [];

        $('.form-check-input').each(function (index, element) {
            if ($(element).is(':checked')) {
                cities.push($(element).val());
            }
        });

        var encodedCities = encodeURIComponent(JSON.stringify(cities));

        $('.toLoad').load('/Dishes/Search?dishName=' + dishName + "&cities=" + encodedCities + "&selectFoodType=" + selectFoodType);
    });
});