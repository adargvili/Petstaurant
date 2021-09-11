
//AddOne
$(document).ready(function () {
    $('.add').on("click", function (e) {
        e.preventDefault();
        var ciId = parseInt($(this).closest('tr').prop('id'));
        var th = this;
        $.ajax({
            type: 'POST',
            url: '/Carts/AddOne',
            data: {
                id: ciId,
            },
            success: function (data) {
                if (parseInt(data[0]) == -1) {
                    return false;
                }
                $(th).closest("tr").find('input').val(data[2].toString());
                $(th).closest('tr').find(".ciPrice").text(data[0].toString() + "₪");;
                document.getElementById("toPrice").innerHTML = "Subtotal: " + data[1].toString() +"₪";
                return false;
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
        var th = this;



        $.ajax({
            type: 'POST',
            url: '/Carts/SubtractOne',
            data: {
                id: ciId,
            },
            success: function (data) {
                if (parseInt(data[0]) == -1) {
                    return false;
                }
                if (parseInt(data[0])==0) {
                    var tr = $(th).closest("tr");
                    document.getElementById("toPrice").innerHTML = "Subtotal: " + data[1].toString() + "₪";
                    tr.remove(); 
                    return false;
                }
                $(th).closest("tr").find('input').val(data[2].toString());
                $(th).closest('tr').find(".ciPrice").text(data[0].toString() + "₪");
                document.getElementById("toPrice").innerHTML = "Subtotal: " + data[1].toString() + "₪";
                return false;
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
                    return false;
                }
                var tr = $(th).closest("tr");
                document.getElementById("toPrice").innerHTML = "Subtotal: " + data[1].toString() + "₪";
                tr.remove();
                return false;
            },
            error: function (data) {
                return false;
            },
            complete: function (data) {
            },
        });

    });
});


//AddDishToCart
$(document).ready(function () {
    $('.addToCart').on("click", function (e) {
        e.preventDefault();
        var diId = $(this).prop('id');
        var signed = $('#logged').val();
        if (signed === 'false') {
            $('#notLoggedIn').modal('show');
        }
        else {
            var th = this;
            $.ajax({
                type: 'POST',
                url: '/Carts/AddDishToCart',
                data: {
                    id: parseInt(diId),
                },
                success: function (data) {
                    if (!data) {
                        alert("Can't add to cart");
                        return false;
                    }
                    document.getElementById(diId).textContent = "Added to cart!";
                    setTimeout(() => {
                        document.getElementById(diId).textContent = "Add to cart";
                    }, 1500);
                    return false;
                },
                error: function (data) {
                    return false;
                },
                complete: function (data) {
                },
            });
        }

    });
});

//ClearCart
$(document).ready(function () {
    $('.clearCart').on("click", function (e) {
        e.preventDefault();
        var th = this;
        $.ajax({
            type: 'POST',
            url: '/Carts/ClearCart',
            data: {
            },
            success: function (data) {
                if (parseInt(data[0]) == 0) {
                    alert("There are no items in the cart");
                    return false;
                }
                var tbl = document.getElementById("tblId");
                tbl.removeChild(tbl.getElementsByTagName("tbody")[0]);
                document.getElementById("toPrice").innerHTML = "Subtotal: " + data[1].toString() + "₪";
                return false;
            },
            error: function (data) {
                return false;
            },
            complete: function (data) {
            },
        });

    });
});