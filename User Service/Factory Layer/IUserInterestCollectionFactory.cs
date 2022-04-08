using Microsoft.EntityFrameworkCore;

using Abstraction_Layer;
using DAL_Layer;

namespace Factory_Layer
{
    public static class IUserInterestCollectionFactory
    {
        static public IUserInterestCollection Get(DbContext context)
        {
            return new UserInterestDAL(context);
        }
    }
}