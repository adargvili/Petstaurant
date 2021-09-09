$(function () {
    $('#dish-filter-form').on("submit", function (e) {
        e.preventDefault();

        var dishName = encodeURIComponent($('#dishName').val());
        var selectFoodType = encodeURIComponent($('#selectFoodType').val());
        var cities = [];

        if ($('#TLV-Check').is(':checked')) {
            cities.push("Tel-Aviv");
        }
        if ($('#BS-Check').is(':checked')) {
            cities.push("Beer-Sheva");
        }
        if ($('#Jer-Check').is(':checked')) {
            cities.push("Jerusalem");
        }
        if ($('#KM-Check').is(':checked')) {
            cities.push("Kiryat-Malachi");
        }

        var encodedCities = encodeURIComponent(JSON.stringify(cities));
        console.log(encodedCities);

        $('.toLoad').load('/Dishes/Search?dishName=' + dishName + "&cities=" + encodedCities + "&selectFoodType=" + selectFoodType);
    });
});