using Microsoft.EntityFrameworkCore;

using Abstraction_Layer;
using DAL_Layer;

namespace Factory_Layer
{
    public static class IUserEventCollectionFactory
    {
        static public IUserEventCollection Get(DbContext context)
        {
            return new UserEventDAL(context);
        }
    }
}