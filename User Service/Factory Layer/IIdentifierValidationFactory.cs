using Abstraction_Layer;
using DAL_Layer;

namespace Factory_Layer
{
    public static class IIdentifierValidationFactory
    {
        static public IIdentifierValidation Get()
        {
            return new UserDAL();
        }

    }
}