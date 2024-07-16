using AutoMapper;
using Caps_Project.DTOs;
using Caps_Project.DTOs.EmpleadoDTOs;
using Caps_Project.Models;
using Caps_Project.Models.View.DTOs.LoginDTOs;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace Caps_Project.Services
{
    public class EmpleadoService : BaseService
    {
        public EmpleadoService(DbCapsContext context) : base(context) { }
        public EmpleadoService(DbCapsContext context, IMapper mapper) : base(context, mapper) { }

        // TODO: Hacer Obtener Lista de Empleados
        public async Task<PaginationUsuarioDTO> ListaEmpleados(PaginationDTO paginationDTO)
        {
            var query = context.VwDatosUsuarios;

            paginationDTO.recordsTotal = await query.CountAsync();
            PaginationUsuarioDTO paginationProductoDTO = new PaginationUsuarioDTO()
            {
                paginationDTO = paginationDTO
            };

            paginationProductoDTO.listUsuarios = await query.Skip(paginationDTO.skip)
                .Take(paginationDTO.pageSize).ToListAsync();

            return paginationProductoDTO;
        }

        /// <summary>
        /// Obtener Lista de Tipos de Empleados para usarlo como Combobox
        /// </summary>
        /// <returns>Lista de Tipo Empleados</returns>
        public async Task<List<TipoEmpleado>> List_TipoEmpleado() 
            => await context.TipoEmpleados.ToListAsync();

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

        public async Task<bool> ActivarUsuario(ActivarUsuarioDTO usuarioDatosDTO)
        {
            bool isExecuted;
            await context.Database.ExecuteSqlRawAsync(
                "EXEC ActivarUsuario @IdEmpleado, @Turno",
                new SqlParameter("@IdEmpleado", usuarioDatosDTO.IdEmpleado),
                new SqlParameter("@Turno", usuarioDatosDTO.Turno)
            );
            isExecuted = true;
            return isExecuted;
        }

        public async Task<bool> DesactivarUsuario(string idEmpleado)
        {
            var empleado = context.EmpleadoActivos.FirstOrDefault(e => e.IdEmpleado == idEmpleado);

            if (empleado != null)
            {
                empleado.EndDate = DateTime.Now;

                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }
      
        public async Task<List<string>> ObtenerListaDeTurnos()
        {
            List<string> turnos = new List<string> { "Dia", "Noche", "Completo" }; ;

            return turnos;
        }


    }
}
