$(function () {
    $('#storeValidation').on("submit", function (e) {
        var ad = $('#address').val();
        var adEr = $('#addressError');
        var c = false;
        var a = ad.split(" ");
        if (a.length < 2 || a.length>4) {
            c = true;
        }
        for (var i = 0; i < a.length; i++) {
            if (a[i].match(/.*([a-zA-Z].*[0-9]|[0-9].*[a-zA-Z]).*/)) {
                c = true;
                    break;
            }
        }
        if (parseInt(a[0])) {
            c = true;
        }

        if (c || ad.startsWith(" ") || ad.endsWith(" ") || ad.includes(",") || parseInt(ad) || ad.match(/\s/g).length > a.length) {
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