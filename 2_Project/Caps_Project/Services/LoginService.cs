using Caps_Project.Migrations;

namespace Caps_Project.Services
{
    public class LoginService: BaseService
    {
        public LoginService(DbCapsContext context ) : base(context) { }
        public void prueba()
        {
            var x = context.Empleados.ToList();
            context.SaveChanges();
        }

    }
}
