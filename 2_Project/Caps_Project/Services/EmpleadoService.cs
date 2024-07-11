using Caps_Project.DTOs.EmpleadoDTOs;
using Caps_Project.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace Caps_Project.Services
{
    public class EmpleadoService : BaseService
    {
        public EmpleadoService(DbCapsContext context) : base(context) { }


        /// <summary>
        /// Obtener Lista de Tipos de Empleados para usarlo como Combobox
        /// </summary>
        /// <returns>Lista de Tipo Empleados</returns>
        public List<TipoEmpleado> List_TipoEmpleado() =>context.TipoEmpleados.ToList();

        public async Task<string> InsertarEmpleado(InsertEmpleadoDTO empleadoDTO)
        {
            var nuevoIdParam = new SqlParameter
            {
                ParameterName = "@NuevoID",
                SqlDbType = System.Data.SqlDbType.Char,
                Size = 10,
                Direction = System.Data.ParameterDirection.Output
            };

            await context.Database.ExecuteSqlRawAsync(
                "EXEC InsertarEmpleado @Nombre, @ApPaterno, @ApMaterno, @Contrasena, @EmployeeType, @NuevoID OUT",
                new SqlParameter("@Nombre", empleadoDTO.Nombre),
                new SqlParameter("@ApPaterno", empleadoDTO.ApPaterno),
                new SqlParameter("@ApMaterno", empleadoDTO.ApMaterno),
                new SqlParameter("@Contrasena", empleadoDTO.Contrasena),
                new SqlParameter("@EmployeeType", empleadoDTO.EmployeeType),
                nuevoIdParam
            );

            return (string)nuevoIdParam.Value;
        }
        public async Task ActivarUsuario(ActivarUsuarioDTO usuarioDatosDTO)
        {
            await context.Database.ExecuteSqlRawAsync(
                "EXEC ActivarUsuario @IdEmpleado, @Turno",
                new SqlParameter("@IdEmpleado", usuarioDatosDTO.IdEmpleado),
                new SqlParameter("@Turno", usuarioDatosDTO.Turno)
            );
        }

        public void DesactivarUsuario(string idEmpleado)
        {
            // Buscar el registro que quieres actualizar
            var empleado = context.EmpleadoActivos.FirstOrDefault(e => e.IdEmpleado == idEmpleado);

            if (empleado != null)
            {
                // Actualizar la propiedad EndDate
                empleado.EndDate = DateTime.Now;

                // Guardar los cambios en la base de datos
                context.SaveChanges();
            }
        }
      
        public async Task<List<string>> ObtenerListaDeTurnos()
        {
            List<string> turnos = new List<string> { "Dia", "Noche", "Completo" }; ;

            return turnos;
        }


    }
}
