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
            adEr.text('Please enter a valid dish name');
        }
        c = false;

        var p = $('#price').val();
        var po = $('#priceError');
        if (parseInt(p) == 0) {
            e.preventDefault();
            po.text('Choose a postive price');
        }

        var de = $('#description').val();
        var deE = $('#descriptionError');
        var d = de.split(" ");
        if (!de.match(/^[\w .,!?]+$/) || de.startsWith(" ") || de.endsWith(" ") || (de.search(/\S/) > d.length - 1)) {
            e.preventDefault();
            deE.text('Please enter a valid description');
        }

        var st = $('#store').val();
        var stE = $('#storeError');
        if (!st.length) {
            e.preventDefault();
            stE.text('You have to choose at least one store.');
        }

        var fd = $('#foodgroup').val();
        var fdE = $('#foodgroupError');
        if (!fd.length) {
            e.preventDefault();
            fdE.text('You have to choose foodgroup.');
        }
    });
});