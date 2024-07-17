$('.btnMostrar').click(function (event) {
    var _Parent = $(this).parent("div.los_controles");
    var input = $(_Parent).find("button.btn-agregar-carrito");
    var botones = $(_Parent).find("div.controles");
    $(this).hide()

    botones.children().show();
    input.show();
});

$(document).on('click', '.btn-sumar', function (event) {
    var div = $(this).parent("div.controles");
    var inputCantidad = $(div).find("input.input-cantidad");
    var card = $(div).parent("div.los_controles");
    var stockInput = $(card).find("input.stock-value");
    var stockValue = stockInput.val();

    // console.log(stockInput);
    var cantidad = parseInt($(inputCantidad).val()) + 1;
    if (cantidad > stockValue) {
        alert("No puedes superar el stock disponible.");
        return;
    }
    $(inputCantidad).val(cantidad);
});

$(document).on('click', '.btn-restar', function (event) {
    var div = $(this).parent("div.controles");
    var input = $(div).find("input.input-cantidad");
    var cantidad = parseInt($(input).val()) - 1;
    if (cantidad >= 1) {
        $(input).val(cantidad);
    }
});

function AddToCart(item) {
    var items = new Object();
    items.ProductId = parseInt($(item).attr("itemid"));
    // items.PrecioUnitario = parseFloat($(item).attr("itemPrice"));
    // items.Nombre = $(item).attr("itemName");

    var divItem = $(item).parent("div.los_controles")
    var controlesItem = $(divItem).find("div.controles")
    var inputCantidad = $(controlesItem).find("input.input-cantidad")[0].value
    items.Quantity = parseInt(inputCantidad)
    $("#cartItem").text("Cart( " + items.Quantity + " )");
    console.log(items)



    $.ajax({
        type: 'POST',
        async: true,
        url: '/Productos/AgregarAlCarrito',
        data: JSON.stringify(items),
        contentType: "application/json",
        dataType: "json",
        processData: false,
        success: function (data) {

            if (data.success) {
                console.log(data.success)
                $(controlesItem).find("input.input-cantidad")[0].value = "0"


            }
        },
        error: function () {
            alert("Hay un problema.")
        }
    })


    const _Parent = $(item).parent("div.los_controles")
    var _Controles = $(_Parent).find("div.controles")
    _Controles.children().hide()
    divItem.children().hide()

}