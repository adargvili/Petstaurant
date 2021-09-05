
//AddOne
$(document).ready(function () {
	$('.add').on("click", function (e) {
		e.preventDefault();
		console.log(e);
		var ciQuantity = parseInt($(this).closest("tr").find('input').prop('value'));
		var dishPrice = parseInt($(this).closest('tr').find('.dishPrice').text());
		var ciPrice = parseInt($(this).closest('tr').find(".ciPrice").text());
		$(this).closest("tr").find('input').val(ciQuantity + 1).toString();
		$(this).closest('tr').find(".ciPrice").text((ciPrice + dishPrice).toString());
		const elements = document.querySelectorAll('.ciPrice');
		let total = 0;
		elements.forEach((el, i) => {
			const val = el.innerHTML.replace(/[^0-9]/, '');
			total += parseInt(val);
		});
		document.getElementById("toPrice").innerHTML =total.toString();
    });
});

//SubtractOne
$(document).ready(function () {
	$('.sub').on("click", function (e) {
		e.preventDefault();
		console.log(e);
		var ciQuantity = parseInt($(this).closest("tr").find('input').prop('value'));
		console.log(ciQuantity);
		if (ciQuantity <= 0) {
			return;
		}
		var dishPrice = parseInt($(this).closest('tr').find('.dishPrice').text());
		var ciPrice = parseInt($(this).closest('tr').find(".ciPrice").text());
		if (ciPrice <= 0) {
			return;
		}
		$(this).closest("tr").find('input').val(ciQuantity - 1).toString();
		$(this).closest('tr').find(".ciPrice").text((ciPrice - dishPrice).toString());
		const elements = document.querySelectorAll('.ciPrice');
		let total = 0;
		elements.forEach((el, i) => {
			const val = el.innerHTML.replace(/[^0-9]/, '');
			total += parseInt(val);
		});
		document.getElementById("toPrice").innerHTML =total.toString();
	});
});