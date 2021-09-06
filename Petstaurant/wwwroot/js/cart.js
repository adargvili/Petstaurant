
//AddOne
$(document).ready(function () {
    $('.add').on("click", function (e) {
        e.preventDefault();
        var ciId = parseInt($(this).closest('tr').prop('id'));
        //var ciQuantity = parseInt($(this).closest("tr").find('input').prop('value'));
        //var dishPrice = parseInt($(this).closest('tr').find('.dishPrice').text());
        //var ciPrice = parseInt($(this).closest('tr').find(".ciPrice").text());
        var th = this;
        $.ajax({
            type: 'POST',
            url: '/Carts/AddOne',
            data: {
                id: ciId,
            },
            success: function (data) {
                if (parseInt(data[0]) == -1) {
                    alert("Can't add this dish to cart 1");
                    return false;
                }
                $(th).closest("tr").find('input').val(data[2].toString());
                $(th).closest('tr').find(".ciPrice").text(data[0].toString() + "$");
                //const elements = document.querySelectorAll('.ciPrice');
                //let total = 0;
                //elements.forEach((el, i) => {
                //    const val = el.innerHTML.replace(/[^0-9]/, '');
                //    total += parseInt(val);
                //});
                document.getElementById("toPrice").innerHTML = "Total Price: " + data[1].toString()+"$";
                return true;
            },
            error: function (data) {
                return false;
            },
            complete: function (data) {
            },
        });

    });
});

//SubtractOne
$(document).ready(function () {
    $('.sub').on("click", function (e) {
        e.preventDefault();
        var ciId = parseInt($(this).closest('tr').prop('id'));
        //var ciQuantity = parseInt($(this).closest("tr").find('input').prop('value'));
        //console.log(ciQuantity);
        //if (ciQuantity <= 0) {
        //    return;
        //}
       //var dishPrice = parseInt($(this).closest('tr').find('.dishPrice').text());
       // var ciPrice = parseInt($(this).closest('tr').find(".ciPrice").text());
        //if (ciPrice <= 0) {
        //    return;
        //}

        var th = this;



        $.ajax({
            type: 'POST',
            url: '/Carts/SubtractOne',
            data: {
                id: ciId,
            },
            success: function (data) {
                if (parseInt(data[0]) == -1) {
                    alert("Can't add this dish to cart 3");
                    return false;
                }
                if (parseInt(data[0])==0) {
                    var tr = $(th).closest("tr");
                    document.getElementById("toPrice").innerHTML = "Total Price: " + data[1].toString() + "$";
                    tr.remove(); 
                    return true;
                }
                $(th).closest("tr").find('input').val(data[2].toString());
                $(th).closest('tr').find(".ciPrice").text(data[0].toString() + "$");
                //const elements = document.querySelectorAll('.ciPrice');
                //let total = 0;
                //elements.forEach((el, i) => {
                //    const val = el.innerHTML.replace(/[^0-9]/, '');
                //    total += parseInt(val);
                //});
                document.getElementById("toPrice").innerHTML = "Total Price: " + data[1].toString() + "$";
                return true;
            },
            error: function (data) {
                return false;
            },
            complete: function (data) {
            },
        });

    });
});

//RemoveCartItem
$(document).ready(function () {
    $('.remove').on("click", function (e) {
        e.preventDefault();
        var ciId = parseInt($(this).closest('tr').prop('id'));
        var th = this;
        $.ajax({
            type: 'POST',
            url: '/Carts/RemoveCartItem',
            data: {
                id: ciId,
            },
            success: function (data) {
                if (parseInt(data[0]) == 0) {
                    alert("Can't remove this cart item");
                    return false;
                }
                var tr = $(th).closest("tr");
                document.getElementById("toPrice").innerHTML = "Total Price: " + data[1].toString() + "$";
                tr.remove();
                return true;
            },
            error: function (data) {
                return false;
            },
            complete: function (data) {
            },
        });

    });
});