using Caps_Project.DTOs.LoginDTOs;
using Caps_Project.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

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
    }
}
