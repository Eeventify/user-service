using Abstraction_Layer;
using DAL_Layer;

namespace Factory_Layer
{
    public static class IIdentifierRecursionCheckerFactory
    {
        static public IIdentifierRecursionChecker Get()
        {
            return new UserDAL();
        }

    }
}