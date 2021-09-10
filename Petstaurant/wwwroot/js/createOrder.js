$(function () {
    $('#orderValidation').on("submit", function (e) {
        var ad = $('#address').val();
        var adEr = $('#addressError');
        var c = false;
        var a = ad.split(" ");
        for (i in a) {
            if (/^(?=.*[A-Z])(?=.*[0-9])[A-Z0-9]+$/.test(a[i])) {
                c = true;
                    break;
            }
        }
        if (c || ad.startsWith(" ") || ad.endsWith(" ") || isNaN(ad.slice(-1) || ad.includes(",") || parseInt(ad) || !(/\d/.test(ad)) || ad.search(/\s/) > 5)) {
            e.preventDefault();
            adEr.text('Israeli address format is required (Example: Israel Galili 5)');
        }
        c = false;

        var po = $('#postalCode').val();
        var poE = $('#postalCodeError');
        if (po.startsWith("0")) {
            e.preventDefault();
            poE.text('Israeli postal code format is required');
        }
    });
});