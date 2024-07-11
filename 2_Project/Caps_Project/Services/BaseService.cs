using AutoMapper;
using Caps_Project.Models;

namespace Caps_Project.Services
{
    public class BaseService
    {
        protected readonly DbCapsContext context;
        protected readonly IMapper mapper;

        public BaseService(DbCapsContext context )
        {
            this.context = context;
        }
        public BaseService(DbCapsContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
    }
}
