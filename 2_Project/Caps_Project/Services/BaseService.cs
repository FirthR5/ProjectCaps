using Caps_Project.Models;

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
