
var modalDisable = new bootstrap.Modal(document.getElementById('DisableProductModal'), {
    keyboard: true
});
var changePriceModal = new bootstrap.Modal(document.getElementById('UpdateProductModal'), {
    keyboard: false
});
var editModal = new bootstrap.Modal(document.getElementById('EditarProductModal'), {
    keyboard: false
});


function openModal(tipoModal) {
    if (tipoModal == 1) {
        $.ajax({
            url: '/Inventario/RegistraNuevoProducto',
            type: 'GET',
            success: function (data) {
                $('#ProductModalBody').html(data);
                $('#ProductModal').modal('hide');
                $('#ProductModal').modal('show');

            },
            error: function () {
                alert('Error occurred while fetching data.');
            }
        });
    }
    if (tipoModal == 2) {
        $.ajax({
            url: '/Inventario/AlmacenaProducto',
            type: 'GET',
            success: function (data) {
                $('#ProductModalBody').html(data);
                $('#ProductModal').modal('hide');
                $('#ProductModal').modal('show');

            },
            error: function () {
                alert('Error occurred while fetching data.');
            }
        });

    }
}

function ChangePriceProductoModal(id, nombre) {
    document.getElementById('idProductoUpdate').innerHTML = id;
    document.getElementById('nombreProductoUpdate').innerHTML = nombre;

    changePriceModal.show();
    

    // Almacena el ID del producto en el modal
    // $('#updatePriceModal').data('id', id).modal('show');
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
    // Almacena el ID del producto en el modal
    // $('#updatePriceModal').data('id', id).modal('show');
}

// ================

function ChangePriceProducto(id, nombre) {
    var id = document.getElementById('idProductoUpdate').innerHTML ;
    var nombre = document.getElementById('nombreProductoUpdate').innerHTML ;
    var precio = document.getElementById('typeUnitPrice').innerHTML;

    
    var xhr = new XMLHttpRequest();
	xhr.open('POST', '/Inventario/DisableProduct?id=' + id, true);
    xhr.setRequestHeader('Content-Type', 'application/json;charset=UTF-8');

	xhr.onload = function (){
        if (this.status == 200) {
            //var data = JSON.parse(this.responseText);
            alert("Se deshabilito el "+nombre)
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
    

    // Almacena el ID del producto en el modal
    // $('#updatePriceModal').data('id', id).modal('show');

    changePriceModal.hide();
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
        }
        else{
            alert("Hubo un problema")
        }
    }
    xhr.send(); 
    $('#tb_productos').DataTable().ajax.reload();
    modalDisable.hide() 
}

function EditProducto(id, nombre) {
    var id = document.getElementById('idProductoEdit').innerHTML; 
    var nombre = document.getElementById('nombreProductoEdit').innerHTML; 

    // Almacena el ID del producto en el modal
    // $('#updatePriceModal').data('id', id).modal('show');

    editModal.close();
}





