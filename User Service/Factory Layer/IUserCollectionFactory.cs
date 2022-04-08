using Microsoft.EntityFrameworkCore;

using Abstraction_Layer;
using DAL_Layer;

namespace Factory_Layer
{
    public static class IUserRegistrationFactory
    {
        static public IUserRegistration Get(DbContext context)
        {
            return new UserDAL(context);
        }
    }
}