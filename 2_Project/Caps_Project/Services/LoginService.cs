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

        public async Task<int> VerificarCredenciales(LoginDTO credenciales)
        {
            //Input Parameter
            var IdEmpleadoParameter = new SqlParameter("IdEmpleado", credenciales.IdEmpleado);
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
        public async Task<int> VerificarUsuarioActivo(string idEmpleado)
        {
            var idEmpleadoParam = new SqlParameter("@IdEmpleado", idEmpleado);
            var countParam = new SqlParameter
            {
                ParameterName = "@Count",
                SqlDbType = System.Data.SqlDbType.Int,
                Direction = System.Data.ParameterDirection.Output
            };

            await context.Database.ExecuteSqlRawAsync(
                "EXEC @Count = VerificarUsuarioActivo @IdEmpleado",
                countParam,
                idEmpleadoParam
            );

            return (int)countParam.Value;
        }
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
