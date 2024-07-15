
var modalDisable = new bootstrap.Modal(document.getElementById('DisableProductModal'), {
    keyboard: true
});
var changePriceModal = new bootstrap.Modal(document.getElementById('UpdateProductModal'), {
    keyboard: false
});
var editModal = new bootstrap.Modal(document.getElementById('EditarProductModal'), {
    keyboard: false
});
var quantityModal = new bootstrap.Modal(document.getElementById('ProductModal'), {
    keyboard: false
});

function quantityOpenModal(id, nombre) {
   document.getElementById('idProductoQuantity').innerHTML = id;
   document.getElementById('nombreProductoQuantity').innerHTML = nombre;
   document.getElementById('typeQuantity').value = "0"; 

    quantityModal.show();
}

function ChangePriceProductoModal(id, nombre) {
    document.getElementById('idProductoUpdate').innerHTML = id;
    document.getElementById('nombreProductoUpdate').innerHTML = nombre;

   document.getElementById('typeUnitPrice').value = "0"; 
    changePriceModal.show();
}


function DeleteProductoModal(id, nombre) {
    // Almacena el ID del producto en el modal
    document.getElementById('idProductDisable').innerHTML = id;
    document.getElementById('nombreProductoDisable').innerHTML = nombre;
    modalDisable.show();
}

function EditProductoModal(id, nombre) {
    document.getElementById('idProductoEdit').innerHTML = id;
    document.getElementById('nombreProductoEdit').innerHTML = nombre;

    editModal.show();
}

// ================
function AlmacenaProducto(id, nombre) {
    var idProducto = parseInt(document.getElementById('idProductoQuantity').innerHTML); 
    var nombre = document.getElementById('nombreProductoQuantity').innerHTML; 
    var quantity = parseFloat(document.getElementById('typeQuantity').value); 

    var data = JSON.stringify({
        IdProduct: idProducto,
        Quantity: quantity
    });

    $.ajax({
        url: '/Inventario/AlmacenaProducto',
        type: 'POST',
        dataType: 'json',
        data: data,
        contentType: 'application/json',
        success: function (response) {
            if (response.success) {
                alert(`Se ingreso ${quantity} del producto ${nombre}`);
                $('#tb_productos').DataTable().ajax.reload();
                changePriceModal.hide();
            }
        },
        error: function (xhr, status, error) {
            console.error(error);
        }
    });

}




function ChangePriceProducto(id, nombre) {
    var id = document.getElementById('idProductoUpdate').innerHTML ;
    var nombre = document.getElementById('nombreProductoUpdate').innerHTML ;
    var precio = document.getElementById('typeUnitPrice').value;

    
    var xhr = new XMLHttpRequest();
    xhr.open('POST', '/Inventario/ActualizarPrecio', true);
    xhr.setRequestHeader('Content-Type', 'application/json;charset=UTF-8');

	xhr.onload = function (){
        if (this.status == 200) {
            //var data = JSON.parse(this.responseText);
            alert(`Se cambio el precio (${precio}) del producto ${nombre}`)
             $('#tb_productos').DataTable().ajax.reload();
            changePriceModal.hide() 
        }
        else{
            alert("Hubo un problema")
        }
    }
    var data = JSON.stringify({
        ProductId: id,
        UnitPrice: precio
    });

    xhr.send(data); 
}


function DeleteProducto(id, nombre) {
    // Almacena el ID del producto en el modal
    var id = document.getElementById('idProductDisable').innerHTML; 
    var nombre = document.getElementById('nombreProductoDisable').innerHTML;

    var xhr = new XMLHttpRequest();
	xhr.open('POST', '/Inventario/DisableProduct?id=' + id, true);
	xhr.onload = function (){
        if (this.status == 200) {
            //var data = JSON.parse(this.responseText);
            alert("Se deshabilito el "+nombre)
            $('#tb_productos').DataTable().ajax.reload();
            modalDisable.hide() 

        }
        else{
            alert("Hubo un problema")
        }
    }
    xhr.send(); 
}

function EditProducto(id, nombre) {
    var id = document.getElementById('idProductoEdit').innerHTML; 
    var nombre = document.getElementById('nombreProductoEdit').innerHTML; 


    editModal.close();
}





