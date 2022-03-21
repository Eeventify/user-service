using Abstraction_Layer;
using DAL_Layer;

namespace Factory_Layer
{
    public static class IUserCollectionFactory
    {
        static public IUserCollection Get()
        {
            return new UserDAL();
        }

    }
}