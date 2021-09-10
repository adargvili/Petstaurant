$(function () {
    $('#dishValidation').on("submit", function (e) {
        var ad = $('#name').val();
        var adEr = $('#nameError');
        var c = false;
        var a = ad.split(" ");
            if (a.length < 1 || a.length > 4) {
                c = true;
        }
        console.log(a.length)
        if (c || !ad.match(/^[a-zA-Z\s]*$/) || ad.startsWith(" ") || ad.endsWith(" ") || (ad.search(/\S/) > a.length - 1)) {
            e.preventDefault();
            adEr.text('Please enter a valid food group name');
        }
        c = false;

    });
});