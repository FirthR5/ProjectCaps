using Caps_Project.Migrations;

namespace Caps_Project.Services
{
    public class BaseService
    {
        protected readonly DbCapsContext context;

        public BaseService(DbCapsContext context )
        {
            this.context = context;
        }
    }
}
