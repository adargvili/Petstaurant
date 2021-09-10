$(function () {
    $('#orderValidation').on("submit", function (e) {
        var ad = $('#address').val();
        var adEr = $('#addressError');
        var c = false;
        var a = ad.split(" ");
        if (a.length < 2 || a.length>4) {
            c = true;
            console.log("test 1")
        }
        for (var i = 0; i < a.length; i++) {
            if (/^(?=.*[A-Z])(?=.*[0-9])[A-Z0-9]+$/.test(a[i])) {
                c = true;
                console.log("test 2")
                    break;
            }
        }
        if (parseInt(a[0]) || !parseInt(a[a.length - 1]) || (ad.match(/([\s]+)/g).length > a.length-1)) {
            c = true;
            console.log((ad.search(/\s/) > a.length - 1))
        }

    if (c || ad.startsWith(" ") || ad.endsWith(" ") || isNaN(ad.slice(-1) || ad.includes(",") || !ad.includes(" ") || parseInt(ad) || !(/\d/.test(ad)) || ad.match(/([\s]+)/g).length > 4)) {
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