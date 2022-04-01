using Microsoft.EntityFrameworkCore;

using Abstraction_Layer;
using DAL_Layer;

namespace Factory_Layer
{
    public static class IUserCollectionFactory
    {
        static public IUserCollection Get(DbContext context)
        {
            return new UserEFDAL(context);
        }

    }
}