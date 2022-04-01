using Microsoft.EntityFrameworkCore;

using Abstraction_Layer;
using DAL_Layer;


namespace Factory_Layer
{
    public static class IIdentifierRecursionCheckerFactory
    {
        static public IIdentifierRecursionChecker Get(DbContext context)
        {
            return new UserEFDAL(context);
        }
    }
}