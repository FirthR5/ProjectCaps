using Caps_Project.DTOs.LoginDTOs;
using Caps_Project.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;

namespace Caps_Project.Services
{
    public class LoginService: BaseService
    {
        public LoginService(DbCapsContext context ) : base(context) { }

        /// <summary>
        /// Verificar Credenciales al hacer Login
        /// </summary>
        /// <param name="credenciales"></param>
        /// <returns>0=False, 1=Verdadero</returns>
        public async Task<int> VerificarCredenciales(LoginDTO credenciales)
        {
            //Input Parameter
            var IdEmpleadoParameter = new SqlParameter("IdEmpleado", credenciales.UserName);
            var ContrasenaParameter = new SqlParameter("Contrasena", credenciales.Contrasena);
            //Output Parameter
            var correctoParam = new SqlParameter
            {
                ParameterName = "Correcto", SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Output
            };

            context.Database.ExecuteSqlRaw("EXEC VerificarCredenciales @IdEmpleado,@Contrasena, @Correcto OUTPUT",
                IdEmpleadoParameter, ContrasenaParameter, correctoParam);

            int Exitoso = (int)correctoParam.Value;
            return Exitoso;
        }
        /// <summary>
        /// Verificar Usuario esta Activo en el Sistema
        /// </summary>
        /// <param name="idEmpleado"></param>
        /// <returns>0=False, 1=Verdadero</returns>
        public async Task<int> VerificarUsuarioActivo(string idEmpleado)
        {
            int Activo;
            var idEmpleadoParam = new SqlParameter("@IdEmpleado", idEmpleado);
            //var ActivParamo = new SqlParameter("Contrasena", Activo);

            var ActivParamo = new SqlParameter
            {
                ParameterName = "Activo",
                SqlDbType = System.Data.SqlDbType.Int,
                Direction = System.Data.ParameterDirection.Output
            };

            await context.Database.ExecuteSqlRawAsync(
                "EXEC VerificarUsuarioActivo @IdEmpleado, @Activo OUTPUT",
                idEmpleadoParam,
                ActivParamo
            );

            return (int)ActivParamo.Value;
        }
        /// <summary>
        /// Credenciales del Empleado
        /// Darle las Credenciales para poder iniciar sesion
        /// Y Guardar informacion (Nombre, ID, Tipo Usuario)
        /// </summary>
        /// <param name="idEmpleado"></param>
        /// <returns></returns>
        public async Task<ClaimsIdentity> CredencialesEmpleado(string idEmpleado)
        {
            var reqTipoEmp = await context.Empleados.FirstOrDefaultAsync(e => e.IdEmpleado == idEmpleado);
            var tipoEmpl = reqTipoEmp.EmployeeType;
            var reqTipo = await context.TipoEmpleados.FirstOrDefaultAsync(t=>t.IdEmployeeType == tipoEmpl);
            string tipoEmpleado = reqTipo.EmpTypeName;

            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, idEmpleado),
                new Claim(ClaimTypes.Role, tipoEmpleado)//TODO: o ponerle el numerito?
            };
            claims.Add(new Claim("Nombre", $"{reqTipoEmp.Nombre} {reqTipoEmp.ApPaterno} {reqTipoEmp.ApMaterno}"));
            ClaimsIdentity claimIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            return claimIdentity;
        }
    }
}
