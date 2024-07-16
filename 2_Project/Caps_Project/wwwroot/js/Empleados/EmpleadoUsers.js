var disableUserModal = new bootstrap.Modal(document.getElementById('UserModal'), {
    keyboard: false
});

function DisableModalOpenModal(id, nombre) {
    document.getElementById('idToDisable').innerHTML = id;
    document.getElementById('nombreUsuario').innerHTML = nombre;

    disableUserModal.show();
}

function DisableUser(id, nombre) {
    var id = document.getElementById('idToDisable').innerHTML;
    var nombre = document.getElementById('nombreUsuario').innerHTML;


    var xhr = new XMLHttpRequest();
    xhr.open('GET', '/EmpleadosUsers/DeActivarEmpleado?IdEmpleado='+id, true);

    xhr.onload = function () {
        if (this.status == 200) {
            var data = JSON.parse(this.responseText);

            alert(`Se deshabilito el usuario con exito`)
            $('#tblEmpleados').DataTable().ajax.reload();
            disableUserModal.hide()
            document.getElementById('idToDisable').innerHTML = "";
            document.getElementById('nombreUsuario').innerHTML = "";
        }
        else {
            alert("Hubo un problema")
        }
    }

    xhr.send();
}



